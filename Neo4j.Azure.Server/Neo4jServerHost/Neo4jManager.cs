using System;
using System.Diagnostics;
using System.IO;
using Diversify.WindowsAzure.ServiceRuntime;
using Diversify.WindowsAzure.ServiceRuntime.Utils;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.StorageClient;
using Neo4j.Azure.Server.Utils;

namespace Neo4j.Azure.Server
{
	public class Neo4jManager
	{
		private IZipping zipping;
		private IFileManipulation fileManipulation;

		internal Neo4jManager() { }

		public Neo4jManager(IFileManipulation fileManipulation, IZipping zipping)
		{
			this.zipping = zipping;
			this.fileManipulation = fileManipulation;
		}

		public void Install(IPaths paths, CloudStorageAccount storageAccount, int port, ICloudDriveManager cloudDriveManager)
		{
			Trace.TraceInformation("Installing Neo4j server.");

			DownloadJava(paths, storageAccount);
            UnzipJava(paths);

			DownloadNeo4j(paths, storageAccount);
			UnzipNeo4j(paths);

            try
            {
                // try to mount cloud drive
                MountDatabase(paths, cloudDriveManager);
                SetServerDbPathConfig(paths, port);
            }
            catch (Exception ex)
            {
                Trace.TraceError("Drive Mounting Error: " + ex.Message);
            }
            
			SetServerPortAndUrlConfig(paths, port);
			SetJavaPath(paths);
			CopyConfigurationFilesToLogsDirectory(paths);

			ClearLogDirectory(paths);

			Trace.TraceInformation("Neo4j server installed.");
		}

		internal void CopyConfigurationFilesToLogsDirectory(IPaths paths)
		{
			Trace.TraceInformation("Copying Neo4j Configuration files to the log directory.");

			var logDirectory = new DirectoryInfo(paths.LocalNeo4jLogsPath);

			var neo4JServerConfigFilePath = paths.Neo4jServerConfigPath;
			File.Copy(neo4JServerConfigFilePath, Path.Combine(logDirectory.FullName, Path.GetFileName(neo4JServerConfigFilePath)));

			var neo4JWrapperConfigPath = paths.Neo4jWrapperConfigPath;
			File.Copy(neo4JWrapperConfigPath, Path.Combine(logDirectory.FullName, Path.GetFileName(neo4JWrapperConfigPath)));

			Trace.TraceInformation("Done copying Neo4j Configuration files to the log directory.");
		}

		internal void ClearLogDirectory(IPaths paths)
		{
			var localNeo4JLogsPath = paths.LocalNeo4jLogsPath;

			Trace.TraceInformation(string.Format("Clearing the Neoj4 log directory <{0}>.", localNeo4JLogsPath));

			var logDirectory = new DirectoryInfo(localNeo4JLogsPath);
			foreach (var fileInfo in logDirectory.GetFiles())
				fileInfo.Delete();

			Trace.TraceInformation(string.Format("Done clearing the Neoj4 log directory <{0}>.", localNeo4JLogsPath));
		}

		internal void MountDatabase(IPaths paths, ICloudDriveManager cloudDriveManager)
		{
			Trace.TraceInformation("Mounting the Neoj4 database blob drive.");

            paths.MountDrivePath = cloudDriveManager.MountDrive(ConfigSettings.StorageConnectionString, paths.Neo4jDBDriveBlobRelativePath, paths.LocalNeo4jDababaseResourceName, paths.Neo4jDBDriveSize);

			Trace.TraceInformation("Done mounting the Neoj4 database blob drive.");
		}

		internal void SetJavaPath(IPaths paths)
		{
			var localJavaPath = paths.LocalJavaExePath;
			var fileName = paths.Neo4jWrapperConfigPath;

			Trace.TraceInformation(string.Format("Setting path to Java in Neo4j to '{0}' in the file <{1}>.", localJavaPath, fileName));

			var wrapperJavaCommand = paths.Neo4jWrapperSettingJavaCommand;
			var patternToFind = string.Format("{0}=", wrapperJavaCommand);
			var lineToInsert = string.Format("{0}{1}", patternToFind, localJavaPath);
			var replacement = Replacement.Create(patternToFind, lineToInsert);

			fileManipulation.ReplaceConfigLine(fileName, replacement);

			Trace.TraceInformation(string.Format("Finished setting path to Java in Neo4j to '{0}'.", localJavaPath));
		}

		internal void SetServerPortAndUrlConfig(IPaths paths, int port)
		{
			Trace.TraceInformation("Setting Neo4j server port and uri settings.");

			var neo4JServerConfigSettings = paths.Neo4jServerConfigSettings;

			var patternToFind = string.Format("{0}=", neo4JServerConfigSettings.Port);
			var lineToInsert = string.Format("{0}{1}", patternToFind, port);
			var replacement1 = Replacement.Create(patternToFind, lineToInsert);

			const string portPattern = "%%port%%";
			string portString = port.ToString();

			string neo4JAdminDataUri = paths.Neo4jAdminDataUri;
			patternToFind = string.Format("{0}=", neo4JServerConfigSettings.WebAdminDataUri);
			string webAdminDataUri = neo4JAdminDataUri.Replace(portPattern, portString);
			lineToInsert = string.Format("{0}{1}", patternToFind, webAdminDataUri);
			var replacement2 = Replacement.Create(patternToFind, lineToInsert);

			string neo4JAdminManagementUri = paths.Neo4jAdminManagementUri;
			patternToFind = string.Format("{0}=", neo4JServerConfigSettings.WebAdminManagementUri);
			string webAdminManagementUri = neo4JAdminManagementUri.Replace(portPattern, portString);
			lineToInsert = string.Format("{0}{1}", patternToFind, webAdminManagementUri);
			var replacement3 = Replacement.Create(patternToFind, lineToInsert);

		    patternToFind = "#org.neo4j.server.webserver.address=0.0.0.0";
            lineToInsert = "org.neo4j.server.webserver.address=0.0.0.0";
            var replacement4 = Replacement.Create(patternToFind, lineToInsert);

			var fileName = paths.Neo4jServerConfigPath;
            fileManipulation.ReplaceConfigLine(fileName, replacement1, replacement2, replacement3, replacement4);

			Trace.TraceInformation("Finished setting Neo4j server port and uri settings.");
		}

		internal void SetServerDbPathConfig(IPaths paths, int port)
		{
			Trace.TraceInformation("Setting Neo4j server database path setting.");

			var patternToFind = string.Format("{0}=", paths.Neo4jServerConfigSettings.DatabaseLocation);
			string dbPath = paths.MountDrivePath + paths.LocalNeo4jDababaseResourceName;
			var lineToInsert = string.Format("{0}{1}", patternToFind, dbPath);
			var replacement = Replacement.Create(patternToFind, lineToInsert);

			var fileName = paths.Neo4jServerConfigPath;
			fileManipulation.ReplaceConfigLine(fileName, replacement);

			Trace.TraceInformation("Finished setting Neo4j server database path settings.");
		}

		internal void UnzipNeo4j(IPaths paths)
		{
		    try
			{
				Trace.TraceInformation("Unzipping Neo4j service.");

				string localNeo4JZip = paths.LocalNeo4jZip;
				var localNeo4JPath = paths.LocalNeo4jPath;
				zipping.Extract(localNeo4JZip, localNeo4JPath);
			}
			catch (Exception e)
			{
				Trace.Fail(string.Format("Error unzipping Neo4j: type <{0}> message <{1}> stack trace <{2}>.", e.GetType().FullName, e.Message, e.StackTrace));
				throw;
			}

			//File.Delete(localNeo4JZip);
			Trace.TraceInformation("Neo4j service unzipped.");
		}

		internal void UnzipJava(IPaths paths)
		{
			string localJavaZip;
			try
			{
				Trace.TraceInformation("Unzipping java.");

				localJavaZip = paths.LocalJavaZip;
				var localJavaPath = paths.LocalJavaPath;
				zipping.Extract(localJavaZip, localJavaPath);
			}
			catch (Exception e)
			{
				Trace.Fail(string.Format("Error unzipping Neo4j: type <{0}> message <{1}> stack trace <{2}>.", e.GetType().FullName, e.Message, e.StackTrace));
				throw;
			}

			//File.Delete(localJavaZip);
			Trace.TraceInformation("Java unzipped.");
		}

		internal void DownloadNeo4j(IPaths paths, CloudStorageAccount storageAccount)
		{
			try
			{
				var localNeo4JZipPath = paths.LocalNeo4jZip;

				if (File.Exists(localNeo4JZipPath))
				{
                    if (paths.AlwaysDownloadFile)
                    {
                        return;
                    }
                    else
                    {
                        File.Delete(localNeo4JZipPath);
                    }
				}

				Trace.TraceInformation("Downloading Neo4j installer.");

				var blobClient = new CloudBlobClient(storageAccount.BlobEndpoint, storageAccount.Credentials);

				string blobStoragePath = storageAccount.BlobEndpoint.AbsoluteUri;
				string neo4jBlobRelativePath = RoleEnvironment.GetConfigurationSettingValue(ConfigSettings.Neo4jBlobNameSetting);

				string neo4jBlobAddress = blobStoragePath.CombineUris(neo4jBlobRelativePath);

				var blob = blobClient.GetBlobReference(neo4jBlobAddress);

			    var option = new BlobRequestOptions();
			    option.Timeout = new TimeSpan(0, 15, 0);
                blob.DownloadToFile(localNeo4JZipPath, option);

				Trace.TraceInformation("Neo4j installer downloaded.");
			}
			catch (Exception e)
			{
				Trace.Fail(string.Format("Error downloading Neo4j: type <{0}> message <{1}> stack trace <{2}>.", e.GetType().FullName, e.Message, e.StackTrace));
				throw;
			}
		}

		public void DownloadJava(IPaths paths, CloudStorageAccount storageAccount)
		{
			try
			{
				var localJavaZipPath = paths.LocalJavaZip;

				if (File.Exists(localJavaZipPath))
				{
					File.Delete(localJavaZipPath);
					//return;
				}

				Trace.TraceInformation("Downloading Java.");

				var blobClient = new CloudBlobClient(storageAccount.BlobEndpoint, storageAccount.Credentials);

				string blobStoragePath = storageAccount.BlobEndpoint.AbsoluteUri;
				string jBlobRelativePath = RoleEnvironment.GetConfigurationSettingValue(ConfigSettings.JavaBlobNameSetting);

				string javaBlobAddress = blobStoragePath.CombineUris(jBlobRelativePath);

				var blob = blobClient.GetBlobReference(javaBlobAddress);
                
                var option = new BlobRequestOptions();
                option.Timeout = new TimeSpan(0, 15, 0);
				blob.DownloadToFile(localJavaZipPath, option);

				Trace.TraceInformation("Java downloaded.");
			}
			catch (Exception e)
			{
				Trace.Fail(string.Format("Error downloading Java: type <{0}> message <{1}> stack trace <{2}>.", e.GetType().FullName, e.Message, e.StackTrace));
				throw;
			}
		}

		public void Start(IPaths paths)
		{
			Trace.TraceInformation("Starting the Neo4j service.");

			string neo4JRoot = paths.LocalNeo4jPath, neo4JRelativePath = paths.Neo4jExePath;

			var process = new Process
			{
				StartInfo = new ProcessStartInfo(neo4JRelativePath)
				{
					RedirectStandardInput = true, RedirectStandardOutput = true,
					RedirectStandardError = true, UseShellExecute = false,
					WindowStyle = ProcessWindowStyle.Hidden, WorkingDirectory = neo4JRoot,
				}

			};

            process.StartInfo.EnvironmentVariables["JAVA_HOME"] = paths.LocalJavaHomePath;
		    process.StartInfo.EnvironmentVariables["javaVersion"] = "1.7";
            if (process.StartInfo.EnvironmentVariables.ContainsKey("Path") && process.StartInfo.EnvironmentVariables["Path"].Length > 0)
            {
                process.StartInfo.EnvironmentVariables["Path"] += ";" + paths.LocalJavaHomePath + "\\bin";
            }
            else
            {
                process.StartInfo.EnvironmentVariables["Path"] = paths.LocalJavaHomePath + "\\bin";
            }

			process.Exited += (sender, e) => Trace.TraceInformation("Neo4j service process exited.");
			process.ErrorDataReceived += (sender, e) => Trace.TraceError(e.Data);
			process.OutputDataReceived += (sender, e) => Trace.TraceInformation(e.Data);

			try
			{
				process.Start();
				process.BeginOutputReadLine();
				process.BeginErrorReadLine();
				process.WaitForExit();
			}
			catch (Exception e)
			{
				Trace.Fail(string.Format("Error running Neo4j: type <{0}> message <{1}> stack trace <{2}>.", e.GetType().FullName, e.Message, e.StackTrace));
				throw;
			}
			Trace.TraceInformation("Neo4j service running.");
		}

		internal DirectoryConfiguration GetLogDirectory(IPaths paths)
		{
			var localNeo4JLogsPath = paths.LocalNeo4jLogsPath;
			return new DirectoryConfiguration
			{
				Container = paths.Neo4jLogsContainerName,
				DirectoryQuotaInMB = 1000,
				Path = localNeo4JLogsPath
			};
		}
	}
}
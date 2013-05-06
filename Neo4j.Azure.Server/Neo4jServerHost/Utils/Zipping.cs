using System.Diagnostics;
using System.IO;
using Ionic.Zip;

namespace Neo4j.Azure.Server.Utils
{
	public class Zipping : IZipping
	{
		public void Extract(string zipFileName, string unzipPath)
		{
			Trace.TraceInformation(string.Format("Unzipping zip-file '{0}' to location '{1}'.", zipFileName, unzipPath));

			var unzippedFiles = 0;
			using (var zipFile = ZipFile.Read(zipFileName))
			{
				foreach (var zipEntry in zipFile)
				{
					//Trace.TraceInformation(string.Format("Unzipping file '{0}.", zipEntry.FileName));
					try
					{
						zipEntry.Extract(unzipPath, ExtractExistingFileAction.OverwriteSilently);
					}
					catch (PathTooLongException ptle)
					{
						string errorTypeName = typeof(PathTooLongException).FullName;
						var totalPathLength = zipEntry.FileName.Length + unzipPath.Length;
						string errorMessage = ptle.Message;
						Trace.TraceError(string.Format("{0}: The path to unzip a file to is too long. File '{1}' to location '{2}'. The total path would be {3} characters long. Error message: {4}", 
						                               errorTypeName, 
						                               zipEntry.FileName, 
						                               unzipPath, 
						                               totalPathLength,
						                               errorMessage));
						throw;
					}

					unzippedFiles++;
				}
			}
			Trace.TraceInformation(string.Format("Finished unzipping '{0}' files from zip-file '{1}' to location '{2}'.", unzippedFiles, zipFileName, unzipPath));
		}
	}
}
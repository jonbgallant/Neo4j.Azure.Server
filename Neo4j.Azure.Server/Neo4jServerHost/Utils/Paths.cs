using System;
using System.Diagnostics;
using System.IO;
using Diversify.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.ServiceRuntime;

namespace Neo4j.Azure.Server.Utils
{
	internal class Paths : IPaths
	{
		private INeo4jServerConfigSettings neo4JServerConfigSettings;
		private string neo4jExePath;

		private readonly ILocalResourceManager localResourceManager;
		ILocalResource localResource;

		public Paths(ILocalResourceManager localResourceManager)
		{
			this.localResourceManager = localResourceManager;
		}

		public string LocalRootPath
		{
			get
			{
				if (localResource == null)
				{
					localResource = localResourceManager.GetByConfigName(ConfigSettings.LocalNeo4jInstallation);
				}
				var rootPath = localResource.RootPath;
				Trace.TraceInformation(string.Format("The root path to the local resource '{0}' is '{1}'.",
																		 ConfigSettings.LocalNeo4jInstallation,
																		 rootPath));
				return rootPath;
			}
		}

		public virtual string LocalNeo4jZip
		{
			get
			{
                var neo4JFileName = RoleEnvironment.GetConfigurationSettingValue(ConfigSettings.Neo4jFileName);
				return Path.Combine(LocalRootPath, neo4JFileName);
			}
		}

		public virtual string LocalJavaZip
		{
			get
			{
                var javaFileName = RoleEnvironment.GetConfigurationSettingValue(ConfigSettings.JavaFileName);
				return Path.Combine(LocalRootPath, javaFileName);
			}
		}

		public string LocalJavaExePath
		{
			get
			{
				return Path.Combine(LocalJavaPath, RoleEnvironment.GetConfigurationSettingValue(ConfigSettings.JavaExePath));
			}
		}

        public string LocalJavaHomePath
        {
            get
            {
                return Path.Combine(LocalJavaPath, RoleEnvironment.GetConfigurationSettingValue(ConfigSettings.JavaHomePath));
            }
        }
		public string Neo4jServerConfigPath
		{
			get { return Path.Combine(LocalNeo4jPath, RoleEnvironment.GetConfigurationSettingValue(ConfigSettings.Neo4jServerConfigPath)); }
		}

		public string Neo4jWrapperConfigPath
		{
			get { return Path.Combine(LocalNeo4jPath, RoleEnvironment.GetConfigurationSettingValue(ConfigSettings.Neo4jWrapperConfigPath)); }
		}

		public string Neo4jAdminDataUri
		{
			get { return RoleEnvironment.GetConfigurationSettingValue(ConfigSettings.Neo4jDataUri); }
		}

		public string Neo4jAdminManagementUri
		{
			get { return RoleEnvironment.GetConfigurationSettingValue(ConfigSettings.Neo4jAdminUri); }
		}

		public int Neo4jPort
		{
			get
			{
				var port = RoleEnvironment.CurrentRoleInstance.InstanceEndpoints[ConfigSettings.Neo4jEndpoint].IPEndpoint.Port;
				Trace.TraceInformation(string.Format("The configured endpoint '{0}' is assigned the dynamic port {1}.", 
																						 ConfigSettings.Neo4jEndpoint,
				                                     port));
				return port;
			}
		}

		public INeo4jServerConfigSettings Neo4jServerConfigSettings
		{
			get { return neo4JServerConfigSettings ?? (neo4JServerConfigSettings = new Neo4jServerConfigSettings()); }
			internal set { neo4JServerConfigSettings = value; }
		}

		public string Neo4jWrapperSettingJavaCommand
		{
			get { return RoleEnvironment.GetConfigurationSettingValue(ConfigSettings.Neo4jWrapperSettingJavaCommand); }
		}

		public string Neo4jDBDriveBlobRelativePath
		{
			get { return RoleEnvironment.GetConfigurationSettingValue(ConfigSettings.Neo4jDBDriveBlobNameSetting); }
		}

		public string LocalNeo4jDababaseResourceName
		{
			get { return ConfigSettings.LocalNeo4jDatabaseResource; }
		}

		public string Neo4jExePath
		{
			get { return neo4jExePath ?? (neo4jExePath = Path.Combine(LocalNeo4jPath, RoleEnvironment.GetConfigurationSettingValue(ConfigSettings.Neo4jExePath))); }
		}

		public string LocalNeo4jLogsPath
		{
			get { return Path.Combine(LocalNeo4jPath, RoleEnvironment.GetConfigurationSettingValue(ConfigSettings.Neo4jLogFolder)); }
		}

		public string Neo4jLogsContainerName
		{
			get { return RoleEnvironment.GetConfigurationSettingValue(ConfigSettings.Neo4jLogsContainerName); }
		}

		public string LocalNeo4jPath
		{
//			get { return Path.Combine(LocalRootPath, "Neo4j"); }
			get { return LocalRootPath; }
		}

		public string LocalJavaPath
		{
			get { return LocalRootPath; }
//			get { return Path.Combine(LocalRootPath, "java"); }
		}


        public bool AlwaysDownloadFile
        {
            get { return RoleEnvironment.GetConfigurationSettingValue(ConfigSettings.Neo4jLogsContainerName).Equals("true", StringComparison.InvariantCultureIgnoreCase); }
        }
        public int Neo4jDBDriveSize
	    {
            get { return int.Parse(RoleEnvironment.GetConfigurationSettingValue(ConfigSettings.Neo4jDBDriveSize)); }
	    }
        public string MountDrivePath { get; set; }
    }
}
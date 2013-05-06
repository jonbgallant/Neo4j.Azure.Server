using Microsoft.WindowsAzure.ServiceRuntime;

namespace Neo4j.Azure.Server
{
	class Neo4jServerConfigSettings : INeo4jServerConfigSettings
	{
		private string port;
		private string webAdminDataUri;
		private string webAdminManagementUri;
		private string databaseLocation;

		public string Port
		{
			get { return port ?? (port = RoleEnvironment.GetConfigurationSettingValue(ConfigSettings.Neo4jServerSettingsPort)); }
		}

		public string WebAdminDataUri
		{
			get { return webAdminDataUri ?? (webAdminDataUri = RoleEnvironment.GetConfigurationSettingValue(ConfigSettings.Neo4jServerSettingsWebAdminDataUri)); }
		}

		public string WebAdminManagementUri
		{
			get { return webAdminManagementUri ?? (webAdminManagementUri = RoleEnvironment.GetConfigurationSettingValue(ConfigSettings.Neo4jServerSettingsWebAdminManagementUri)); }
		}

		public string DatabaseLocation
		{
			get { return databaseLocation ?? (databaseLocation = RoleEnvironment.GetConfigurationSettingValue(ConfigSettings.Neo4jServerSettingsDatabaseLocation)); }
		}

	}
}
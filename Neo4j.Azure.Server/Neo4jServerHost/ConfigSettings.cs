namespace Neo4j.Azure.Server
{
	internal static class ConfigSettings
	{
		public const string DiagnosticsConnectionString = "Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString";

		public const string StorageConnectionString = "Storage connection string";
		public const string Neo4jBlobNameSetting = "Neo4j blob uri";
		public const string Neo4jDBDriveBlobNameSetting = "Neo4j db drive blob uri";
		public const string JavaBlobNameSetting = "Java blob uri";

		public const string Neo4jServerSettingsPort = "Neo4j server setting names web server port";
		public const string Neo4jServerSettingsWebAdminDataUri = "Neo4j server setting names web admin data uri";
		public const string Neo4jServerSettingsWebAdminManagementUri = "Neo4j server setting names web admin management uri";
		public const string Neo4jServerSettingsDatabaseLocation = "Neo4j server setting names database location";
		public const string Neo4jWrapperSettingJavaCommand = "Neo4j wrapper setting java command";
        public const string Neo4jAlwaysDownloadFiles = "Neo4j always download files";

		public const string Neo4jServerConfigPath = "Neo4j server config path";
		public const string Neo4jWrapperConfigPath = "Neo4j wrapper config path";
		public const string Neo4jLogFolder = "Neo4j log folder";
		public const string Neo4jExePath = "Neo4j exe path";
		public const string Neo4jEndpoint = "Neo4j endpoint";

		public const string LocalNeo4jInstallation = "Neo4jInst";
		public const string Neo4jFileName = "Neo4j neo4j file name";
		public const string Neo4jLogsContainerName = "Neo4j logs blob container";
		public const string Neo4jAdminUri = "Neo4j Admin Uri";
		public const string Neo4jDataUri = "Neo4j Data Uri";
		public const string JavaFileName = "Neo4j java file name";
		public const string JavaExePath = "Java exe path";
	    public const string JavaHomePath = "Java home path";
		public const string LocalNeo4jDatabaseResource = "Neo4jDb";

	    public const string Neo4jDBDriveSize = "Neo4j db drive size";
	}
}
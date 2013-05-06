namespace Neo4j.Azure.Server.Utils
{
	public interface IPaths
	{
		string LocalRootPath { get; }

		string LocalNeo4jZip { get; }
		string LocalJavaZip { get; }

		string Neo4jExePath { get; }

		string Neo4jLogsContainerName { get; }

		string LocalNeo4jPath { get; }
		string LocalNeo4jLogsPath { get; }
		string LocalNeo4jDababaseResourceName { get; }
		string LocalJavaPath { get; }
		string LocalJavaExePath { get; }

		string Neo4jServerConfigPath { get; }
		string Neo4jWrapperConfigPath { get; }
		string Neo4jAdminDataUri { get; }
		string Neo4jAdminManagementUri { get; }
		int Neo4jPort { get; }
		INeo4jServerConfigSettings Neo4jServerConfigSettings { get; }
		string Neo4jWrapperSettingJavaCommand { get; }
		string Neo4jDBDriveBlobRelativePath { get; }

        bool AlwaysDownloadFile { get; }
        int Neo4jDBDriveSize { get; }
        string MountDrivePath { get; set; }
        string LocalJavaHomePath { get; }
	}
}
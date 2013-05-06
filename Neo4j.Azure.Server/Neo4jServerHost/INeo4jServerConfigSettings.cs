namespace Neo4j.Azure.Server
{
	public interface INeo4jServerConfigSettings
	{
		string Port { get; }
		string WebAdminDataUri { get; }
		string WebAdminManagementUri { get; }
		string DatabaseLocation { get; }
	}
}
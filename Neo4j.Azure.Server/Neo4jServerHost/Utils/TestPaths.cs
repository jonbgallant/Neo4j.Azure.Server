using Diversify.WindowsAzure.ServiceRuntime;

namespace Neo4j.Azure.Server.Utils
{
	class TestPaths : Paths
	{
		public TestPaths(ILocalResourceManager localResourceManager) : base(localResourceManager)
		{ }

		public override string LocalNeo4jZip
		{
			get { return @"D:\Code\Neo4j.Azure.Server\Zips\neo4j-1.2-windows.zip"; }
		}

		public override string LocalJavaZip
		{
			get { return @"D:\Code\Neo4j.Azure.Server\Zips\jre6.zip"; }
		}
	}
}
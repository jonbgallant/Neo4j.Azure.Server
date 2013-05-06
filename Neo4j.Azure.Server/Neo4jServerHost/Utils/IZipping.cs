namespace Neo4j.Azure.Server.Utils
{
	public interface IZipping
	{
		void Extract(string zipFileName, string unzipPath);
	}
}
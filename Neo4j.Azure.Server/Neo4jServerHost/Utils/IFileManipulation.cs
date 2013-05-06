namespace Neo4j.Azure.Server.Utils
{
	public interface IFileManipulation
	{
		void ReplaceConfigLine(string fileName, params Replacement[] replacements);
	}
}
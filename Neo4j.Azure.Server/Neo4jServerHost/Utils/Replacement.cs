namespace Neo4j.Azure.Server.Utils
{
	public class Replacement
	{
		public string PatternToFind { get; set; }
		public string LineToInsert { get; set; }

		private Replacement(string patternToFind, string lineToInsert)
		{
			PatternToFind = patternToFind;
			LineToInsert = lineToInsert;
		}

		public static Replacement Create(string patternToFind, string lineToInsert)
		{
			return new Replacement(patternToFind, lineToInsert);
		}
	}
}
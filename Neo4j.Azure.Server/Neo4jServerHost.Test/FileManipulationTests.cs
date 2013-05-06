using System;
using System.Diagnostics;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neo4j.Azure.Server.Test.Mocks;
using Neo4j.Azure.Server.Utils;

namespace Neo4j.Azure.Server.Test
{
	[TestClass]
	public class FileManipulationTests
	{
		[TestMethod]
		public void SetPort_three_locations()
		{
			var expectedUri = "http://foo.bar";
			var expectedPort = 123456;

			var neo4JServerConfigPath = Path.Combine(Environment.CurrentDirectory, "neo4j-server.properties");
			IPaths paths = new PathsMock(neo4JServerConfigPath, null);

			var pattern1ToFind = "org.neo4j.server.webserver.port=";
			var line1ToInsert = string.Format("{0}{1}", pattern1ToFind, expectedPort);

			var pattern2ToFind = "org.neo4j.server.webadmin.data.uri=";
			var line2ToInsert = string.Format("{0}{1}{2}db/data/", pattern2ToFind, expectedUri, expectedPort);

			var pattern3ToFind = "org.neo4j.server.webadmin.management.uri=";
			var line3ToInsert = string.Format("{0}{1}{2}", pattern3ToFind, expectedUri, expectedPort);

			// Test
			new FileManipulation().ReplaceConfigLine(paths.Neo4jServerConfigPath, 
																							 Replacement.Create(pattern1ToFind, line1ToInsert),
																							 Replacement.Create(pattern2ToFind, line2ToInsert),
																							 Replacement.Create(pattern3ToFind, line3ToInsert));

			Debug.WriteLine("Dumping the file and verifying:");
			string line;
			var changeCount = 0;
			using (var file = new StreamReader(neo4JServerConfigPath))
				while (!file.EndOfStream)
				{
					line = file.ReadLine();
					if (line == null) continue;
					Debug.WriteLine(line);
					
					if(line.StartsWith(pattern1ToFind))
					{
						if (line != line1ToInsert)
							Assert.Fail("Change 1 was not performed.");
						else
							changeCount++;
					}
					else if(line.StartsWith(pattern2ToFind))
					{
						if (line != line2ToInsert)
							Assert.Fail("Change 2 was not performed.");
						else
							changeCount++;
					}
					else if(line.StartsWith(pattern3ToFind))
					{
						if (line != line3ToInsert)
							Assert.Fail("Change 3 was not performed.");
						else
							changeCount++;
					}
				}
			Assert.AreEqual(3, changeCount, "Three config changes was not performed.");
			Debug.WriteLine("Done dumping the file.");
		}

		[TestMethod]
		public void SetDBDrive()
		{
			var expectedPath = "c:/foo/thedb";

			var neo4JServerConfigPath = Path.Combine(Environment.CurrentDirectory, "neo4j-server.properties");
			IPaths paths = new PathsMock(neo4JServerConfigPath, null);

			var pattern1ToFind = "org.neo4j.server.database.location=";
			var line1ToInsert = string.Format("{0}{1}", pattern1ToFind, expectedPath);

			new FileManipulation().ReplaceConfigLine(paths.Neo4jServerConfigPath, 
																							 Replacement.Create(pattern1ToFind, line1ToInsert));

			string line;
			bool changePerformed = false;
			Debug.WriteLine("Dumping the file and verifying:");
			using (var file = new StreamReader(neo4JServerConfigPath))
				while (!file.EndOfStream)
				{
					line = file.ReadLine();
					if (line == null) continue;
					Debug.WriteLine(line);

					if (!line.StartsWith(pattern1ToFind)) continue;

					if (line != line1ToInsert)
						Assert.Fail("Db was not found.");
					else
						changePerformed = true;
				}
			Debug.WriteLine("Done dumping the file.");

			Assert.IsTrue(changePerformed, "Db path was not changed.");
		}

		[TestMethod]
		public void SetJavaPath()
		{
			var expectedPath = "c:/foo/java";

			var neo4JwrapperConfigPath = Path.Combine(Environment.CurrentDirectory, "neo4j-wrapper.conf");
			IPaths paths = new PathsMock(null, neo4JwrapperConfigPath);

			var pattern1ToFind = "wrapper.java.command=";
			var line1ToInsert = string.Format("{0}{1}", pattern1ToFind, expectedPath);

			new FileManipulation().ReplaceConfigLine(paths.Neo4jWrapperConfigPath, 
																							 Replacement.Create(pattern1ToFind, line1ToInsert));

			string line;
			bool changePerformed = false;
			Debug.WriteLine("Dumping the file and verifying:");
			using (var file = new StreamReader(neo4JwrapperConfigPath))
				while (!file.EndOfStream)
				{
					line = file.ReadLine();
					if (line == null) continue;
					Debug.WriteLine(line);

					if (!line.StartsWith(pattern1ToFind)) continue;

					if (line != line1ToInsert)
						Assert.Fail("Db was not found.");
					else
						changePerformed = true;
				}
			Debug.WriteLine("Done dumping the file.");

			Assert.IsTrue(changePerformed, "Java path was not changed.");
		}
	}
}
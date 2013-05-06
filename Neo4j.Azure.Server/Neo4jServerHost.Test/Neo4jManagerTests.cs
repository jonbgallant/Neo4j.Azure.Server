using System;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neo4j.Azure.Server.Utils;
using Rhino.Mocks;

namespace Neo4j.Azure.Server.Test
{
	[TestClass]
	public class Neo4jManagerTests
	{
		[TestMethod]
		public void SetPort_three_locations()
		{
			var expectedPort = 1234;
			var mockRepository = new MockRepository();

			// Arrange
			var fileManipulation = mockRepository.StrictMock<IFileManipulation>();
			var paths = mockRepository.StrictMock<IPaths>();

			var neo4JServerConfigSettings = mockRepository.StrictMock<INeo4jServerConfigSettings>();
			neo4JServerConfigSettings.Expect(n => n.Port).Return("org.neo4j.server.webserver.port");
			neo4JServerConfigSettings.Expect(n => n.WebAdminDataUri).Return("webadmindatauri");
			neo4JServerConfigSettings.Expect(n => n.WebAdminManagementUri).Return("webadminmanagementuri");

			var neo4JManager = new Neo4jManager(fileManipulation, null);

			paths.Expect(p => p.Neo4jServerConfigSettings).Return(neo4JServerConfigSettings);

			paths.Expect(p => p.Neo4jAdminDataUri).Return("Neo4jAdminDataUri");
			paths.Expect(p => p.Neo4jAdminManagementUri).Return("Neo4jAdminManagementUri");
			paths.Expect(p => p.Neo4jServerConfigPath).Return("Neo4jServerConfigPath");

			fileManipulation.Expect(fm => fm.ReplaceConfigLine(null, null)).IgnoreArguments();

			mockRepository.ReplayAll();

			// Act
			neo4JManager.SetServerPortAndUrlConfig(paths, expectedPort);

			// Assert
			mockRepository.VerifyAll();
		}

		[TestMethod]
		public void SetJavaPath()
		{
			string expectedFileName = "config/wrapper.conf";
			string expectedWrapperJavaSetting = "wrapper.java.setting";
			string expectedLocalJavaExePath = "c:/localjavapath/java.exe";
			var mockRepository = new MockRepository();

			// Arrange
			var fileManipulation = mockRepository.StrictMock<IFileManipulation>();
			var paths = mockRepository.StrictMock<IPaths>();

			var neo4JManager = new Neo4jManager(fileManipulation, null);

			paths.Expect(p => p.LocalJavaExePath).Return(expectedLocalJavaExePath);
			paths.Expect(p => p.Neo4jWrapperSettingJavaCommand).Return(expectedWrapperJavaSetting);
			paths.Expect(p => p.Neo4jWrapperConfigPath).Return(expectedFileName);
			fileManipulation.Expect(fm => fm.ReplaceConfigLine(null, null)).IgnoreArguments();

			mockRepository.ReplayAll();

			// Act
			neo4JManager.SetJavaPath(paths);

			// Assert
			mockRepository.VerifyAll();
		}

		[TestMethod]
		public void ClearLogDirectory()
		{
			// Arrange - testfolder with three files
			var testFolder = Directory.CreateDirectory(Path.Combine(Environment.CurrentDirectory, "ClearLogDirectoryTest"));
			File.CreateText(Path.Combine(testFolder.FullName, "File1.txt")).Close();
			File.CreateText(Path.Combine(testFolder.FullName, "File2.txt")).Close();
			File.CreateText(Path.Combine(testFolder.FullName, "File3.txt")).Close();
	
			var neo4JManager = new Neo4jManager();
			var paths = MockRepository.GenerateMock<IPaths>();
			paths.Expect(p => p.LocalNeo4jLogsPath).Return(testFolder.FullName).Repeat.Once();

			// Pre
			Assert.AreEqual(3, testFolder.GetFiles().Count());

			// Act
			neo4JManager.ClearLogDirectory(paths);

			// Assert
			Assert.AreEqual(0, testFolder.GetFiles().Count());
			paths.VerifyAllExpectations();

			// Cleanup
			testFolder.Delete();
		}

		[TestMethod]
		public void CopyConfigurationFilesToLogsDirectory()
		{
			// Arrange - testfolder with our two files
			var inFolder = Directory.CreateDirectory(Path.Combine(Environment.CurrentDirectory, "CopyConfigurationFilesToLogsDirectoryIn"));
			var outFolder = Directory.CreateDirectory(Path.Combine(Environment.CurrentDirectory, "CopyConfigurationFilesToLogsDirectoryOut"));
			var fileName1 = "File1.txt";
			File.Create(Path.Combine(inFolder.FullName, fileName1)).Close();
			var fileName2 = "File2.txt";
			File.Create(Path.Combine(inFolder.FullName, fileName2)).Close();
			var file1 = inFolder.GetFiles().First();
			var file2 = inFolder.GetFiles().Last();

			var neo4JManager = new Neo4jManager();
			var paths = MockRepository.GenerateMock<IPaths>();
			paths.Expect(p => p.LocalNeo4jLogsPath).Return(outFolder.FullName).Repeat.Once();
			paths.Expect(p => p.Neo4jServerConfigPath).Return(file1.FullName).Repeat.Once();
			paths.Expect(p => p.Neo4jWrapperConfigPath).Return(file2.FullName).Repeat.Once();

			// Pre
			Assert.AreEqual(2, inFolder.GetFiles().Count());
			Assert.AreEqual(0, outFolder.GetFiles().Count());

			// Act
			neo4JManager.CopyConfigurationFilesToLogsDirectory(paths);

			// Assert
			Assert.AreEqual(2, inFolder.GetFiles().Count());
			Assert.AreEqual(2, outFolder.GetFiles().Count());
			Assert.AreEqual(fileName1, outFolder.GetFiles().First().Name);
			Assert.AreEqual(fileName2, outFolder.GetFiles().Last().Name);
			paths.VerifyAllExpectations();

			// Cleanup
			inFolder.Delete(true);
			outFolder.Delete(true);
		}
	}
}
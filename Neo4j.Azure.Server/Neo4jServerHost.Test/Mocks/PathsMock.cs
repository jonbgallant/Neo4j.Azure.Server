using System;
using Neo4j.Azure.Server.Utils;

namespace Neo4j.Azure.Server.Test.Mocks
{
	public class PathsMock : IPaths
	{
		public PathsMock(string neo4jServerConfigPath, string neo4jwrapperConfigPath)
		{
			Neo4jServerConfigPath = neo4jServerConfigPath;
			Neo4jWrapperConfigPath = neo4jwrapperConfigPath;
		}

		public string Neo4jServerConfigPath { get; private set; }

		public string Neo4jWrapperConfigPath { get; private set; }

		public string Neo4jAdminDataUri
		{
			get { throw new NotImplementedException(); }
			set { throw new NotImplementedException(); }
		}

		public string Neo4jAdminManagementUri
		{
			get { throw new NotImplementedException(); }
		}

		public int Neo4jPort
		{
			get { throw new NotImplementedException(); }
		}

		public INeo4jServerConfigSettings Neo4jServerConfigSettings
		{
			get { throw new NotImplementedException(); }
			set { throw new NotImplementedException(); }
		}

		public string Neo4jWrapperSettingJavaCommand
		{
			get { throw new NotImplementedException(); }
		}

		public string Neo4jDBDriveBlobRelativePath
		{
			get { throw new NotImplementedException(); }
		}

		public string Neo4jDababasePath
		{
			get { throw new NotImplementedException(); }
		}

		public string LocalRootPath
		{
			get { throw new NotImplementedException(); }
		}

		public string LocalNeo4jZip
		{
			get { throw new NotImplementedException(); }
		}

		public string Neo4jExePath
		{
			get { throw new NotImplementedException(); }
		}

		public string LocalNeo4jLogsPath
		{
			get { throw new NotImplementedException(); }
		}

		public string LocalNeo4jDababaseResourceName
		{
			get { throw new NotImplementedException(); }
		}

		public string Neo4jLogsContainerName
		{
			get { throw new NotImplementedException(); }
		}

		public string LocalNeo4jPath
		{
			get { throw new NotImplementedException(); }
		}

		public string LocalJavaPath
		{
			get { throw new NotImplementedException(); }
		}

		public string LocalJavaZip
		{
			get { throw new NotImplementedException(); }
		}

		public string LocalJavaExePath
		{
			get { throw new NotImplementedException(); }
		}


        public bool AlwaysDownloadFile
        {
            get { throw new NotImplementedException(); }
        }

        public int Neo4jDBDriveSize
        {
            get { throw new NotImplementedException(); }
        }

        public string MountDrivePath
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }


        public string LocalJavaHomePath
        {
            get { throw new NotImplementedException(); }
        }
    }
}
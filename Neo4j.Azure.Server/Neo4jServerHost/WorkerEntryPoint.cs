using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using Diversify.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
using Neo4j.Azure.Server.Utils;
using Microsoft.WindowsAzure.Storage;

namespace Neo4j.Azure.Server
{
	public class WorkerEntryPoint : RoleEntryPoint
	{
		Neo4jManager neo4JManager;
		IPaths paths;

		public override bool OnStart()
		{
			var fileManipulation = new FileManipulation();
			var zipping = new Zipping();
			var localResourceManager = new LocalResourceManager();
			var cloudDriveManager = new CloudDriveManager(localResourceManager);
			paths = new Paths(localResourceManager);
			//paths = new TestPaths(localResourceManager);

			neo4JManager = new Neo4jManager(fileManipulation, zipping);

			ServicePointManager.DefaultConnectionLimit = 12;

			CrashDumps.EnableCollection(true);

			var initialConfiguration = DiagnosticMonitor.GetDefaultInitialConfiguration();
			var directoryConfiguration = neo4JManager.GetLogDirectory(paths);
			initialConfiguration.Directories.DataSources.Add(directoryConfiguration);

			DiagnosticMonitor.Start(ConfigSettings.DiagnosticsConnectionString, initialConfiguration);

			RoleEnvironment.Changing += RoleEnvironmentChanging;
			
            // this is no longer supported in 2.0
            //CloudStorageAccount.SetConfigurationSettingPublisher(
            //        (configName, configSetter) => 
            //            configSetter(RoleEnvironment.GetConfigurationSettingValue(configName))
            //    );

			var storageAccount = CloudStorageAccount.Parse(
                ConfigSettings.StorageConnectionString);

			var neo4jInternalPort = paths.Neo4jPort;
			neo4JManager.Install(paths, storageAccount, neo4jInternalPort, cloudDriveManager);

			return base.OnStart();
		}

		public override void Run()
		{
			neo4JManager.Start(paths);

			while (true)
			{
				Thread.Sleep(TimeSpan.FromMinutes(1));
				Trace.TraceInformation("Neo4j service seems to be running fine.");
			}
		}

		private static void RoleEnvironmentChanging(object sender, RoleEnvironmentChangingEventArgs e)
		{
			if (e.Changes.Any(change => change is RoleEnvironmentConfigurationSettingChange))
			{
				e.Cancel = true;
			}
		}
	}
}
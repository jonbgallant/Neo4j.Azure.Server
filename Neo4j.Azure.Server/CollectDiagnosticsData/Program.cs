using System;
using System.Configuration;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.Diagnostics.Management;
using Microsoft.WindowsAzure.Storage;

namespace CollectDiagnosticsData
{
	static class Program
	{
		static void Main()
		{
            /*
			var accountName = ConfigurationManager.AppSettings["Cloud Diagnostics Storage Account Name"];
			Console.WriteLine(string.Format("Cloud Diagnostics Storage Account Name: {0}", accountName));
			var accountKey = ConfigurationManager.AppSettings["Cloud Diagnostics Storage Account Key"];
			Console.WriteLine(string.Format("Cloud Diagnostics Storage Account Key: {0}", new string('*', accountKey.Length)));

			var storageCredentialsAccountAndKey = new StorageCredentialsAccountAndKey(accountName, accountKey);
			var storageAccount = new CloudStorageAccount(storageCredentialsAccountAndKey, true);
            */

            var connectionString = ConfigurationManager.AppSettings["CloudStorateAccountConnectionString"];

            var storageAccount = CloudStorageAccount.Parse(connectionString);

			Console.WriteLine(string.Format("Deployment ID: "));
			var deploymentID = Console.ReadLine();

            var deploymentDiagnosticManager = new DeploymentDiagnosticManager(connectionString, deploymentID);

			string roleInstanceName;
			Guid guid;

			var roleNames = deploymentDiagnosticManager.GetRoleNames();
			foreach (var roleName in roleNames)
			{
				Console.WriteLine(string.Format("Role Name: {0}", roleName));

				foreach (var roleInstanceDiagnosticManager in deploymentDiagnosticManager.GetRoleInstanceDiagnosticManagersForRole(roleName))
				{
					roleInstanceName = roleInstanceDiagnosticManager.RoleInstanceId;
					Console.WriteLine(string.Format("Role Instance Name: {0}", roleInstanceName));

					Console.WriteLine("Calling to transfer logs.");
					guid = Transfer(roleInstanceDiagnosticManager, DataBufferName.Logs);
					Console.WriteLine(string.Format("Logs transfer '{0}'", guid));
					Console.WriteLine("Calling to transfer directories.");
					guid = Transfer(roleInstanceDiagnosticManager, DataBufferName.Directories);
					Console.WriteLine(string.Format("Directories transfer '{0}'", guid));
				}
			}

			Console.WriteLine("Hit any key to end...");
			Console.ReadKey();
		}

		private static Guid Transfer(RoleInstanceDiagnosticManager ridm, DataBufferName dataBufferName)
		{
			ridm.CancelOnDemandTransfers(dataBufferName);

			var transferOptions = new OnDemandTransferOptions
															{
																From = DateTime.MinValue,
																To = DateTime.UtcNow,
																LogLevelFilter = LogLevel.Undefined
															};

			return ridm.BeginOnDemandTransfer(dataBufferName, transferOptions);
		}
	}
}
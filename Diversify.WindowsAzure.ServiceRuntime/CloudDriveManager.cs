using System;
using System.Linq;
using Diversify.WindowsAzure.ServiceRuntime.Utils;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;

namespace Diversify.WindowsAzure.ServiceRuntime
{
	public class CloudDriveManager : ICloudDriveManager
	{
		private readonly ILocalResourceManager _localResourceManager;

		public CloudDriveManager(ILocalResourceManager localResourceManager)
		{
            _localResourceManager = localResourceManager;
		}

		public Uri GetBlobUri(CloudStorageAccount account, string blobRelativePath)
		{
			var blobsStoragePath = account.BlobEndpoint.AbsoluteUri;
			var blobPath = blobsStoragePath.CombineUris(blobRelativePath);
			var blobUri = new Uri(blobPath);
			return blobUri;
		}

		public string MountDrive(string storageConnectionString, string blobRelativePath, string localResourceName, int sizeInMb)
		{
            var account = CloudStorageAccount.Parse(storageConnectionString.Replace("DefaultEndpointsProtocol=https", "DefaultEndpointsProtocol=http"));            
			var blobUri = GetBlobUri(account, blobRelativePath);

            var localCache = _localResourceManager.GetByConfigName(localResourceName);
			CloudDrive.InitializeCache(localCache.RootPath, localCache.MaximumSizeInMegabytes);

			var drive = new CloudDrive(blobUri, account.Credentials);

            drive.CreateIfNotExist(sizeInMb);
			
            var driveRootPath = drive.Mount(localCache.MaximumSizeInMegabytes, DriveMountOptions.Force);
			
			return driveRootPath;
		}

		public void UnmountAll(string storageConnectionString)
		{
			var account = CloudStorageAccount.Parse(storageConnectionString);
			var credentials = account.Credentials;

			foreach (var drive in CloudDrive.GetMountedDrives()
                .Select(mountedDrive => new CloudDrive(mountedDrive.Value, credentials)))
			{
				drive.Unmount();
			}
		}
	}
}
namespace Diversify.WindowsAzure.ServiceRuntime
{
	public interface ICloudDriveManager
	{
		string MountDrive(string storageConnectionString, string blobRelativePath, string localResourceName, int sizeInMb);
		void UnmountAll(string storageConnectionString);
	}
}
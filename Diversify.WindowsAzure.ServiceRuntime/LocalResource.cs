using MSLocalResource = Microsoft.WindowsAzure.ServiceRuntime.LocalResource;

namespace Diversify.WindowsAzure.ServiceRuntime
{
	internal class LocalResource : ILocalResource
	{
        private readonly MSLocalResource _localResourceManager;

		public LocalResource(MSLocalResource localResource)
		{
            _localResourceManager = localResource;
		}

		public int MaximumSizeInMegabytes
		{
            get { return _localResourceManager.MaximumSizeInMegabytes; }
		}

		public string Name
		{
            get { return _localResourceManager.Name; }
		}

		public string RootPath
		{
            get { return _localResourceManager.RootPath; }
		}
	}
}
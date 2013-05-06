using System.Collections.Generic;
using Microsoft.WindowsAzure.ServiceRuntime;

namespace Diversify.WindowsAzure.ServiceRuntime
{
	public class LocalResourceManager : ILocalResourceManager
	{
        private readonly IDictionary<string, ILocalResource> _localResourceManager;

		public LocalResourceManager()
		{
            _localResourceManager = new Dictionary<string, ILocalResource>();
		}

		public ILocalResource GetByConfigName(string localResourceName)
		{
            if (_localResourceManager.ContainsKey(localResourceName)) return _localResourceManager[localResourceName];

            lock (_localResourceManager)
			{
				var msLocalResource = RoleEnvironment.GetLocalResource(localResourceName);

				ILocalResource localResource = new LocalResource(msLocalResource);
                _localResourceManager.Add(localResourceName, localResource);

				return localResource;
			}
		}
	}
}
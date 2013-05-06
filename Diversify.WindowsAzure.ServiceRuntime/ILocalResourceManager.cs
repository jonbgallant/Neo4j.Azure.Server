namespace Diversify.WindowsAzure.ServiceRuntime
{
	public interface ILocalResourceManager
	{
		ILocalResource GetByConfigName(string localResourceName);
	}
}
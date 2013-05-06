namespace Diversify.WindowsAzure.ServiceRuntime
{
	public interface ILocalResource
	{
		int MaximumSizeInMegabytes { get; }
		string Name { get; }
		string RootPath { get; }
	}
}
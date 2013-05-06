using System;

namespace Diversify.WindowsAzure.ServiceRuntime.Utils
{
	public static class StringExtensions
	{
		public static string CombineUris(this string uri, string relativePathToAppend)
		{
			var builder = new UriBuilder(uri);
			builder.PathAppend(relativePathToAppend);
			return builder.Uri.AbsoluteUri;
		}
	}
}
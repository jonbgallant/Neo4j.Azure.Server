using System;

namespace Diversify.WindowsAzure.ServiceRuntime.Utils
{
	public static class UriBuilderExtensions
	{
		/// <summary>
		/// The <see cref="UriBuilder.Path"/> gets appended by a second piece of relative path.
		/// </summary>
		/// <param name="uriBuilder">the <see cref="UriBuilder"/> to append with the <paramref name="pathToAppend"/>.</param>
		/// <param name="pathToAppend">the relative path to append to the <paramref name="uriBuilder"/>.</param>
		/// <remarks>There will be exactly one slash "/" in the middle of the original path (<see cref="UriBuilder.Path"/>) and new path (<paramref name="pathToAppend"/>) pieces regardless if none, one or both of them had a slash to begin with.</remarks>
		public static void PathAppend(this UriBuilder uriBuilder, string pathToAppend)
		{
			uriBuilder.Path = string.Concat(uriBuilder.Path,
											uriBuilder.Path == "/" || uriBuilder.Path.EndsWith("/") ? string.Empty : "/" /* This will avoid the dreaded doubleslash. */, 
											pathToAppend != null ? pathToAppend.TrimStart('/') : string.Empty /* The pathToAppend is valid wether or not it has a leading slash. */);
		}
	}
}
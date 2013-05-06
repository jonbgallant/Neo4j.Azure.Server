using System;
using Diversify.WindowsAzure.ServiceRuntime.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Diversify.WindowsAzure.ServiceRuntime.Tests.Utils
{
	[TestClass]
	public class UriBuilderExtensionsTests
	{
		[TestMethod]
		public void CreateUri()
		{
			var builder = new UriBuilder("https", "foo.com", 1234, "/somepath/somefile.htm");
			Assert.AreEqual("https://foo.com:1234/somepath/somefile.htm", builder.Uri.AbsoluteUri);
		}

		[TestMethod]
		public void CreateUri2()
		{
			var builder = new UriBuilder("https", "foo.com")
			{
				Port = 4321,
				Path = "/somepath/somefile.htm"
			};

			Assert.AreEqual("https://foo.com:4321/somepath/somefile.htm", builder.Uri.AbsoluteUri);
		}

		[TestMethod]
		public void CreateUri3_default_http()
		{
			var builder = new UriBuilder("foo.com")
			{
				Port = 4321,
				Path = "/somepath/somefile.htm"
			};

			Assert.AreEqual("http://foo.com:4321/somepath/somefile.htm", builder.Uri.AbsoluteUri);
		}

		[TestMethod]
		public void CreateUri3_set_https()
		{
			var builder = new UriBuilder("foo.com")
			{
				Scheme = "https",
				Port = 4321,
				Path = "/somepath/somefile.htm"
			};

			Assert.AreEqual("https://foo.com:4321/somepath/somefile.htm", builder.Uri.AbsoluteUri);
		}

		[TestMethod]
		public void CreateUri3_append_relative_path()
		{
			var builder = new UriBuilder("foo.com")
			{
				Scheme = "https",
				Port = 4321,
				Path = "/a"
			};

			Assert.AreEqual("/a", builder.Path);
			builder.PathAppend("/somepath/somefile.htm");

			Assert.AreEqual("https://foo.com:4321/a/somepath/somefile.htm", builder.Uri.AbsoluteUri);
		}

		[TestMethod]
		public void CreateUri3_append_relative_path_2()
		{
			var builder = new UriBuilder("foo.com")
			{
				Scheme = "https",
				Port = 4321,
				Path = "/a"
			};

			Assert.AreEqual("/a", builder.Path);
			builder.PathAppend("/////somepath/somefile.htm");

			Assert.AreEqual("https://foo.com:4321/a/somepath/somefile.htm", builder.Uri.AbsoluteUri);
		}
		[TestMethod]
		public void CreateUri3_append_relative_path_3()
		{
			var builder = new UriBuilder("foo.com")
			{
				Scheme = "https",
				Port = 4321
			};

			Assert.AreEqual("/", builder.Path);

			builder.PathAppend("/somepath/somefile.htm");

			Assert.AreEqual("https://foo.com:4321/somepath/somefile.htm", builder.Uri.AbsoluteUri);
		}

		[TestMethod]
		public void Relative_path_default_is_slash()
		{
			var builder = new UriBuilder("foo.com");

			Assert.AreEqual("/", builder.Path);
		}

		[TestMethod]
		public void Clear_Relative_path_does_nothing()
		{
			var builder = new UriBuilder("foo.com")
			              	{
			              		Path = string.Empty
			              	};

			Assert.AreEqual("/", builder.Path);
		}

		[TestMethod]
		public void Null_Relative_path_does_nothing()
		{
			var builder = new UriBuilder("foo.com")
			              	{
			              		Path = null
			              	};

			Assert.AreEqual("/", builder.Path);
		}

		[TestMethod]
		public void Add_slash_only_adds_slash()
		{
			var builder = new UriBuilder("foo.com")
			              	{
			              		Path = "a"
			              	};

			builder.Path += "/";

			Assert.AreEqual("http://foo.com/a/", builder.Uri.AbsoluteUri);
		}

		[TestMethod]
		public void Add_slash_only_adds_slash_and_append_works()
		{
			var builder = new UriBuilder("foo.com")
			              	{
			              		Path = "a"
			              	};

			builder.Path += "/";

			builder.PathAppend("b/");

			Assert.AreEqual("http://foo.com/a/b/", builder.Uri.AbsoluteUri);
		}
	}
}
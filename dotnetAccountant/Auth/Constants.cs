using System;

namespace dotnetAccountant
{
	public static class ContentTypes
	{
		public static class Application
		{
			private const string application = "application";
			public const string json = application + "/json";
			public const string javascript = application + "/javascript";
			public const string xml = application + "/xml";
		}
		public static class Text
		{
			private const string text = "text";
			public const string plain = text + "/plain";
			public const string xml = text + "/xml";
			public const string html = text + "/html";
		}
	}

	public static class Urls{
		public const string BaseUrl = "https://graph.microsoft.com/v1.0/";
	}

	public static class Permissions
	{
		public static class User
		{
			public const string Read = "User.Read";
			public const string ReadAll = "User.Read.All";
			public const string ReadWrite = "User.ReadWrite";
			public const string ReadWriteAll = "User.ReadWrite.All";
		}

		public static class Files
		{
			public const string Read = "Files.Read";
			public const string ReadAll = "Files.Read.All";
			public const string ReadWrite = "Files.ReadWrite";
			public const string ReadWriteAll = "Files.ReadWrite.All";
		}
	}

	public static class Timeouts{
		public static readonly TimeSpan Silent = TimeSpan.FromSeconds(5);
		public static readonly TimeSpan Interactive = TimeSpan.FromMinutes(1);
	}

	public static class Selects{
		public const string id = "id";
		public const string name = "name";
		public const string size = "size";
		public const string webUrl = "webUrl";
		/// <summary>
		/// Not working
		/// </summary>
		public const string content = "content"; 
		public const string createdDateTime = "createdDateTime";
		/// <summary>
		/// Not working
		/// </summary>
		public const string downloadUrl = "@microsoft.graph.downloadUrl";
	}
}
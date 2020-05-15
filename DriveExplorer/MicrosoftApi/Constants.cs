using System;

namespace DriveExplorer.MicrosoftApi
{
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
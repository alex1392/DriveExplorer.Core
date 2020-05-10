using Xunit;
using System;
using dotnetAccountant;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using Microsoft.Graph;
using System.Linq;
using System.Net;
using System.IO;
using System.Text;

namespace dotnetAccountant.Tests
{
	public class GraphManagerTests
	{
		private IConfigurationRoot appConfig;
		private AuthProvider authProvider;
		private GraphManager graphManager;

		public GraphManagerTests()
		{
			appConfig = Program.GetAppConfig();
			AuthProvider.Initialize(appConfig);
			GraphManager.Initialize(AuthProvider.Instance);
			authProvider = AuthProvider.Instance;
			graphManager = GraphManager.Instance;
		}

		[Fact]
		public void GetGraphManager_EqualToOriginal()
		{
			//Given
			//When
			var graphManager1 = GraphManager.Instance;
			//Then
			Assert.Equal(graphManager, graphManager1);
		}

		[Fact]
		public void GetUser_UserMailEqualToUsernameInAppConfig()
		{
			//Given
			authProvider.Scopes = new[] { Permissions.User.Read };
			string Username = appConfig[nameof(Username)];
			//When
			var user = graphManager.GetMeAsync().Result;
			Console.WriteLine(user.DisplayName);
			//Then
			Assert.Equal(user.Mail, Username);
		}

		[Fact]
		public void SearchDrive_ResultNotNull()
		{
			//Given
			authProvider.Scopes = new[] { Permissions.Files.Read };
			//When
			var file = graphManager.SearchDriveAsync("LICENSE.txt",
				new[] {
					new QueryOption("$top", "5"),
					new QueryOption("$select", Selects.name + "," + Selects.id)
					}).Result.First();
			Console.WriteLine(file.Name);
			//Then
			Assert.NotNull(file);
		}

		[Fact]
		public void DownloadFile_ResultNotNull()
		{
			//Given
			authProvider.Scopes = new[] { Permissions.Files.Read };
			//When
			var item = graphManager.SearchDriveAsync("LICENSE.txt").Result.CurrentPage.First();
			var stream = graphManager.GetFileAsync(item.Id).Result;
			string content;
			using (var reader = new StreamReader(stream))
			{
				content = reader.ReadToEnd();
				Console.WriteLine(content);
			}
			//Then
			Assert.True(!string.IsNullOrEmpty(content));
		}

		[Fact]
		public void GetDriveRoot_ResultNotNull()
		{
			//Given
			authProvider.Scopes = new[] { Permissions.Files.Read };
			//When
			var item = graphManager.GetDriveRootAsync().Result;
			Console.WriteLine(item.Name);
			//Then
			Assert.NotNull(item);
		}

		[Fact]
		public void GetChildrenOfRoot_ResultNotNull()
		{
			//Given
			authProvider.Scopes = new[] { Permissions.Files.Read };
			//When
			var root = graphManager.GetDriveRootAsync().Result;
			var children = graphManager.GetChildrenAsync(root.Id).Result;
			foreach (var child in children)
			{
				Console.WriteLine(child.Name);
			}
			//Then
			Assert.NotNull(children);
		}

		[Fact]
		public void GetFoldersAndFilesOfRoot_ResultNotNull()
		{
			//Given
			authProvider.Scopes = new[] { Permissions.Files.Read };
			//When
			var root = graphManager.GetDriveRootAsync().Result;
			var (folders, files) = graphManager.GetFolersAndFilesAsync(root.Id).Result;
			Console.WriteLine("=====Folders=====");
			folders.ForEach(folder => Console.WriteLine(folder.Name));
			Console.WriteLine("=====Files=====");
			files.ForEach(file => Console.WriteLine(file.Name));
			//Then
			Assert.True(folders != null || files != null);
		}

		[Fact]
		public void UpdateFile_ResultNotNull()
		{
			//Given
			authProvider.Scopes = new[] { Permissions.Files.ReadWrite };
			//When
			var itemId = graphManager.SearchDriveAsync("LICENSE.txt").Result.FirstOrDefault()?.Id;
			var content = "aaa";
			var driveItem = graphManager.UpdateFileAsync(itemId, content).Result;
			Console.WriteLine(driveItem.Name);
			//Then
			Assert.NotNull(driveItem);
		}

		[Fact]
		public void UploadFile_ResultNotNull()
		{
			//Given
			authProvider.Scopes = new[] { Permissions.Files.ReadWrite };
			//When
			var content = "aaa";
			var parentId = graphManager.GetDriveRootAsync().Result.Id;
			var filename = "aaa.txt";
			var response = graphManager.UploadFileAsync(parentId, filename, content).Result;
			Console.WriteLine(response);
			//Then
			Assert.NotNull(response);
		}
	}
}
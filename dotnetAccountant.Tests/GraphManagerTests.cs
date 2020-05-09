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

		public GraphManagerTests()
		{
			appConfig = Program.GetAppConfig();
			AuthProvider.Initialize(appConfig);
			GraphManager.Initialize(AuthProvider.Instance);
			authProvider = AuthProvider.Instance;
		}

		[Fact]
		public void SingletonTest()
		{
			//Given
			var graphManager1 = GraphManager.Instance;
			//When
			var graphManager2 = GraphManager.Instance;
			//Then
			Assert.Equal(graphManager1, graphManager2);
		}

		[Fact]
		public void TimeoutTest()
		{
			//Given
			var graphManager = GraphManager.Instance;
			//When

			//Then
			Assert.ThrowsAsync(typeof(TimeoutException),
				() => graphManager.GetMeAsync(millisecondsDelay: (int)Timeouts.Silent.TotalMilliseconds));
		}

		[Fact]
		public void GetMeTest()
		{
			//Given
			// set scopes of authProvider before generating graphManager
			authProvider.Scopes = new[] { Permissions.User.Read };
			var graphManager = GraphManager.Instance;
			string Username = appConfig[nameof(Username)];
			//When
			var user = graphManager.GetMeAsync().Result;
			Console.WriteLine(user.DisplayName);
			//Then
			Assert.Equal(user.Mail, Username);
		}

		[Fact]
		public void SearchDriveTest()
		{
			//Given
			authProvider.Scopes = new[] { Permissions.Files.Read };
			var graphManager = GraphManager.Instance;
			//When
			var items = graphManager.SearchDriveAsync("LICENSE.txt",
				new[] {
					new QueryOption("$top", "5"),
					new QueryOption("$select", Selects.name + "," + Selects.id)
					}).Result.CurrentPage;
			var file = items.First();
			Console.WriteLine(file.Name);
			//Then
			Assert.NotNull(file);
		}

		[Fact]
		public void DownloadFileTest()
		{
			//Given
			authProvider.Scopes = new[] { Permissions.Files.Read };
			var graphManager = GraphManager.Instance;
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
		public void GetDriveRootTest()
		{
			//Given
			authProvider.Scopes = new[] { Permissions.Files.Read };
			var graphManager = GraphManager.Instance;
			//When
			var item = graphManager.GetDriveRootAsync().Result;
			Console.WriteLine(item.Name);
			//Then
			Assert.NotNull(item);
		}

		[Fact]
		public void UpdateFileTest()
		{
			//Given
			authProvider.Scopes = new[] { Permissions.Files.ReadWrite };
			var graphManager = GraphManager.Instance;
			//When
			var itemId = graphManager.SearchDriveAsync("LICENSE.txt").Result.FirstOrDefault()?.Id;
			var content = "aaa";
			var driveItem = graphManager.UpdateFileAsync(itemId, content).Result;
			Console.WriteLine(driveItem.Name);
			//Then
			Assert.NotNull(driveItem);
		}

		[Fact]
		public void UploadFileTest()
		{
			//Given
			authProvider.Scopes = new[] { Permissions.Files.ReadWrite };
			var graphManager = GraphManager.Instance;
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
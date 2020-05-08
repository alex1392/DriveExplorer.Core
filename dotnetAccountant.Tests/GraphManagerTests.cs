using Xunit;
using System;
using dotnetAccountant;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using Microsoft.Graph;
using System.Linq;

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
			var accessToken = authProvider.GetAccessTokenWithUsernamePassword().Result;
			var graphManager = GraphManager.Instance;
			string Username = appConfig[nameof(Username)];
			//When
			var user = graphManager.GetMeAsync().Result;
			//Then
			Assert.Equal(user.Mail, Username);
		}

		[Fact]
		public void SearchDriveTest()
		{
			//Given
			authProvider.Scopes = new[] { Permissions.Files.Read };
			var accessToken = authProvider.GetAccessTokenWithUsernamePassword().Result;
			var graphManager = GraphManager.Instance;
			//When
			var items = graphManager.SearchDriveAsync(".csv",
				new[] { Selects.name, Selects.id, Selects.downloadUrl },
				new[] { new QueryOption("$top", "5") }).Result.CurrentPage;
			var file = items.First();
			//Then
			Assert.NotNull(file);
		}

		[Fact]
		public void DownloadFileTest()
		{
			//Given
			authProvider.Scopes = new[] { Permissions.Files.Read };
			var accessToken = authProvider.GetAccessTokenWithUsernamePassword().Result;
			//When

			//Then
		}
	}
}
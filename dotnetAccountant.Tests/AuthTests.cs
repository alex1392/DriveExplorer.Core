using System;
using Xunit;
using dotnetAccountant;
using Microsoft.Graph;
using Microsoft.Identity.Client;
using Microsoft.Extensions.Configuration;
using Moq;

namespace dotnetAccountant.Tests
{
	public class AuthTests
	{

		public DeviceCodeAuthProvider GetAuthProvider()
		{
			// build app configuration
			var appConfig = Program.LoadAppConfig();
			string appId = appConfig[nameof(appId)];
			string[] scopes = appConfig[nameof(scopes)].Split(';');
			return new DeviceCodeAuthProvider(appId, scopes);
		}

		[Fact]
		public void GetAccessTokenTest()
		{
			// Arrange
			var authProvider = GetAuthProvider();
			// Act
			var token = authProvider.GetAccessToken().Result;
			// Assert
			Assert.NotNull(token);
		}
	}
}

using System;
using Xunit;
using dotnetAccountant;
using Microsoft.Graph;
using Microsoft.Identity.Client;
using Microsoft.Extensions.Configuration;
using Moq;

namespace dotnetAccountant.Tests
{
	public class AuthProviderTests
	{
		private IConfigurationRoot appConfig;

		public AuthProviderTests(){
			appConfig = Program.GetAppConfig();
			AuthProvider.Initialize(appConfig);
		}

		[Fact]
		public void SingletonTest()
		{
			//Given
			var authProvider1 = AuthProvider.Instance;
			//When
			var authProvider2 = AuthProvider.Instance;
			//Then
			Assert.Equal(authProvider1, authProvider2);

		}
		
		[Fact]
		public void GetAccessTokenWithUsernamePasswordTest()
		{
			// Arrange
			var authProvider = AuthProvider.Instance;
			// Act
			var token = authProvider.GetAccessTokenWithUsernamePassword().Result;
			// Assert
			Assert.NotNull(token);
		}
	}
}

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
		private AuthProvider authProvider;

		public AuthProviderTests(){
			appConfig = Program.GetAppConfig();
			AuthProvider.Initialize(appConfig);
			authProvider = AuthProvider.Instance;
		}

		[Fact]
		public void GetAuthProvider_EqualToOriginal()
		{
			//Given
			//When
			var authProvider1 = AuthProvider.Instance;
			//Then
			Assert.Equal(authProvider1, authProvider);

		}
		
		[Fact]
		public void GetAccessTokenWithUsernamePassword_ResultNotNull()
		{
			//Given
			//When
			var token = authProvider.GetAccessTokenWithUsernamePassword().Result;
			//Then
			Assert.NotNull(token);
		}
	}
}

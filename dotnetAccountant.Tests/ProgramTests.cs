using Xunit;
using System;
using Microsoft.Extensions.Configuration;
using dotnetAccountant;

namespace dotnetAccountant.Tests
{
	public class ProgramTests
	{
		[Fact]
		public void LoadAppConfigTest(){
			var appConfig = Program.LoadAppConfig();
			Assert.True(appConfig != null, "Null appConfig");
		}

		[Fact]
		public void GetAppSettingsTest()
		{
			var appConfig = Program.LoadAppConfig();
			var (appId, scopes) = Program.GetAppSettings(appConfig);
			Assert.True(!string.IsNullOrEmpty(appId), $"Empty {nameof(appId)}");
			Assert.True(Guid.TryParse(appId, out _), $"{nameof(appId)} should be a {typeof(Guid)}");
		}
	}
}
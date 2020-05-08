
using Xunit;

namespace dotnetAccountant.Tests
{
	public class ProgramTests
	{
		[Fact]
		public void GetAppConfigTest()
		{
			var appConfig = Program.GetAppConfig();
			Assert.NotNull(appConfig);
		}
	}
}

using Xunit;

namespace dotnetAccountant.Tests
{
	public class ProgramTests
	{
		[Fact]
		public void GetAppConfig_ResultNotNull()
		{
			//Given
			//When
			var appConfig = Program.GetAppConfig();
			//Then
			Assert.NotNull(appConfig);
		}
	}
}
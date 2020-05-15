using Xunit;

namespace DriveExplorer
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
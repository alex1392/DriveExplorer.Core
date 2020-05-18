using System;
using System.IO;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Drive.v3;
using Xunit;
using System.Threading;

namespace DriveExplorer.GoogleApi
{
	public class GoogleApiTests
	{

		public GoogleApiTests()
		{

		}
		

		[Fact]
		public async Task OAuthTestAsync()
		{
			//Given
			UserCredential credential;
			using (var stream = new FileStream("GoogleApi/client_secret.json", FileMode.Open, FileAccess.Read))
			{
				credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
					GoogleClientSecrets.Load(stream).Secrets,
					new [] { DriveService.Scope.Drive },
					"user",
					CancellationToken.None);
			}
			var service = new DriveService(new BaseClientService.Initializer
			{
				HttpClientInitializer = credential,
				ApplicationName = "googleApi",
			});
			//When
			var drives = (await service.Drives.List().ExecuteAsync()).Drives;
			var files = (await service.Files.List().ExecuteAsync()).Files;
			Console.WriteLine("=====Drives=====");
			foreach (var drive in drives)
			{
				Console.WriteLine(drive.Id + ":" + drive.Name);
			}
			Console.WriteLine("=====Files=====");
			foreach (var file in files)
			{
				Console.WriteLine(file.Id + ":" + file.Name);
			}

			//Then
			Assert.NotNull(files);
		}
	}
}
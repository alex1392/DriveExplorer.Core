using System.Net.Http;
using Microsoft.Graph;
using Microsoft.Graph.Extensions;
using System;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace dotnetAccountant
{
	public class GraphManager
	{
		private static GraphServiceClient client;
		private static GraphManager instance;
		public static GraphManager Instance => instance;

		// lazy singleton
		public static void Initialize(IAuthenticationProvider authProvider)
		{
			if (instance == null)
			{
				instance = new GraphManager(authProvider);
			}
			else
			{
				Console.WriteLine($"{typeof(GraphManager)} has already been initialized.");
			}
		}

		private GraphManager(IAuthenticationProvider authProvider)
		{
			client = new GraphServiceClient(authProvider);
		}

		///
		/// <summary>
		/// Requires scopes: <see cref="Permissions.User.Read"/>
		/// </summary>
		/// <param name="millisecondsDelay">Delay this task for testing.</param>
		/// <returns></returns>
		public async Task<User> GetMeAsync(int millisecondsDelay = 0)
		{
			using (var cts = new CancellationTokenSource(Timeouts.Silent))
			{
				await Task.Delay(millisecondsDelay);
				var result = await client.Me.Request().GetAsync(cts.Token);
				return result;
			}
		}

		/// <summary>
		/// Requires scopes: <see cref="Permissions.Files.Read"/>
		/// </summary>
		/// <param name="query"><see cref="System.String"/> of search query</param>
		/// <param name="options"><see cref="IEnumerable{T}"/> of strings to select</param>
		/// <returns></returns>
		public async Task<IDriveItemSearchCollectionPage> SearchDriveAsync(string query, IEnumerable<QueryOption> options = null)
		{
			using (var cts = new CancellationTokenSource(Timeouts.Silent))
			{
				return await client.Me.Drive.Root.Search(query).Request(options).GetAsync(cts.Token);
			}
		}

		public async Task<Stream> GetFileAsync(string id)
		{
			using (var cts = new CancellationTokenSource(Timeouts.Silent))
			{
				return await client.Me.Drive.Items[id].Content.Request().GetAsync(cts.Token);
			}

		}

		public async Task<string> UploadFileAsync(string parentId, string filename, string content)
		{
			var urlString = Urls.BaseUrl + $"/me/drive/items/{parentId}:/{filename}:/content";
			var uri = new Uri(urlString);
			using (var cts = new CancellationTokenSource(Timeouts.Silent))
			using (var request = new HttpRequestMessage(HttpMethod.Put, uri))
			using (var stringContent = new StringContent(content, Encoding.UTF8))
			{
				request.Content = stringContent;
				await AuthProvider.Instance.AuthenticateRequestAsync(request);
				using (var response = await client.HttpProvider.SendAsync(request,HttpCompletionOption.ResponseContentRead, cts.Token))
				{
					return await response.Content.ReadAsStringAsync();
				}
			}

		}

		public async Task<DriveItem> GetDriveRootAsync()
		{
			using (var cts = new CancellationTokenSource(Timeouts.Silent))
			{
				return await client.Me.Drive.Root.Request().GetAsync(cts.Token);
			}
		}

		public async Task<DriveItem> UpdateFileAsync(string itemId, string content)
		{
			using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(content)))
			using (var cts = new CancellationTokenSource(Timeouts.Silent))
			{
				return await client.Me.Drive.Items[itemId].Content.Request().PutAsync<DriveItem>(stream, cts.Token);
			}
		}
	}
}
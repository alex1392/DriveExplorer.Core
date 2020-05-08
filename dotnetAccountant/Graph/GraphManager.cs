using System.Net.Http;
using Microsoft.Graph;
using Microsoft.Graph.Extensions;
using System;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;

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
			else {
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
		public async Task<User> GetMeAsync()
		{
			using (var cts = new CancellationTokenSource(Timeouts.Silent))
			{
				try
				{
					return await client.Me.Request().GetAsync(cts.Token);
				}
				catch (Exception ex)
				{
					Console.WriteLine($"Error getting user: {ex.Message}");
					throw ex;
				}
			}
		}

		/// 
			/// <summary>
			/// Requires scopes: <see cref="Permissions.Files.Read"/>
			/// </summary>
			/// <param name="query"><see cref="string"> of search query</param>
			/// <param name="selects"><see cref="IEnumerable"> of strings to select</param>
			/// <returns></returns>
		public async Task<IDriveItemSearchCollectionPage> SearchDriveAsync(string query, IEnumerable<string> selects = null, IEnumerable<QueryOption> options = null)
		{
			using (var cts = new CancellationTokenSource(Timeouts.Silent))
			{
				try
				{
					return await client.Me.Drive.Root.Search(query).Request(options).Select(string.Join(',',selects)).GetAsync(cts.Token);
				}
				catch (Exception ex)
				{
					Console.WriteLine($"Error getting user: {ex.Message}");
					throw ex;
				}
			}
			
		}
	}
}
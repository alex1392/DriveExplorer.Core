using Microsoft.Identity.Client;
using Microsoft.Graph;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Net.Http;
using System;
using System.Diagnostics;
using System.Threading;
using System.Linq;
using System.Security;

namespace dotnetAccountant
{
	public class DeviceCodeAuthProvider : IAuthenticationProvider
	{
		private IPublicClientApplication _msalClient;
		private string[] _scopes;
		private IAccount _userAccount;

		public DeviceCodeAuthProvider(string appId, string[] scope)
		{
			_scopes = scope;
			_msalClient = PublicClientApplicationBuilder
				.Create(appId)
				.WithAuthority(AadAuthorityAudience.AzureAdAndPersonalMicrosoftAccount, true)
				// for interactively sign in
				.WithRedirectUri("http://localhost") 
				// for username & password sign in
				.WithAuthority("https://login.microsoftonline.com/organizations")
				.Build();
			_userAccount = _msalClient.GetAccountsAsync().Result.FirstOrDefault();
		}

		// Implement IAuthenticationProvider 
		public async Task AuthenticateRequestAsync(HttpRequestMessage request)
		{
			request.Headers.Authorization = new AuthenticationHeaderValue("bearer", await GetAccessToken());
		}

		public async Task<string> GetAccessToken()
		{
			AuthenticationResult result;
			// check if user already signed in
			if (_userAccount != null)
			{
				result = await GetAccessTokenSilently();
				if (result != null)
				{
					_userAccount = result.Account;
					return result.AccessToken;
				}
			}

			result = await GetAccessTokenWithUsernamePassword();
			if (result != null)
			{
				_userAccount = result.Account;
				return result.AccessToken;
			}
			
			result = await GetAccessTokenInteractively();
			if (result != null)
			{
				_userAccount = result.Account;
				return result.AccessToken;
			}

			result = await GetAccessTokenWithDeviceCode();
			if (result != null)
			{
				_userAccount = result.Account;
				return result.AccessToken;
			}

			return null;
		}


		private async Task<AuthenticationResult> GetAccessTokenSilently()
		{
			// If there is an account, call AcquireTokenSilent
			// By doing this, MSAL will refresh the token automatically if
			// it is expired. Otherwise it returns the cached token.
			using (var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5)))
			{
				try
				{
					var result = await _msalClient
						.AcquireTokenSilent(_scopes, _userAccount)
						.ExecuteAsync(cts.Token);
					return result;
				}
				catch (Exception ex)
				{
					Console.WriteLine($"Cannot get access token silently: {ex.Message}");
					return null;
				}
			}
		}

		// Not applicable for personal account
		private async Task<AuthenticationResult> GetAccessTokenWithUsernamePassword()
		{
			using (var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5)))
			{
				try
				{
					var appConfig = Program.LoadAppConfig();
					string Username = appConfig[nameof(Username)];
					string Password = appConfig[nameof(Password)];
					var secureString = new SecureString();
					foreach (var c in Password)
					{
						secureString.AppendChar(c);
					}
					var result = await _msalClient
						.AcquireTokenByUsernamePassword(_scopes, Username, secureString)
						.ExecuteAsync(cts.Token);
					return result;
				}
				catch (Exception ex)
				{
					Console.WriteLine($"Cannot get access token with username and password: {ex.Message}");
					return null;
				}
			}
		}

		private async Task<AuthenticationResult> GetAccessTokenWithDeviceCode()
		{
			using (var cts = new CancellationTokenSource(TimeSpan.FromMinutes(1)))
			{
				try
				{
					// Invoke device code flow so user can sign-in with a browser
					var result = await _msalClient.AcquireTokenWithDeviceCode(_scopes, deviceCodeCallback =>
					{
						// display instructions to let user follow the device code flow
						Console.WriteLine(deviceCodeCallback.Message);
						// display instructions in testing output as well
						Trace.WriteLine(deviceCodeCallback.Message);
						return Task.FromResult(0);
					}).ExecuteAsync(cts.Token);

					return result;
				}
				catch (TimeoutException)
				{
					Console.WriteLine("Sign-In operation timeout");
					return null;
				}
				catch (Exception ex)
				{
					Console.WriteLine($"Error getting access token: {ex.Message}");
					return null;
				}
			}
		}

		private async Task<AuthenticationResult> GetAccessTokenInteractively()
		{
			using (var cts = new CancellationTokenSource(TimeSpan.FromMinutes(1)))
			{
				try
				{
					var result = await _msalClient.AcquireTokenInteractive(_scopes).ExecuteAsync(cts.Token);
					return result;
				}
				catch (TimeoutException)
				{
					Console.WriteLine("Sign-In operation timeout");
					return null;
				}
				catch (Exception ex)
				{
					Console.WriteLine($"Error getting access token: {ex.Message}");
					return null;
				}
			}
		}

	}
}
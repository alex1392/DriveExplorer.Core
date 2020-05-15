using System;
using System.Net.Http;
using Xunit;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using RichardSzalay.MockHttp;
using System.Text;

namespace DriveExplorer.ReqresApi
{
	public class ReqresApiTests
	{
		private HttpClient client;

		public ReqresApiTests()
		{
			client = new HttpClient();
			client.Timeout = TimeSpan.FromSeconds(1);
		}

		[Theory]
		[InlineData(1)]
		[InlineData(12)]
		public async Task GetSingleUser_SuccessResponseAsync(int id)
		{
			//Given
			var url = $"https://reqres.in/api/users/{id}";
			//When
			var response = await client.GetAsync(url);
			Console.WriteLine(response.StatusCode);
			var content = await response.Content.ReadAsStringAsync();
			//Then
			Assert.True(response.IsSuccessStatusCode);
			response.Dispose();
		}

		[Theory]
		[InlineData(0)]
		[InlineData(13)]
		public async Task GetSingleUser_ErrorNotFoundAsync(int id)
		{
			//Given
			var url = $"https://reqres.in/api/users/{id}";
			//When
			var response = await client.GetAsync(url);
			Console.WriteLine(response.StatusCode);
			//Then
			Assert.False(response.IsSuccessStatusCode);
			response.Dispose();
		}

		[Theory]
		[InlineData(1)]
		[InlineData(2)]
		public async Task GetListUsers_SuccessResponseAsync(int page)
		{
			//Given
			var url = $"https://reqres.in/api/users?page={page}";
			//When
			var response = await client.GetAsync(url);
			Console.WriteLine(response.StatusCode);
			//Then
			Assert.True(response.IsSuccessStatusCode);
			response.Dispose();
		}

		[Theory]
		[InlineData(1)]
		[InlineData(12)]
		public async Task GetSingleResource_SucessResponseAsync(int id)
		{
			//Given
			var url = $"https://reqres.in/api/unknown/{id}";
			//When
			var response = await client.GetAsync(url);
			var content = await response.Content.ReadAsStringAsync();
			Console.WriteLine(response.StatusCode);
			//Then
			Assert.True(response.IsSuccessStatusCode);
			response.Dispose();
		}

		[Theory]
		[InlineData(1)]
		[InlineData(2)]
		public async Task GetListResources_SuccessResponseAsync(int page)
		{
			//Given
			var url = $"https://reqres.in/api/unknown?page={page}";
			//When
			var response = await client.GetAsync(url);
			var content = await response.Content.ReadAsStringAsync();
			Console.WriteLine(response.StatusCode);
			//Then
			Assert.True(response.IsSuccessStatusCode);
			response.Dispose();
		}

		[Theory]
		[InlineData(0)]
		[InlineData(13)]
		public async Task GetSingleResource_ErrorNotFoundAsync(int id)
		{
			//Given
			var url = $"https://reqres.in/api/unknown/{id}";
			//When
			var response = await client.GetAsync(url);
			Console.WriteLine(response.StatusCode);
			//Then
			Assert.False(response.IsSuccessStatusCode);
			response.Dispose();
		}

		[Fact]
		public async Task CreateUser_IdenticalUserNameAsync()
		{
			//Given
			var url = $"https://reqres.in/api/users";
			var name = "morpheus";
			var content = new StringContent(
				"{\"name\":\"" + name + "\",\"job\":\"leader\"}", Encoding.UTF8, ContentTypes.Application.json); ;
			//When
			var response = await client.PostAsync(url, content);
			var responseBody = await response.Content.ReadAsStringAsync();
			Console.WriteLine(response.StatusCode);
			Console.WriteLine(responseBody);
			var jObject = JObject.Parse(responseBody);
			//Then
			Assert.Equal(name, (string)jObject["name"]);
			response.Dispose();
		}

		[Fact]
		public async Task UpdateUser_IdenticalUserNameAsync()
		{
			//Given
			var id = 1;
			var name = "morpheus";
			var url = $"https://reqres.in/api/users/{id}";
			var requestBody = new StringContent("{\"name\":\"" + name + "\",\"job\":\"leader\"}", Encoding.UTF8, ContentTypes.Application.json);
			//When
			var response = await client.PutAsync(url, requestBody);
			var responseBody = await response.Content.ReadAsStringAsync();
			Console.WriteLine(response.StatusCode);
			Console.WriteLine(responseBody);
			var jObject = JObject.Parse(responseBody);
			//Then
			Assert.Equal(name, (string)jObject["name"]);
			response.Dispose();
		}

		[Fact]
		public async Task PatchUser_IdenticalUserNameAsync()
		{
			//Given
			var id = 1;
			var name = "morpheus";
			var url = $"https://reqres.in/api/users/{id}";
			var requestBody = new StringContent("{\"name\":\"" + name + "\",\"job\":\"leader\"}", Encoding.UTF8, ContentTypes.Application.json);
			//When
			var response = await client.PatchAsync(url, requestBody);
			var responseBody = await response.Content.ReadAsStringAsync();
			Console.WriteLine(response.StatusCode);
			Console.WriteLine(responseBody);
			var jObject = JObject.Parse(responseBody);
			//Then
			Assert.Equal(name, (string)jObject["name"]);
			response.Dispose();
		}

		[Fact]
		public async Task DeleteUser_SuccessResponseAsync()
		{
			//Given
			var id = 1;
			var url = $"https://reqres.in/api/users/{id}";
			//When
			var response = await client.DeleteAsync(url);
			Console.WriteLine(response.StatusCode);
			//Then
			Assert.True(response.IsSuccessStatusCode);
			response.Dispose();
		}

		[Fact]
		public async Task RegisterUser_SuccessResponseAsync()
		{
			//Given
			var url = $"https://reqres.in/api/register";
			var requestBody = new StringContent(
				"{\"email\": \"eve.holt@reqres.in\",\"password\": \"pistol\"}", Encoding.UTF8, ContentTypes.Application.json);
			//When
			var response = await client.PostAsync(url, requestBody);
			var responseBody = await response.Content.ReadAsStringAsync();
			Console.WriteLine(response.StatusCode);
			Console.WriteLine(responseBody);
			//Then
			Assert.True(response.IsSuccessStatusCode);
			response.Dispose();
		}

		[Fact]
		public async Task RegisterUser_ErrorMissingPasswordAsync()
		{
			//Given
			var url = $"https://reqres.in/api/register";
			var requestBody = new StringContent(
				"{\"email\": \"eve.holt@reqres.in\"}", Encoding.UTF8, ContentTypes.Application.json);
			//When
			var response = await client.PostAsync(url, requestBody);
			var responseBody = await response.Content.ReadAsStringAsync();
			Console.WriteLine(response.StatusCode);
			Console.WriteLine(responseBody);
			//Then
			Assert.False(response.IsSuccessStatusCode);
			Assert.True(responseBody.Contains("error") && responseBody.Contains("password"));
			response.Dispose();
		}

		[Fact]
		public async Task LoginUser_SuccessResponseAsync()
		{
			//Given
			var url = $"https://reqres.in/api/login";
			var requestBody = new StringContent(
				"{\"email\": \"eve.holt@reqres.in\",\"password\": \"pistol\"}", Encoding.UTF8, ContentTypes.Application.json);
			//When
			var response = await client.PostAsync(url, requestBody);
			var responseBody = await response.Content.ReadAsStringAsync();
			Console.WriteLine(response.StatusCode);
			Console.WriteLine(responseBody);
			//Then
			Assert.True(response.IsSuccessStatusCode);
			response.Dispose();
		}

		[Fact]
		public async Task LoginUser_ErrorMissingPasswordAsync()
		{
			//Given
			var url = $"https://reqres.in/api/login";
			var requestBody = new StringContent(
				"{\"email\": \"eve.holt@reqres.in\"}", Encoding.UTF8, ContentTypes.Application.json);
			//When
			var response = await client.PostAsync(url, requestBody);
			var responseBody = await response.Content.ReadAsStringAsync();
			Console.WriteLine(response.StatusCode);
			Console.WriteLine(responseBody);
			//Then
			Assert.False(response.IsSuccessStatusCode);
			Assert.True(responseBody.Contains("error") && responseBody.Contains("password"));
			response.Dispose();
		}

		[Fact]
		public async Task GetSingleUserDelayed_ErrorTimeoutAsync()
		{
			//Given
			var id = 1;
			var delay = client.Timeout.TotalSeconds + 1;
			var url = $"https://reqres.in/api/users/{id}?delay={delay}";
			//When
			Func<Task> action = async () =>
			{
				try
				{
					await client.GetAsync(url);
				}
				catch (TaskCanceledException ex)
				{
					throw new TimeoutException("The process was cancelled due to timeout.", ex);
				}
			};
			//Then
			await Assert.ThrowsAsync<TimeoutException>(action);
		}
	}

	public class ReqresApiParsingTests
	{
		private MockHttpMessageHandler mockHttp;

		public ReqresApiParsingTests()
		{
			mockHttp = new MockHttpMessageHandler();
		}

		[Fact]
		public async Task ParseSingleUser_IdenticalUserIdAsync()
		{
			//Given
			var Id = 1;
			var url = $"https://reqres.in/api/users/{Id}";
			mockHttp.When(HttpMethod.Get, url)
				.Respond(request => new HttpResponseMessage
				{
					Content = new StringContent(
						"{\"data\":{\"id\":" + Id.ToString() + ",\"email\":\"george.bluth@reqres.in\",\"first_name\":\"George\",\"last_name\":\"Bluth\",\"avatar\":\"https://s3.amazonaws.com/uifaces/faces/twitter/calebogden/128.jpg\"},\"ad\":{\"company\":\"StatusCode Weekly\",\"url\":\"http://statuscode.org/\",\"text\":\"A weekly newsletter focusing on software development, infrastructure, the server, performance, and the stack end of things.\"}}"),
				});
			string content;
			using (var client = mockHttp.ToHttpClient())
			using (var response = await client.GetAsync(url))
			{
				content = await response.Content.ReadAsStringAsync();
			}
			//When
			var type = new { data = new User(), ad = new Ad() };
			var user = JsonConvert.DeserializeAnonymousType(content, type);
			//Then
			Assert.Equal(Id, user.data.id);
		}

		[Fact]
		public async Task ParseUserList_IdenticalUserIdAsync()
		{
			//Given
			var pageId = 1;
			var url = $"https://reqres.in/api/users?page={pageId}";
			mockHttp.When(HttpMethod.Get, url)
				.Respond(request => new HttpResponseMessage
				{
					Content = new StringContent(
						"{\"page\":" + pageId.ToString() + ",\"per_page\":6,\"total\":12,\"total_pages\":2,\"data\":[{\"id\":7,\"email\":\"michael.lawson@reqres.in\",\"first_name\":\"Michael\",\"last_name\":\"Lawson\",\"avatar\":\"https://s3.amazonaws.com/uifaces/faces/twitter/follettkyle/128.jpg\"},{\"id\":8,\"email\":\"lindsay.ferguson@reqres.in\",\"first_name\":\"Lindsay\",\"last_name\":\"Ferguson\",\"avatar\":\"https://s3.amazonaws.com/uifaces/faces/twitter/araa3185/128.jpg\"},{\"id\":9,\"email\":\"tobias.funke@reqres.in\",\"first_name\":\"Tobias\",\"last_name\":\"Funke\",\"avatar\":\"https://s3.amazonaws.com/uifaces/faces/twitter/vivekprvr/128.jpg\"},{\"id\":10,\"email\":\"byron.fields@reqres.in\",\"first_name\":\"Byron\",\"last_name\":\"Fields\",\"avatar\":\"https://s3.amazonaws.com/uifaces/faces/twitter/russoedu/128.jpg\"},{\"id\":11,\"email\":\"george.edwards@reqres.in\",\"first_name\":\"George\",\"last_name\":\"Edwards\",\"avatar\":\"https://s3.amazonaws.com/uifaces/faces/twitter/mrmoiree/128.jpg\"},{\"id\":12,\"email\":\"rachel.howell@reqres.in\",\"first_name\":\"Rachel\",\"last_name\":\"Howell\",\"avatar\":\"https://s3.amazonaws.com/uifaces/faces/twitter/hebertialmeida/128.jpg\"}],\"ad\":{\"company\":\"StatusCode Weekly\",\"url\":\"http://statuscode.org/\",\"text\":\"A weekly newsletter focusing on software development, infrastructure, the server, performance, and the stack end of things.\"}}"),
				});
			string content;
			using (var client = mockHttp.ToHttpClient())
			using (var response = await client.GetAsync(url))
			{
				content = await response.Content.ReadAsStringAsync();
			}
			//When
			var type = new
			{
				page = 0,
				per_page = 0,
				total = 0,
				total_pages = 0,
				data = new User[0],
				ad = new Ad(),
			};
			var result = JsonConvert.DeserializeAnonymousType(content, type);
			//Then
			Assert.Equal(pageId, result.page);
		}

		[Fact]
		public async Task ParseSingleResource_IdenticalResourceIdAsync()
		{
			//Given
			var id = 1;
			var url = $"https://reqres.in/api/unknown/{id}";
			mockHttp.When(HttpMethod.Get, url)
				.Respond((request) => new HttpResponseMessage
				{
					Content = new StringContent(
						"{\"data\":{\"id\":" + id.ToString() + ",\"name\":\"honeysuckle\",\"year\":2011,\"color\":\"#D94F70\",\"pantone_value\":\"18-2120\"},\"ad\":{\"company\":\"StatusCode Weekly\",\"url\":\"http://statuscode.org/\",\"text\":\"A weekly newsletter focusing on software development, infrastructure, the server, performance, and the stack end of things.\"}}")
				});
			var client = mockHttp.ToHttpClient();
			var response = await client.GetAsync(url);
			var content = await response.Content.ReadAsStringAsync();
			//When
			var type = new { data = new Resource(), ad = new Ad() };
			var result = JsonConvert.DeserializeAnonymousType(content, type);
			//Then
			Assert.Equal(id, result.data.id);
			response.Dispose();
		}

		[Fact]
		public async Task ParseResourceList_IdenticalResourceIdAsync()
		{
			//Given
			var id = 1;
			var url = $"https://reqres.in/api/unknown?page={id}";
			mockHttp.When(HttpMethod.Get, url)
				.Respond((request) => new HttpResponseMessage
				{
					Content = new StringContent(
						"{\"page\":" + id.ToString() + ",\"per_page\":6,\"total\":12,\"total_pages\":2,\"data\":[{\"id\":1,\"name\":\"cerulean\",\"year\":2000,\"color\":\"#98B2D1\",\"pantone_value\":\"15-4020\"},{\"id\":2,\"name\":\"fuchsia rose\",\"year\":2001,\"color\":\"#C74375\",\"pantone_value\":\"17-2031\"},{\"id\":3,\"name\":\"true red\",\"year\":2002,\"color\":\"#BF1932\",\"pantone_value\":\"19-1664\"},{\"id\":4,\"name\":\"aqua sky\",\"year\":2003,\"color\":\"#7BC4C4\",\"pantone_value\":\"14-4811\"},{\"id\":5,\"name\":\"tigerlily\",\"year\":2004,\"color\":\"#E2583E\",\"pantone_value\":\"17-1456\"},{\"id\":6,\"name\":\"blue turquoise\",\"year\":2005,\"color\":\"#53B0AE\",\"pantone_value\":\"15-5217\"}],\"ad\":{\"company\":\"StatusCode Weekly\",\"url\":\"http://statuscode.org/\",\"text\":\"A weekly newsletter focusing on software development, infrastructure, the server, performance, and the stack end of things.\"}}")
				});
			var client = mockHttp.ToHttpClient();
			var response = await client.GetAsync(url);
			var content = await response.Content.ReadAsStringAsync();
			//When
			var type = new
			{
				page = 0,
				per_page = 0,
				total = 0,
				total_pages = 0,
				data = new Resource[0],
				ad = new Ad()
			};
			var result = JsonConvert.DeserializeAnonymousType(content, type);
			//Then
			Assert.Equal(id, result.page);
			response.Dispose();
		}

	}

	public class Resource
	{
		public int id;
		public string name;
		public string year;
		public string color;
		public string pantone_value;
	}
	public class User
	{
		public int id;
		public string email;
		public string first_name;
		public string last_name;
		public string avatar;
	}
	public class Ad
	{
		public string company;
		public string url;
		public string text;
	}
}
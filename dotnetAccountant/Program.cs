using System;
using Microsoft.Extensions.Configuration;

namespace dotnetAccountant
{
    public class Program
    {
		public static IConfigurationRoot AppConfig { get; set; }

		static void Main(string[] args)
        {
            Console.WriteLine($"Hello {args?[0]}");

			// build app configuration
			AppConfig = GetAppConfig();
		}

		public static IConfigurationRoot GetAppConfig(){
			return new ConfigurationBuilder()
					.AddUserSecrets<Program>()
					.Build();
		}
    }
}

using System;
using Microsoft.Extensions.Configuration;

namespace dotnetAccountant
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }

		public static IConfigurationRoot LoadAppConfig(){
			// build app configuration
			var appConfig = new ConfigurationBuilder()
				.AddUserSecrets<Program>()
				.Build();
			return appConfig;
		}

		public static (string appId, string[] scopes) GetAppSettings(IConfigurationRoot appConfig){
			return (appConfig["appId"], appConfig["scopes"].Split(';'));
		}
    }
}

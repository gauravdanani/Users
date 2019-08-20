using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using Users.Application.Config;
using Users.Application.Helpers;
using Users.Application.Interfaces;
using Users.Application.Services;
using Users.Persistence.DataSources;
using Users.Persistence.Repositories;
using Users.Presentation;

namespace Users
{
    public class Program
    {
        private static IServiceProvider _serviceProvider;
        static void Main(string[] args)
        {
            RegisterServices();

            _serviceProvider.GetService<IConsoleApplication>().Run();

            DisposeServices();
        }

        private static void RegisterServices()
        {
            var collection = new ServiceCollection();
            //Load COnfiguration
            var config = LoadConfiguration();

            // Add the config to our DI container for later user
            collection.AddSingleton(config);
            collection.AddOptions();
            // Add our Config object so it can be injected
            collection.Configure<FileSourceConfig>(config.GetSection("FileSourceConfig"));

            collection.AddTransient<IFileDataParser, JsonParser>();
            collection.AddTransient<IUsersSource, FileDataSource>();
            collection.AddTransient<IUserRepository, UserRepository>();
            collection.AddTransient<IUserService, UserService>();
            collection.AddTransient<IFileHelper, FileHelper>();
            collection.AddTransient<IConsoleApplication, ConsoleApplication>();

            _serviceProvider = collection.AddLogging(cnfg => cnfg.AddConsole()).BuildServiceProvider();
        }

        public static IConfiguration LoadConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true,
                             reloadOnChange: true);
            return builder.Build();
        }

        private static void DisposeServices()
        {
            if (_serviceProvider == null)
            {
                return;
            }
            if (_serviceProvider is IDisposable)
            {
                ((IDisposable)_serviceProvider).Dispose();
            }
        }
    }
}

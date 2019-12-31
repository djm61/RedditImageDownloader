using System;
using System.IO;
using System.Runtime.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RedditImageDownloader.Data;
using RedditImageDownloader.Logging;

namespace RedditImageDownloader.Console
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var services = new ServiceCollection();
            ConfigureServices(services);

            var serviceProvider = services.BuildServiceProvider();

            var process = serviceProvider.GetService<Process.Process>();
            string input;
            do
            {
                System.Console.WriteLine("[A] to add a new source");
                System.Console.WriteLine("[R] to run");
                System.Console.WriteLine("[E] to exit");
                System.Console.Write("Choice: ");
                input = System.Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(input))
                {
                    if (input.ToUpper() == "A")
                    {
                        var name = GetNewName();
                        var niceName = GetNewNiceName();
                        var url = GetNewUrl();
                        var feed = GetNewFeedUrl();
                        process.AddSource(name, niceName, url, feed);
                    }
                    else if (input.ToUpper() == "R")
                    {
                        process.ParseSources();
                        process.DownloadImages();
                    }
                }

            } while (string.IsNullOrWhiteSpace(input) || input.ToUpper() != "E");
        }

        private static void ConfigureServices(IServiceCollection serviceCollection)
        {
            // build config
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.Development.json", optional: true, reloadOnChange: true)
                //.AddEnvironmentVariables()
                .Build();

            serviceCollection.AddSingleton(configuration);

            serviceCollection
                .AddLogging(configure =>
                {
                    configure.AddConfiguration(configuration.GetSection("Logging"));
                    configure.AddConsole();
                    configure.AddProvider(new Log4NetProvider("log4net.config", false));
                })
                .Configure<LoggerFilterOptions>(options => options.MinLevel = LogLevel.Debug);

            var connectionString = configuration.GetConnectionString("Default");
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new NullReferenceException("Default Connection String is missing");
            }

            serviceCollection.AddDbContext<RedditImageDownloaderContext>(options =>
                options.UseSqlite(connectionString)
                    .EnableDetailedErrors()
                    .EnableSensitiveDataLogging()
                    .UseLoggerFactory(new LoggerFactory()));

            serviceCollection.AddOptions();
            serviceCollection.Configure<ConnectionStrings>(configuration.GetSection("ConnectionStrings"));

            // add services:
            serviceCollection.AddSingleton<Process.Process>();
        }

        private static string GetNewName()
        {
            string name;
            do
            {
                System.Console.Write("Enter name: ");
                name = System.Console.ReadLine();
            } while (string.IsNullOrWhiteSpace(name));

            return name;
        }

        private static string GetNewNiceName()
        {
            string name;
            do
            {
                System.Console.Write("Enter nice name: ");
                name = System.Console.ReadLine();
            } while (string.IsNullOrWhiteSpace(name));

            return name;
        }

        private static string GetNewUrl()
        {
            string url;
            do
            {
                System.Console.Write("Enter url: ");
                url = System.Console.ReadLine();
            } while (string.IsNullOrWhiteSpace(url));

            return url;
        }

        private static string GetNewFeedUrl()
        {
            string url;
            do
            {
                System.Console.Write("Enter feed url: ");
                url = System.Console.ReadLine();
            } while (string.IsNullOrWhiteSpace(url));

            return url;
        }
    }
}

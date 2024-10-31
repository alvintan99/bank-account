using AwesomeGICBank.ConsoleApp.DependencyInjection;
using AwesomeGICBank.ConsoleApp.UI;
using AwesomeGICBank.Infrastructure.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace AwesomeGICBank.ConsoleApp
{
    public class Program
    {
        //public static Task Main(string[] args)
        //{
        //    System.Console.WriteLine("AwesomeGIC Bank");
        //    return Task.CompletedTask;
        //}

        public static async Task Main(string[] args)
        {
            // Create service collection
            var services = new ServiceCollection();

            // Register infrastructure services first
            services.AddInfrastructureServices();

            // Then register console services
            services.AddConsoleServices();

            // Build service provider
            var serviceProvider = services.BuildServiceProvider();

            // Create console UI
            var consoleUI = serviceProvider.GetRequiredService<ConsoleUI>();

            // Run the application
            await consoleUI.RunAsync();
        }
    }
}
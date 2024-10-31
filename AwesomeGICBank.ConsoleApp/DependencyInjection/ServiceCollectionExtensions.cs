using AwesomeGICBank.Application.UseCases;
using AwesomeGICBank.Application.UseCases.DefineInterestRule;
using AwesomeGICBank.Application.UseCases.GenerateStatement;
using AwesomeGICBank.ConsoleApp.Services;
using AwesomeGICBank.ConsoleApp.UI;
using Microsoft.Extensions.DependencyInjection;

namespace AwesomeGICBank.ConsoleApp.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddConsoleServices(this IServiceCollection services)
        {
            // Register console-specific services
            services.AddSingleton<ConsoleService>();
            services.AddSingleton<MenuHandler>();
            services.AddSingleton<ConsoleUI>();

            // Register use cases
            services.AddScoped<ProcessTransactionUseCase>();
            services.AddScoped<DefineInterestRuleUseCase>();
            services.AddScoped<GenerateStatementUseCase>();

            return services;
        }
    }
}

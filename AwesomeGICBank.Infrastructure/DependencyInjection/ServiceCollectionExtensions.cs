using AwesomeGICBank.Application.Common.Interfaces;
using AwesomeGICBank.Application.Common.Interfaces.Repositories;
using AwesomeGICBank.Infrastructure.Persistence.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace AwesomeGICBank.Infrastructure.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            // Register repositories
            services.AddSingleton<ITransactionRepository, InMemoryTransactionRepository>();
            services.AddSingleton<IInterestRuleRepository, InMemoryInterestRuleRepository>();

            return services;
        }
    }
}

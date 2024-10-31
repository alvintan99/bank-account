using AwesomeGICBank.Application.Common.Interfaces.Repositories;
using AwesomeGICBank.Infrastructure.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace AwesomeGICBank.Tests.Infrastructure.DependencyInjection
{
    public class ServiceCollectionExtensionsTests
    {
        [Fact]
        public void AddInfrastructureServices_ShouldRegisterAllServices()
        {
            var services = new ServiceCollection();

            services.AddInfrastructureServices();
            var serviceProvider = services.BuildServiceProvider();

            Assert.NotNull(serviceProvider.GetService<ITransactionRepository>());
            Assert.NotNull(serviceProvider.GetService<IInterestRuleRepository>());
        }
    }
}

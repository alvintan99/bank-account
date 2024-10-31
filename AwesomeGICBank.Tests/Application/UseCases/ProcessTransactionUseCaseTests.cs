using AwesomeGICBank.Application.Commands;
using AwesomeGICBank.Application.Common.Interfaces.Repositories;
using AwesomeGICBank.Application.UseCases;
using AwesomeGICBank.Domain.Models;
using Moq;

namespace AwesomeGICBank.Tests.Application.UseCases
{
    public class ProcessTransactionUseCaseTests
    {
        [Fact]
        public async Task ProcessTransaction_WithInsufficientFunds_ShouldReturnFailureResult()
        {
            var repository = new Mock<ITransactionRepository>();
            var useCase = new ProcessTransactionUseCase(repository.Object);

            var account = new Account(AccountId.From("AC001"));
            var transactionDate = new DateTime(2023, 06, 26);
            account.Deposit(transactionDate, Money.FromDecimal(50.00m));

            repository.Setup(r => r.GetAccountAsync(It.IsAny<AccountId>()))
                .ReturnsAsync(account);

            var result = await useCase.ProcessAsync(new ProcessTransactionCommand(
                DateTime.Today,
                "AC001",
                TransactionType.Withdrawal,
                60.00m));

            Assert.True(result.Failure);
            Assert.Contains("Insufficient funds", result.Error);
            Assert.Contains("50.00", result.Error);
        }
    }
}
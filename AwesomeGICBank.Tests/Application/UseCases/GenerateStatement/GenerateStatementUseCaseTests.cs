using AwesomeGICBank.Application.Commands;
using AwesomeGICBank.Application.Common.Interfaces.Repositories;
using AwesomeGICBank.Application.UseCases.GenerateStatement;
using AwesomeGICBank.Domain.Models;
using Moq;

namespace AwesomeGICBank.Tests.Application.UseCases.NewFolder
{
    public class GenerateStatementUseCaseTests
    {
        private readonly Mock<ITransactionRepository> _transactionRepository;
        private readonly Mock<IInterestRuleRepository> _interestRuleRepository;
        private readonly GenerateStatementUseCase _useCase;

        public GenerateStatementUseCaseTests()
        {
            _transactionRepository = new Mock<ITransactionRepository>();
            _interestRuleRepository = new Mock<IInterestRuleRepository>();
            _useCase = new GenerateStatementUseCase(_transactionRepository.Object, _interestRuleRepository.Object);
        }

        [Fact]
        public async Task ProcessAsync_AccountNotFound_ShouldReturnFailure()
        {
            var command = new GenerateStatementCommand("AC001", 2024, 1);
            _transactionRepository.Setup(r => r.GetAccountAsync(It.IsAny<AccountId>()))
                .ReturnsAsync((Account?)null);

            var result = await _useCase.ProcessAsync(command);

            Assert.True(result.Failure);
            Assert.Contains("not found", result.Error);
        }

        [Fact]
        public async Task ProcessAsync_ValidAccount_ShouldReturnStatement()
        {
            var accountId = AccountId.From("AC001");
            var account = new Account(accountId);
            var command = new GenerateStatementCommand("AC001", 2024, 1);

            _transactionRepository.Setup(r => r.GetAccountAsync(It.IsAny<AccountId>()))
                .ReturnsAsync(account);
            _transactionRepository.Setup(r => r.GetTransactionsForPeriodAsync(
                It.IsAny<AccountId>(),
                It.IsAny<DateTime>(),
                It.IsAny<DateTime>()))
                .ReturnsAsync(new List<Transaction>());
            _interestRuleRepository.Setup(r => r.GetEffectiveRulesAsync(
                It.IsAny<DateTime>(),
                It.IsAny<DateTime>()))
                .ReturnsAsync(new List<InterestRule>());

            var result = await _useCase.ProcessAsync(command);

            Assert.True(result.Success);
            Assert.NotNull(result.Value);
            Assert.Equal("AC001", result.Value.AccountId);
        }
    }
}

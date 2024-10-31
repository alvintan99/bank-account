using AwesomeGICBank.Domain.Models;
using AwesomeGICBank.Infrastructure.Persistence.Repositories;

namespace AwesomeGICBank.Tests.Infrastructure.Persistence.Repositories
{
    public class InMemoryTransactionRepositoryTests
    {
        private readonly InMemoryTransactionRepository _repository;

        public InMemoryTransactionRepositoryTests()
        {
            _repository = new InMemoryTransactionRepository();
        }

        [Fact]
        public async Task GetAccountAsync_NonExistentAccount_ShouldReturnNull()
        {
            var accountId = AccountId.From("AC001");

            var result = await _repository.GetAccountAsync(accountId);

            Assert.Null(result);
        }

        [Fact]
        public async Task SaveAccountAsync_NewAccount_ShouldPersistAccount()
        {
            var accountId = AccountId.From("AC001");
            var account = new Account(accountId);
            account.Deposit(DateTime.Today, Money.FromDecimal(100m));

            await _repository.SaveAccountAsync(account);
            var savedAccount = await _repository.GetAccountAsync(accountId);

            Assert.NotNull(savedAccount);
            Assert.Equal(account.Id.Value, savedAccount.Id.Value);
            Assert.Equal(100m, savedAccount.GetBalance().ToDecimal());
        }

        [Fact]
        public async Task SaveAccountAsync_ExistingAccount_ShouldUpdateAccount()
        {
            var accountId = AccountId.From("AC001");
            var account = new Account(accountId);

            // First deposit
            account.Deposit(DateTime.Today, Money.FromDecimal(100m));
            await _repository.SaveAccountAsync(account);

            // Second deposit
            account.Deposit(DateTime.Today, Money.FromDecimal(50m));
            await _repository.SaveAccountAsync(account);

            var savedAccount = await _repository.GetAccountAsync(accountId);

            Assert.NotNull(savedAccount);
            Assert.Equal(150m, savedAccount.GetBalance().ToDecimal());
            Assert.Equal(2, savedAccount.GetTransactions().Count);
        }

        [Fact]
        public async Task GetTransactionsForPeriodAsync_WithTransactions_ShouldReturnFilteredTransactions()
        {
            var accountId = AccountId.From("AC001");
            var account = new Account(accountId);
            var date1 = new DateTime(2024, 1, 1);
            var date2 = new DateTime(2024, 1, 15);
            var date3 = new DateTime(2024, 2, 1);

            account.Deposit(date1, Money.FromDecimal(100m));
            account.Deposit(date2, Money.FromDecimal(50m));
            account.Deposit(date3, Money.FromDecimal(75m));
            await _repository.SaveAccountAsync(account);

            var transactions = await _repository.GetTransactionsForPeriodAsync(
                accountId,
                new DateTime(2024, 1, 1),
                new DateTime(2024, 1, 31));

            Assert.Equal(2, transactions.Count());
            Assert.All(transactions, t => Assert.True(t.Date >= date1 && t.Date <= date2));
            Assert.Equal(150m, transactions.Sum(t => t.Amount.ToDecimal()));
        }

        [Fact]
        public async Task GetTransactionsForPeriodAsync_NoTransactions_ShouldReturnEmpty()
        {
            var accountId = AccountId.From("AC001");

            var transactions = await _repository.GetTransactionsForPeriodAsync(
                accountId,
                DateTime.Today,
                DateTime.Today.AddDays(1));

            Assert.Empty(transactions);
        }
    }
}

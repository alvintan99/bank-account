using AwesomeGICBank.Application.Common.Interfaces.Repositories;
using AwesomeGICBank.Domain.Models;

namespace AwesomeGICBank.Infrastructure.Persistence.Repositories
{
    public class InMemoryTransactionRepository : ITransactionRepository
    {
        private readonly Dictionary<string, Account> _accounts = new();

        public Task<Account?> GetAccountAsync(AccountId id)
        {
            _accounts.TryGetValue(id.Value, out var account);
            return Task.FromResult(account);
        }

        public Task SaveAccountAsync(Account account)
        {
            _accounts[account.Id.Value] = account;
            return Task.CompletedTask;
        }

        public Task<IEnumerable<Transaction>> GetTransactionsForPeriodAsync(
            AccountId accountId,
            DateTime startDate,
            DateTime endDate)
        {
            if (!_accounts.TryGetValue(accountId.Value, out var account))
                return Task.FromResult(Enumerable.Empty<Transaction>());

            // Convert IOrderedEnumerable to IEnumerable
            IEnumerable<Transaction> transactions = account.GetTransactions()
                .Where(t => t.Date >= startDate && t.Date <= endDate)
                .OrderBy(t => t.Date)
                .ThenBy(t => t.Id?.Value)
                .ToList();

            return Task.FromResult(transactions);
        }
    }
}

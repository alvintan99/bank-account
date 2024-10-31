using AwesomeGICBank.Domain.Models;

namespace AwesomeGICBank.Application.Common.Interfaces.Repositories
{
    public interface ITransactionRepository
    {
        Task<Account?> GetAccountAsync(AccountId id);
        Task SaveAccountAsync(Account account);
        Task<IEnumerable<Transaction>> GetTransactionsForPeriodAsync(
            AccountId accountId,
            DateTime startDate,
            DateTime endDate);
    }
}

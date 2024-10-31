using AwesomeGICBank.Domain.Models;

namespace AwesomeGICBank.Application.Common.Results
{
    public record StatementResult
    {
        public string AccountId { get; }
        public List<Transaction> Transactions { get; }

        public StatementResult(string accountId, IEnumerable<Transaction> transactions)
        {
            AccountId = accountId;
            Transactions = transactions.ToList();
        }
    }
}

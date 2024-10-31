using AwesomeGICBank.Domain.Exceptions;

namespace AwesomeGICBank.Domain.Models
{
    public class Account
    {
        public AccountId Id { get; }
        private Money _balance = Money.Zero;
        private readonly List<Transaction> _transactions = new List<Transaction>();

        public Account(AccountId id)
        {
            Id = id;
        }

        public void Deposit(DateTime date, Money amount)
        {
            _balance = _balance.Add(amount);
            var transaction = Transaction.CreateDeposit(
                TransactionId.Generate(date, _transactions
                    .Count(t => t.Date.Date == date.Date) + 1),
                date,
                Id,
                amount);
            transaction.SetResultingBalance(_balance);
            _transactions.Add(transaction);
        }

        public void Withdraw(DateTime date, Money amount)
        {
            var newBalance = _balance.Subtract(amount);
            if (newBalance.ToDecimal() < 0)
                throw new InsufficientFundsException(Id.Value, amount.ToDecimal(), _balance.ToDecimal());

            _balance = newBalance;
            var transaction = Transaction.CreateWithdrawal(
                TransactionId.Generate(date, _transactions
                    .Count(t => t.Date.Date == date.Date) + 1),
                date,
                Id,
                amount);
            transaction.SetResultingBalance(_balance);
            _transactions.Add(transaction);
        }

        public Money GetBalance() => _balance;
        public IReadOnlyList<Transaction> GetTransactions() => _transactions.AsReadOnly();
    }
}

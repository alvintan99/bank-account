namespace AwesomeGICBank.Domain.Models
{
    public class Transaction
    {
        public TransactionId Id { get; }
        public DateTime Date { get; }
        public AccountId AccountId { get; }
        public TransactionType Type { get; }
        public Money Amount { get; }
        public Money ResultingBalance { get; private set; }

        private Transaction(
            TransactionId id,
            DateTime date,
            AccountId accountId,
            TransactionType type,
            Money amount)
        {
            Id = id;
            Date = date;
            AccountId = accountId;
            Type = type;
            Amount = amount;
            ResultingBalance = Money.Zero;
        }

        public static Transaction CreateDeposit(
            TransactionId id,
            DateTime date,
            AccountId accountId,
            Money amount)
        {
            return new Transaction(id, date, accountId, TransactionType.Deposit, amount);
        }

        public static Transaction CreateWithdrawal(
            TransactionId id,
            DateTime date,
            AccountId accountId,
            Money amount)
        {
            return new Transaction(id, date, accountId, TransactionType.Withdrawal, amount);
        }

        public static Transaction CreateInterest(
            DateTime date,
            AccountId accountId,
            Money amount)
        {
            return new Transaction(null!, date, accountId, TransactionType.Interest, amount);
        }

        public void SetResultingBalance(Money balance)
        {
            ResultingBalance = balance;
        }
    }
}

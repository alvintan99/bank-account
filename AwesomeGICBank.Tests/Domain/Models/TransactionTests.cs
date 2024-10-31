using AwesomeGICBank.Domain.Models;

namespace AwesomeGICBank.Tests.Domain.Models
{
    public class TransactionTests
    {
        private readonly AccountId _accountId = AccountId.From("AC001");
        private readonly DateTime _date = new DateTime(2024, 1, 1);

        [Fact]
        public void CreateDeposit_ValidParameters_ShouldCreateTransaction()
        {
            var id = TransactionId.Generate(_date, 1);
            var amount = Money.FromDecimal(100m);

            var transaction = Transaction.CreateDeposit(id, _date, _accountId, amount);

            Assert.Equal(id, transaction.Id);
            Assert.Equal(_date, transaction.Date);
            Assert.Equal(_accountId, transaction.AccountId);
            Assert.Equal(TransactionType.Deposit, transaction.Type);
            Assert.Equal(amount.ToDecimal(), transaction.Amount.ToDecimal());
        }

        [Fact]
        public void CreateWithdrawal_ValidParameters_ShouldCreateTransaction()
        {
            var id = TransactionId.Generate(_date, 1);
            var amount = Money.FromDecimal(100m);

            var transaction = Transaction.CreateWithdrawal(id, _date, _accountId, amount);

            Assert.Equal(id, transaction.Id);
            Assert.Equal(_date, transaction.Date);
            Assert.Equal(_accountId, transaction.AccountId);
            Assert.Equal(TransactionType.Withdrawal, transaction.Type);
            Assert.Equal(amount.ToDecimal(), transaction.Amount.ToDecimal());
        }

        [Fact]
        public void CreateInterest_ValidParameters_ShouldCreateTransaction()
        {
            var amount = Money.FromDecimal(10m);

            var transaction = Transaction.CreateInterest(_date, _accountId, amount);

            Assert.Equal(_date, transaction.Date);
            Assert.Equal(_accountId, transaction.AccountId);
            Assert.Equal(TransactionType.Interest, transaction.Type);
            Assert.Equal(amount.ToDecimal(), transaction.Amount.ToDecimal());
        }

        [Fact]
        public void SetResultingBalance_ShouldUpdateBalance()
        {
            var transaction = Transaction.CreateDeposit(
                TransactionId.Generate(_date, 1),
                _date,
                _accountId,
                Money.FromDecimal(100m));
            var newBalance = Money.FromDecimal(500m);

            transaction.SetResultingBalance(newBalance);

            Assert.Equal(newBalance.ToDecimal(), transaction.ResultingBalance.ToDecimal());
        }
    }
}

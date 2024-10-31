using AwesomeGICBank.Domain.Exceptions;
using AwesomeGICBank.Domain.Models;

namespace AwesomeGICBank.Tests.Domain.Models
{
    public class AccountTests
    {
        [Fact]
        public void CreateAccount_WithValidId_ShouldSucceed()
        {
            var exception = Record.Exception(() => AccountId.From("AC001"));

            Assert.Null(exception);
        }

        [Fact]
        public void Deposit_ShouldIncreaseBalance()
        {
            var account = new Account(AccountId.From("AC001"));
            var amount = Money.FromDecimal(100.00m);
            var transactionDate = new DateTime(2023, 06, 26);

            account.Deposit(transactionDate, amount);

            Assert.Equal(100.00m, account.GetBalance().ToDecimal());
        }

        [Fact]
        public void Withdraw_WithSufficientFunds_ShouldDecreaseBalance()
        {
            var account = new Account(AccountId.From("AC001"));
            var transactionDate = new DateTime(2023, 06, 26);

            account.Deposit(transactionDate, Money.FromDecimal(100.00m));

            account.Withdraw(transactionDate, Money.FromDecimal(60.00m));

            Assert.Equal(40.00m, account.GetBalance().ToDecimal());
        }

        [Fact]
        public void Withdraw_WithInsufficientFunds_ShouldThrowException()
        {
            var account = new Account(AccountId.From("AC001"));
            var transactionDate = new DateTime(2023, 06, 26);

            account.Deposit(transactionDate, Money.FromDecimal(50.00m));

            var exception = Assert.Throws<InsufficientFundsException>(() => account.Withdraw(transactionDate, Money.FromDecimal(60.00m)));
            Assert.Contains("Insufficient funds", exception.Message);
        }

        [Fact]
        public void Withdraw_WithInsufficientFunds_ShouldThrowDetailedException()
        {
            var account = new Account(AccountId.From("AC001"));
            var transactionDate = new DateTime(2023, 06, 26);

            account.Deposit(transactionDate, Money.FromDecimal(50.00m));
            var withdrawalAmount = Money.FromDecimal(60.00m);

            var exception = Assert.Throws<InsufficientFundsException>(() => account.Withdraw(transactionDate, withdrawalAmount));

            Assert.Equal("AC001", exception.AccountId);
            Assert.Equal(60.00m, exception.AttemptedAmount);
            Assert.Equal(50.00m, exception.AvailableAmount);
        }

        [Fact]
        public void Transaction_ShouldBeRecorded()
        {
            var account = new Account(AccountId.From("AC001"));
            var amount = Money.FromDecimal(100.00m);
            var transactionDate = new DateTime(2023, 06, 26);

            account.Deposit(transactionDate, amount);

            var transactions = account.GetTransactions();
            Assert.Single(transactions);
            Assert.Equal(TransactionType.Deposit, transactions[0].Type);
            Assert.Equal(100.00m, transactions[0].Amount.ToDecimal());
        }
    }
}
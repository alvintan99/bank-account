using AwesomeGICBank.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwesomeGICBank.Tests.Domain.Models
{
    public class InterestCalculationTests
    {
        [Fact]
        public void Calculate_WithValidData_ShouldReturnCorrectResult()
        {
            var accountId = AccountId.From("AC001");
            var startDate = new DateTime(2024, 1, 1);
            var transactions = new List<Transaction>
            {
                Transaction.CreateDeposit(
                    TransactionId.Generate(startDate, 1),
                    startDate,
                    accountId,
                    Money.FromDecimal(1000m))
            };
            var rules = new List<InterestRule>
            {
                InterestRule.Create(startDate, "RULE1", 3.65m)
            };

            var result = InterestCalculation.Calculate(
                accountId,
                transactions,
                rules,
                2024,
                1);

            Assert.NotNull(result);
            Assert.True(result.TotalInterest.ToDecimal() > 0);
            Assert.Equal(TransactionType.Interest, result.InterestTransaction.Type);
        }
    }
}

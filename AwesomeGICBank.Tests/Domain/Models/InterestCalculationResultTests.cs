using AwesomeGICBank.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwesomeGICBank.Tests.Domain.Models
{
    public class InterestCalculationResultTests
    {
        [Fact]
        public void Create_ValidParameters_ShouldCreateResult()
        {
            var accountId = AccountId.From("AC001");
            var startDate = new DateTime(2024, 1, 1);
            var balance = Money.FromDecimal(1000m);
            var rule = InterestRule.Create(startDate, "RULE1", 3.65m);
            var periods = new List<InterestPeriod>
            {
                new InterestPeriod(
                    startDate,
                    startDate.AddDays(30),
                    31,
                    balance,
                    rule,
                    36.50m)
            };
            var totalInterest = Money.FromDecimal(3.10m);
            var interestTransaction = Transaction.CreateInterest(
                startDate.AddMonths(1),
                accountId,
                totalInterest);

            var result = new InterestCalculationResult(
                periods,
                totalInterest,
                interestTransaction);

            Assert.Equal(periods, result.Periods);
            Assert.Equal(totalInterest, result.TotalInterest);
            Assert.Equal(interestTransaction, result.InterestTransaction);
        }
    }
}

using AwesomeGICBank.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwesomeGICBank.Tests.Domain.Models
{
    public class InterestPeriodTests
    {
        [Fact]
        public void Create_ValidParameters_ShouldCreateInterestPeriod()
        {
            var startDate = new DateTime(2024, 1, 1);
            var endDate = new DateTime(2024, 1, 31);
            var balance = Money.FromDecimal(1000m);
            var rule = InterestRule.Create(startDate, "RULE1", 3.65m);
            var annualizedInterest = 36.50m; // 1000 * 3.65%

            var period = new InterestPeriod(
                startDate,
                endDate,
                31,
                balance,
                rule,
                annualizedInterest);

            Assert.Equal(startDate, period.StartDate);
            Assert.Equal(endDate, period.EndDate);
            Assert.Equal(31, period.NumberOfDays);
            Assert.Equal(balance, period.Balance);
            Assert.Equal(rule, period.Rule);
            Assert.Equal(annualizedInterest, period.AnnualizedInterest);
            Assert.Equal(0.1m, period.DailyInterest); // 36.50/365
        }
    }
}

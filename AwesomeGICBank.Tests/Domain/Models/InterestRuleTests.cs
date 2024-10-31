using AwesomeGICBank.Domain.Exceptions;
using AwesomeGICBank.Domain.Models;

namespace AwesomeGICBank.Tests.Domain.Models
{
    public class InterestRuleTests
    {
        [Fact]
        public void Create_ValidParameters_ShouldSucceed()
        {
            var rule = InterestRule.Create(DateTime.Today, "RULE1", 1.5m);

            Assert.NotNull(rule);
            Assert.Equal("RULE1", rule.RuleId);
            Assert.Equal(1.5m, rule.Rate);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(100)]
        [InlineData(101)]
        public void Create_InvalidRate_ShouldThrowException(decimal rate)
        {
            Assert.Throws<InvalidInterestRateException>(() =>
                InterestRule.Create(DateTime.Today, "RULE1", rate));
        }

        [Fact]
        public void CalculateDailyInterest_ShouldReturnCorrectAmount()
        {
            var rule = InterestRule.Create(DateTime.Today, "RULE1", 3.65m); // 3.65% annual rate
            var balance = Money.FromDecimal(1000m);

            var dailyInterest = rule.CalculateDailyInterest(balance);

            // 1000 * (3.65/100) / 365 = 0.1m per day
            Assert.Equal(0.1m, dailyInterest);
        }
    }
}

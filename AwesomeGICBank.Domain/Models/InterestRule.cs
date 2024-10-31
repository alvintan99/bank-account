using AwesomeGICBank.Domain.Exceptions;

namespace AwesomeGICBank.Domain.Models
{
    public class InterestRule
    {
        public DateTime EffectiveDate { get; }
        public string RuleId { get; }
        public decimal Rate { get; }

        private InterestRule(DateTime effectiveDate, string ruleId, decimal rate)
        {
            EffectiveDate = effectiveDate;
            RuleId = ruleId;
            Rate = rate;
        }

        public static InterestRule Create(DateTime effectiveDate, string ruleId, decimal rate)
        {
            if (rate <= 0 || rate >= 100)
                throw InvalidInterestRateException.OutOfRange(rate);
            return new InterestRule(effectiveDate, ruleId, rate);
        }

        public decimal CalculateDailyInterest(Money balance)
        {
            return balance.ToDecimal() * (Rate / 100m) / 365m;
        }
    }
}

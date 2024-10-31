namespace AwesomeGICBank.Domain.Models
{
    public record InterestPeriod(
            DateTime StartDate,
            DateTime EndDate,
            int NumberOfDays,
            Money Balance,
            InterestRule Rule,
            decimal AnnualizedInterest)
    {
        public decimal DailyInterest => AnnualizedInterest / 365m;
    }
}

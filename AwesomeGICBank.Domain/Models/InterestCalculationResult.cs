namespace AwesomeGICBank.Domain.Models
{
    public class InterestCalculationResult
    {
        public List<InterestPeriod> Periods { get; }
        public Money TotalInterest { get; }
        public Transaction InterestTransaction { get; }

        public InterestCalculationResult(
            List<InterestPeriod> periods,
            Money totalInterest,
            Transaction interestTransaction)
        {
            Periods = periods;
            TotalInterest = totalInterest;
            InterestTransaction = interestTransaction;
        }
    }
}

namespace AwesomeGICBank.Domain.Models
{
    public class InterestCalculation
    {
        public static InterestCalculationResult Calculate(
            AccountId accountId,
            IEnumerable<Transaction> transactions,
            IEnumerable<InterestRule> rules,
            int year,
            int month)
        {
            var startDate = new DateTime(year, month, 1);
            var endDate = startDate.AddMonths(1).AddDays(-1);
            var dailyBalances = CalculateDailyBalances(transactions, startDate, endDate);
            var periods = new List<InterestPeriod>();

            // Get periods where either balance or interest rule changes
            var balanceChangePoints = dailyBalances
                .Select(b => b.Key)
                .OrderBy(d => d)
                .ToList();

            var ruleChangePoints = rules
                .Where(r => r.EffectiveDate >= startDate && r.EffectiveDate <= endDate)
                .Select(r => r.EffectiveDate)
                .OrderBy(d => d)
                .ToList();

            var allChangePoints = balanceChangePoints
                .Union(ruleChangePoints)
                .OrderBy(d => d)
                .ToList();

            // Calculate interest for each period
            DateTime periodStart = startDate;
            foreach (var changePoint in allChangePoints.Concat(new[] { endDate.AddDays(1) }))
            {
                if (periodStart >= changePoint) continue;

                var periodEnd = changePoint.AddDays(-1);
                var balance = dailyBalances[periodStart];
                var applicableRule = GetApplicableRule(rules, periodStart);

                if (applicableRule != null)
                {
                    var numberOfDays = (periodEnd - periodStart).Days + 1;
                    var annualizedInterest = balance.ToDecimal() *
                        (applicableRule.Rate / 100m) *
                        numberOfDays;

                    periods.Add(new InterestPeriod(
                        periodStart,
                        periodEnd,
                        numberOfDays,
                        balance,
                        applicableRule,
                        annualizedInterest));
                }

                periodStart = changePoint;
            }

            // Calculate total interest
            var totalAnnualizedInterest = periods.Sum(p => p.AnnualizedInterest);
            var totalDailyInterest = Math.Round(totalAnnualizedInterest / 365m, 2);
            var totalInterest = Money.FromDecimal(totalDailyInterest);

            // Create interest transaction
            var interestTransaction = Transaction.CreateInterest(
                endDate,
                accountId,
                totalInterest);

            return new InterestCalculationResult(periods, totalInterest, interestTransaction);
        }

        private static Dictionary<DateTime, Money> CalculateDailyBalances(
            IEnumerable<Transaction> transactions,
            DateTime startDate,
            DateTime endDate)
        {
            var dailyBalances = new Dictionary<DateTime, Money>();
            var currentBalance = Money.Zero;
            var currentDate = startDate;

            var orderedTransactions = transactions
                .Where(t => t.Date >= startDate && t.Date <= endDate)
                .OrderBy(t => t.Date)
                .ThenBy(t => t.Id?.Value)
                .ToList();

            var transactionIndex = 0;

            while (currentDate <= endDate)
            {
                // Process all transactions for the current date
                while (transactionIndex < orderedTransactions.Count &&
                       orderedTransactions[transactionIndex].Date == currentDate)
                {
                    var transaction = orderedTransactions[transactionIndex];
                    if (transaction.Type == TransactionType.Deposit)
                        currentBalance = currentBalance.Add(transaction.Amount);
                    else if (transaction.Type == TransactionType.Withdrawal)
                        currentBalance = currentBalance.Subtract(transaction.Amount);

                    transactionIndex++;
                }

                dailyBalances[currentDate] = currentBalance;
                currentDate = currentDate.AddDays(1);
            }

            return dailyBalances;
        }

        private static InterestRule? GetApplicableRule(
            IEnumerable<InterestRule> rules,
            DateTime date)
        {
            return rules
                .Where(r => r.EffectiveDate <= date)
                .OrderByDescending(r => r.EffectiveDate)
                .FirstOrDefault();
        }
    }
}

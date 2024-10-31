using AwesomeGICBank.Application.Commands;
using AwesomeGICBank.Application.Common.Interfaces.Repositories;
using AwesomeGICBank.Application.Common.Results;
using AwesomeGICBank.Domain.Models;

namespace AwesomeGICBank.Application.UseCases.GenerateStatement
{
    public class GenerateStatementUseCase
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IInterestRuleRepository _interestRuleRepository;

        public GenerateStatementUseCase(
            ITransactionRepository transactionRepository,
            IInterestRuleRepository interestRuleRepository)
        {
            _transactionRepository = transactionRepository;
            _interestRuleRepository = interestRuleRepository;
        }

        public async Task<Result<StatementResult>> ProcessAsync(GenerateStatementCommand command)
        {
            try
            {
                var accountId = AccountId.From(command.AccountId);
                var startDate = new DateTime(command.Year, command.Month, 1);
                var endDate = startDate.AddMonths(1).AddDays(-1);

                var account = await _transactionRepository.GetAccountAsync(accountId);
                if (account == null)
                    return Result<StatementResult>.Fail($"Account {command.AccountId} not found");

                var transactions = await _transactionRepository.GetTransactionsForPeriodAsync(accountId, startDate, endDate);

                // Get all interest rules that might apply to this period
                var rules = await _interestRuleRepository.GetEffectiveRulesAsync(startDate, endDate);

                // Calculate interest and get the interest transaction
                var interestCalc = InterestCalculation.Calculate(
                    accountId,
                    transactions,
                    rules,
                    command.Year,
                    command.Month);

                // Add interest transaction to the list if interest is greater than zero
                var allTransactions = transactions.ToList();
                if (interestCalc.TotalInterest.ToDecimal() > 0)
                {
                    allTransactions.Add(interestCalc.InterestTransaction);
                }

                // Order transactions by date
                allTransactions = allTransactions
                    .OrderBy(t => t.Date)
                    .ThenBy(t => t.Id?.Value)
                    .ToList();

                return Result<StatementResult>.Ok(new StatementResult(command.AccountId, allTransactions));
            }
            catch (Exception ex)
            {
                return Result<StatementResult>.Fail(ex.Message);
            }
        }
    }
}

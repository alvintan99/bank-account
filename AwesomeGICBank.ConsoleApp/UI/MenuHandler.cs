using AwesomeGICBank.Application.Common.Results;
using AwesomeGICBank.Application.UseCases;
using AwesomeGICBank.Application.UseCases.DefineInterestRule;
using AwesomeGICBank.Application.UseCases.GenerateStatement;
using AwesomeGICBank.ConsoleApp.Services;
using AwesomeGICBank.ConsoleApp.UI.Parsers;
using AwesomeGICBank.Domain.Extensions;
using AwesomeGICBank.Domain.Models;

namespace AwesomeGICBank.ConsoleApp.UI
{
    public class MenuHandler
    {
        private readonly ProcessTransactionUseCase _processTransactionUseCase;
        private readonly DefineInterestRuleUseCase _defineInterestRuleUseCase;
        private readonly GenerateStatementUseCase _generateStatementUseCase;
        private readonly ConsoleService _consoleService;

        public MenuHandler(
            ProcessTransactionUseCase processTransactionUseCase,
            DefineInterestRuleUseCase defineInterestRuleUseCase,
            GenerateStatementUseCase generateStatementUseCase,
            ConsoleService consoleService)
        {
            _processTransactionUseCase = processTransactionUseCase;
            _defineInterestRuleUseCase = defineInterestRuleUseCase;
            _generateStatementUseCase = generateStatementUseCase;
            _consoleService = consoleService;
        }

        public void DisplayMainMenu()
        {
            _consoleService.WriteLine("[T] Input transactions");
            _consoleService.WriteLine("[I] Define interest rules");
            _consoleService.WriteLine("[P] Print statement");
            _consoleService.WriteLine("[Q] Quit");
            _consoleService.Write(">");
        }

        public async Task HandleTransactionInputAsync()
        {
            _consoleService.WriteLine("Please enter transaction details in <Date> <Account> <Type> <Amount> format");
            _consoleService.WriteLine("(or enter blank to go back to main menu):");
            _consoleService.Write(">");

            var input = _consoleService.ReadLine()?.Trim();
            if (string.IsNullOrWhiteSpace(input))
                return;

            var command = TransactionCommandParser.Parse(input);
            if (command == null)
            {
                _consoleService.WriteLine("Invalid input format. Please try again.");
                return;
            }

            var result = await _processTransactionUseCase.ProcessAsync(command);
            if (result.Success)
            {
                await DisplayAccountTransactions(command.AccountId);
            }
            else
            {
                _consoleService.WriteLine($"Error: {result.Error}");
            }
        }

        private async Task DisplayAccountTransactions(string accountId)
        {
            var account = await _processTransactionUseCase.GetAccountAsync(accountId);
            if (account == null)
            {
                _consoleService.WriteLine($"Account {accountId} not found.");
                return;
            }

            var transactions = account.GetTransactions().OrderBy(t => t.Date).ThenBy(t => t.Id?.Value);

            _consoleService.WriteLine($"\nAccount: {accountId}");
            _consoleService.WriteLine("| Date     | Txn Id      | Type | Amount |");

            foreach (var transaction in transactions)
            {
                _consoleService.WriteLine(
                    $"| {transaction.Date:yyyyMMdd} | " +
                    $"{transaction.Id,-10} | " +
                    $"{transaction.Type.ToDisplayCode()}    | " +
                    $"{transaction.Amount.ToDecimal(),6:F2} |");
            }
            _consoleService.WriteLine("");
            _consoleService.WriteLine("Is there anything else you'd like to do?");
        }

        public async Task HandleInterestRuleInputAsync()
        {
            _consoleService.WriteLine("Please enter interest rules details in <Date> <RuleId> <Rate in %> format");
            _consoleService.WriteLine("(or enter blank to go back to main menu):");
            _consoleService.Write(">");

            var input = _consoleService.ReadLine()?.Trim();
            if (string.IsNullOrWhiteSpace(input))
                return;

            var command = InterestRuleCommandParser.Parse(input);
            if (command == null)
            {
                _consoleService.WriteLine("Invalid input format. Please try again.");
                return;
            }

            var result = await _defineInterestRuleUseCase.ProcessAsync(command);
            if (result.Success)
            {
                await DisplayInterestRules();
            }
            else
            {
                _consoleService.WriteLine($"Error: {result.Error}");
            }
        }

        private async Task DisplayInterestRules()
        {
            var rules = await _defineInterestRuleUseCase.GetAllRulesAsync();

            _consoleService.WriteLine("\nInterest rules:");
            _consoleService.WriteLine("| Date     | RuleId | Rate (%) |");

            foreach (var rule in rules.OrderBy(r => r.EffectiveDate))
            {
                _consoleService.WriteLine(
                    $"| {rule.EffectiveDate:yyyyMMdd} | " +
                    $"{rule.RuleId,-6} | " +
                    $"{rule.Rate,8:F2} |");
            }

            _consoleService.WriteLine("");
            _consoleService.WriteLine("Is there anything else you'd like to do?");
        }

        public async Task HandlePrintStatementAsync()
        {
            _consoleService.WriteLine("Please enter account and month to generate the statement <Account> <Year><Month>");
            _consoleService.WriteLine("(or enter blank to go back to main menu):");
            _consoleService.Write(">");

            var input = _consoleService.ReadLine()?.Trim();
            if (string.IsNullOrWhiteSpace(input))
                return;

            var command = StatementCommandParser.Parse(input);
            if (command == null)
            {
                _consoleService.WriteLine("Invalid input format. Please try again.");
                return;
            }

            var result = await _generateStatementUseCase.ProcessAsync(command);
            if (result.Success)
            {
                PrintStatement(result.Value);

                _consoleService.WriteLine("");
                _consoleService.WriteLine("Is there anything else you'd like to do?");
            }
            else
            {
                _consoleService.WriteLine($"Error: {result.Error}");
            }
        }

        private void PrintStatement(StatementResult statement)
        {
            _consoleService.WriteLine($"\nAccount: {statement.AccountId}");
            _consoleService.WriteLine("| Date     | Txn Id      | Type | Amount | Balance |");

            var runningBalance = Money.Zero;
            foreach (var transaction in statement.Transactions)
            {
                // Update running balance
                if (transaction.Type == TransactionType.Deposit)
                    runningBalance = runningBalance.Add(transaction.Amount);
                else if (transaction.Type == TransactionType.Withdrawal)
                    runningBalance = runningBalance.Subtract(transaction.Amount);
                else if (transaction.Type == TransactionType.Interest)
                    runningBalance = runningBalance.Add(transaction.Amount);

                // Format transaction ID (empty for interest transactions)
                var txnId = transaction.Type == TransactionType.Interest ?
                    string.Empty :
                    transaction.Id?.Value;

                _consoleService.WriteLine(
                    $"| {transaction.Date:yyyyMMdd} | " +
                    $"{txnId,-11} | " +
                    $"{transaction.Type.ToDisplayCode()}    | " +
                    $"{transaction.Amount.ToDecimal(),6:F2} | " +
                    $"{runningBalance.ToDecimal(),7:F2} |");
            }
        }

        public async Task<Result<TransactionResult>> ProcessTransactionAsync(string input)
        {
            var command = TransactionCommandParser.Parse(input);
            if (command == null)
                return Result<TransactionResult>.Fail("Invalid input format");

            return await _processTransactionUseCase.ProcessAsync(command);
        }
    }
}

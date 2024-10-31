using AwesomeGICBank.Application.Commands;
using AwesomeGICBank.Domain.Models;
using System.Globalization;

namespace AwesomeGICBank.ConsoleApp.UI.Parsers
{
    public static class TransactionCommandParser
    {
        public static ProcessTransactionCommand? Parse(string input)
        {
            var parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length != 4)
                return null;

            if (!DateTime.TryParseExact(parts[0], "yyyyMMdd",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out var transactionDate))
                return null;

            var accountId = parts[1];
            if (string.IsNullOrWhiteSpace(accountId))
                return null;

            var typeStr = parts[2].ToUpper();
            var type = typeStr switch
            {
                "D" => TransactionType.Deposit,
                "W" => TransactionType.Withdrawal,
                _ => throw new ArgumentException("Invalid transaction type")
            };

            if (!decimal.TryParse(parts[3], out var amount))
                return null;

            return new ProcessTransactionCommand(transactionDate, accountId, type, amount);
        }
    }
}

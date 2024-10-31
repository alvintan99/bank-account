using AwesomeGICBank.Application.Commands;

namespace AwesomeGICBank.ConsoleApp.UI.Parsers
{
    public static class StatementCommandParser
    {
        public static GenerateStatementCommand? Parse(string input)
        {
            var parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length != 2)
                return null;

            var accountId = parts[0];
            if (string.IsNullOrWhiteSpace(accountId))
                return null;

            if (!int.TryParse(parts[1], out var yearMonth) || parts[1].Length != 6)
                return null;

            int year = yearMonth / 100;
            int month = yearMonth % 100;

            if (month < 1 || month > 12)
                return null;

            return new GenerateStatementCommand(accountId, year, month);
        }
    }
}

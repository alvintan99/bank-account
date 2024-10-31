using AwesomeGICBank.Application.Commands;
using System.Globalization;

namespace AwesomeGICBank.ConsoleApp.UI.Parsers
{
    public static class InterestRuleCommandParser
    {
        public static DefineInterestRuleCommand? Parse(string input)
        {
            var parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length != 3)
                return null;

            if (!DateTime.TryParseExact(parts[0], "yyyyMMdd",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out var date))
                return null;

            var ruleId = parts[1];
            if (string.IsNullOrWhiteSpace(ruleId))
                return null;

            if (!decimal.TryParse(parts[2],
                NumberStyles.Number,
                CultureInfo.InvariantCulture,
                out var rate))
                return null;

            return new DefineInterestRuleCommand(date, ruleId, rate);
        }
    }
}

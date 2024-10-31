using AwesomeGICBank.Application.Commands;
using AwesomeGICBank.ConsoleApp.UI.Parsers;

namespace AwesomeGICBank.Tests.ConsoleApp.UI.Parsers
{
    public class StatementCommandParserTests
    {
        [Theory]
        [InlineData("AC001 202401")]
        public void Parse_ValidInput_ShouldReturnCommand(string input)
        {
            var result = StatementCommandParser.Parse(input);

            Assert.NotNull(result);
            Assert.IsType<GenerateStatementCommand>(result);
        }

        [Theory]
        [InlineData("")]
        [InlineData("AC001")]
        [InlineData("AC001 2024")] // Invalid year/month format
        [InlineData("AC001 202413")] // Invalid month
        public void Parse_InvalidInput_ShouldReturnNull(string input)
        {
            var result = StatementCommandParser.Parse(input);

            Assert.Null(result);
        }
    }
}

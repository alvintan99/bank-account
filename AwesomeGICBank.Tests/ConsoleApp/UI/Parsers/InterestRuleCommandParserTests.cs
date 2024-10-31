using AwesomeGICBank.Application.Commands;
using AwesomeGICBank.ConsoleApp.UI.Parsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwesomeGICBank.Tests.ConsoleApp.UI.Parsers
{
    public class InterestRuleCommandParserTests
    {
        [Theory]
        [InlineData("20240101 RULE1 1.5")]
        public void Parse_ValidInput_ShouldReturnCommand(string input)
        {
            var result = InterestRuleCommandParser.Parse(input);

            Assert.NotNull(result);
            Assert.IsType<DefineInterestRuleCommand>(result);
        }

        [Theory]
        [InlineData("")]
        [InlineData("invalid")]
        [InlineData("20240101 RULE1")] // Missing rate
        [InlineData("date RULE1 1.5")] // Invalid date
        public void Parse_InvalidInput_ShouldReturnNull(string input)
        {
            var result = InterestRuleCommandParser.Parse(input);

            Assert.Null(result);
        }
    }
}

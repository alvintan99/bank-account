using AwesomeGICBank.Application.Commands;
using AwesomeGICBank.ConsoleApp.UI.Parsers;
using AwesomeGICBank.Domain.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwesomeGICBank.Tests.ConsoleApp.UI.Parsers
{
    public class TransactionCommandParserTests
    {
        [Theory]
        [InlineData("20240101 AC001 D 100.00", "20240101", "AC001", TransactionType.Deposit, 100.00)]
        [InlineData("20240101 AC001 W 50.00", "20240101", "AC001", TransactionType.Withdrawal, 50.00)]
        public void Parse_ValidInput_ShouldReturnCommand(
            string input,
            string expectedDate,
            string expectedAccount,
            TransactionType expectedType,
            decimal expectedAmount)
        {
            var result = TransactionCommandParser.Parse(input);

            Assert.NotNull(result);
            Assert.Equal(DateTime.ParseExact(expectedDate, "yyyyMMdd", CultureInfo.InvariantCulture), result.Date);
            Assert.Equal(expectedAccount, result.AccountId);
            Assert.Equal(expectedType, result.Type);
            Assert.Equal(expectedAmount, result.Amount);
        }

        [Theory]
        [InlineData("")] // Empty string
        [InlineData("invalid")] // Invalid format
        [InlineData("20240101 AC001 D")] // Missing amount
        [InlineData("invalid AC001 D 100.00")] // Invalid date
        [InlineData("20240101  D 100.00")] // Empty account
        [InlineData("20240101 AC001 D invalid")] // Invalid amount
        public void Parse_InvalidInput_ShouldReturnNull(string input)
        {
            var result = TransactionCommandParser.Parse(input);

            Assert.Null(result);
        }

        [Theory]
        [InlineData("20240101 AC001 X 100.00")] // Invalid transaction type
        [InlineData("20240101 AC001 I 100.00")] // Interest type not allowed
        public void Parse_InvalidTransactionType_ShouldThrowArgumentException(string input)
        {
            var exception = Assert.Throws<ArgumentException>(() => TransactionCommandParser.Parse(input));
            Assert.Equal("Invalid transaction type", exception.Message);
        }

        [Theory]
        [InlineData("20240101 AC001 d 100.00")] // Lowercase deposit
        [InlineData("20240101 AC001 w 100.00")] // Lowercase withdrawal
        public void Parse_CaseInsensitiveTransactionType_ShouldReturnCommand(string input)
        {
            var result = TransactionCommandParser.Parse(input);

            Assert.NotNull(result);
        }

        [Theory]
        [InlineData("20240101  AC001  D  100.00")] // Multiple spaces
        [InlineData(" 20240101 AC001 D 100.00 ")] // Leading/trailing spaces
        public void Parse_ExtraWhitespace_ShouldReturnCommand(string input)
        {
            var result = TransactionCommandParser.Parse(input);

            Assert.NotNull(result);
        }
    }
}

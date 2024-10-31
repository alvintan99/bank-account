using AwesomeGICBank.Domain.Models;
using AwesomeGICBank.Domain.Extensions;

namespace AwesomeGICBank.Tests.Domain.Extensions
{
    public class TransactionTypeExtensionsTests
    {
        [Theory]
        [InlineData(TransactionType.Deposit, "D")]
        [InlineData(TransactionType.Withdrawal, "W")]
        [InlineData(TransactionType.Interest, "I")]
        public void ToDisplayCode_ValidType_ShouldReturnCorrectCode(TransactionType type, string expected)
        {
            var result = type.ToDisplayCode();

            Assert.Equal(expected, result);
        }
    }
}

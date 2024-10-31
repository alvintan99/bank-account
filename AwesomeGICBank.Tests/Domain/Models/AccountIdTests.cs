using AwesomeGICBank.Domain.Exceptions;
using AwesomeGICBank.Domain.Models;

namespace AwesomeGICBank.Tests.Domain.Models
{
    public class AccountIdTests
    {
        [Fact]
        public void CreateAccount_WithValidId_ShouldSucceed()
        {
            var exception = Record.Exception(() => AccountId.From("AC001"));

            Assert.Null(exception);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData(" ")]
        public void CreateAccount_WithInvalidId_ShouldThrowException(string invalidId)
        {
            var exception = Assert.Throws<InvalidAccountIdException>(() => AccountId.From(invalidId));

            Assert.Contains("Account ID cannot be empty", exception.Message);
        }
    }
}
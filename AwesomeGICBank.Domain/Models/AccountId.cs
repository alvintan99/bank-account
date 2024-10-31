using AwesomeGICBank.Domain.Exceptions;

namespace AwesomeGICBank.Domain.Models
{
    public class AccountId
    {
        public string Value { get; }

        private AccountId(string value)
        {
            Value = value;
        }

        public static AccountId From(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new InvalidAccountIdException("Account ID cannot be empty");

            return new AccountId(value);
        }
    }
}

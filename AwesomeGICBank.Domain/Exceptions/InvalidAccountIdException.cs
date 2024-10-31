namespace AwesomeGICBank.Domain.Exceptions
{
    public class InvalidAccountIdException : DomainExceptionBase
    {
        public InvalidAccountIdException(string message) : base(message) { }

        public static InvalidAccountIdException Empty() =>
            new InvalidAccountIdException("Account ID cannot be empty");

        public static InvalidAccountIdException Invalid(string accountId) =>
            new InvalidAccountIdException($"Account ID '{accountId}' is invalid");
    }
}

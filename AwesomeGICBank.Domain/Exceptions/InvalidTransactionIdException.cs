namespace AwesomeGICBank.Domain.Exceptions
{
    public class InvalidTransactionIdException : DomainExceptionBase
    {
        public InvalidTransactionIdException(string message) : base(message) { }

        //public static InvalidTransactionIdException InvalidFormat(string value) =>
        //    new InvalidTransactionIdException($"Transaction ID format is invalid: {value}");

        public static InvalidTransactionIdException InvalidSequence() =>
            new InvalidTransactionIdException("Sequence must be greater than zero");
    }
}

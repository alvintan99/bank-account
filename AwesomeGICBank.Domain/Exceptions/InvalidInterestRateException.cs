namespace AwesomeGICBank.Domain.Exceptions
{
    public class InvalidInterestRateException : DomainExceptionBase
    {
        public InvalidInterestRateException(string message) : base(message) { }

        public static InvalidInterestRateException OutOfRange(decimal rate) =>
            new InvalidInterestRateException($"Interest rate of {rate} is invalid. It must be between 0 and 100.");
    }
}

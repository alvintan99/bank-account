namespace AwesomeGICBank.Domain.Exceptions
{
    public class InsufficientFundsException : DomainExceptionBase
    {
        public InsufficientFundsException(string accountId, decimal attempted, decimal available)
            : base($"Insufficient funds in account {accountId}. Attempted: {attempted:C}, Available: {available:C}")
        {
            AccountId = accountId;
            AttemptedAmount = attempted;
            AvailableAmount = available;
        }

        public string AccountId { get; }
        public decimal AttemptedAmount { get; }
        public decimal AvailableAmount { get; }
    }
}

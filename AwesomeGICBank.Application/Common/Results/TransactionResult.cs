namespace AwesomeGICBank.Application.Common.Results
{
    public class TransactionResult
    {
        public TransactionResult(
            bool success,
            string? transactionId = null,
            decimal? balance = null,
            string? error = null)
        {
            Success = success;
            TransactionId = transactionId;
            Balance = balance;
            Error = error;
        }

        public bool Success { get; }
        public string? TransactionId { get; }
        public decimal? Balance { get; }
        public string? Error { get; }

        public static TransactionResult Successful(string transactionId, decimal balance) =>
            new(true, transactionId, balance);

        public static TransactionResult Failed(string error) =>
            new(false, error: error);
    }
}

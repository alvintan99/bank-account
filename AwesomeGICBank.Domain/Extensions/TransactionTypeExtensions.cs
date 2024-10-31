using AwesomeGICBank.Domain.Models;

namespace AwesomeGICBank.Domain.Extensions
{
    public static class TransactionTypeExtensions
    {
        public static string ToDisplayCode(this TransactionType type)
        {
            return type switch
            {
                TransactionType.Deposit => "D",
                TransactionType.Withdrawal => "W",
                TransactionType.Interest => "I",
                _ => throw new ArgumentException($"Unknown transaction type: {type}")
            };
        }
    }
}

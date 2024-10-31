using AwesomeGICBank.Domain.Models;

namespace AwesomeGICBank.Application.Commands
{
    public record ProcessTransactionCommand(DateTime Date, string AccountId, TransactionType Type, decimal Amount);
}

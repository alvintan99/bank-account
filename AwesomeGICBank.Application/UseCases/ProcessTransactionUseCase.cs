using AwesomeGICBank.Application.Commands;
using AwesomeGICBank.Application.Common.Interfaces.Repositories;
using AwesomeGICBank.Application.Common.Results;
using AwesomeGICBank.Domain.Exceptions;
using AwesomeGICBank.Domain.Models;

namespace AwesomeGICBank.Application.UseCases
{
    public class ProcessTransactionUseCase
    {
        private readonly ITransactionRepository _transactionRepository;

        public ProcessTransactionUseCase(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        public async Task<Account?> GetAccountAsync(string accountId)
        {
            try
            {
                return await _transactionRepository.GetAccountAsync(AccountId.From(accountId));
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<Result<TransactionResult>> ProcessAsync(ProcessTransactionCommand command)
        {
            try
            {
                var accountId = AccountId.From(command.AccountId);
                var amount = Money.FromDecimal(command.Amount);

                var account = await _transactionRepository.GetAccountAsync(accountId) ?? new Account(accountId);

                if (command.Type == TransactionType.Deposit)
                {
                    account.Deposit(command.Date, amount);
                }
                else
                {
                    account.Withdraw(command.Date, amount);
                }

                await _transactionRepository.SaveAccountAsync(account);

                var transaction = account.GetTransactions().Last();

                return Result<TransactionResult>.Ok(
                    TransactionResult.Successful(
                        transaction.Id.Value,
                        transaction.ResultingBalance.ToDecimal()));
            }
            catch (InvalidAccountIdException)
            {
                return Result<TransactionResult>.Fail("Invalid account ID provided");
            }
            catch (InvalidMoneyException)
            {
                return Result<TransactionResult>.Fail("Invalid amount provided");
            }
            catch (InsufficientFundsException ex)
            {
                return Result<TransactionResult>.Fail($"Insufficient funds. Available balance: {ex.AvailableAmount:C}");
            }
            catch (Exception)
            {
                return Result<TransactionResult>.Fail("An unexpected error occurred");
            }
        }
    }
}

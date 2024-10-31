namespace AwesomeGICBank.Application.Commands
{
    public record GenerateStatementCommand(string AccountId, int Year, int Month);
}

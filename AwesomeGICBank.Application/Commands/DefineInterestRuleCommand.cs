namespace AwesomeGICBank.Application.Commands
{
    public record DefineInterestRuleCommand(DateTime EffectiveDate, string RuleId, decimal Rate);
}

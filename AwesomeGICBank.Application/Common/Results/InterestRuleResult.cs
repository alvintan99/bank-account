namespace AwesomeGICBank.Application.Common.Results
{
    public record InterestRuleResult
    {
        public InterestRuleResult(bool success)
        {
            Success = success;
        }

        public bool Success { get; }
        public string? RuleId { get; init; }
        public decimal? Rate { get; init; }
    }
}

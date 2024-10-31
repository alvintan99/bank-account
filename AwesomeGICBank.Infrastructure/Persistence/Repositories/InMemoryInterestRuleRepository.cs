using AwesomeGICBank.Application.Common.Interfaces.Repositories;
using AwesomeGICBank.Domain.Models;

namespace AwesomeGICBank.Infrastructure.Persistence.Repositories
{
    public class InMemoryInterestRuleRepository : IInterestRuleRepository
    {
        private readonly List<InterestRule> _rules = new();

        public Task<List<InterestRule>> GetEffectiveRulesAsync(DateTime startDate, DateTime endDate)
        {
            var rules = _rules
                .Where(r => r.EffectiveDate <= endDate)
                .OrderBy(r => r.EffectiveDate)
                .ToList();

            return Task.FromResult(rules);
        }

        public Task SaveRuleAsync(InterestRule rule)
        {
            // Remove any existing rule with the same date
            _rules.RemoveAll(r => r.EffectiveDate == rule.EffectiveDate);
            _rules.Add(rule);
            return Task.CompletedTask;
        }

        public Task<InterestRule?> GetRuleByIdAsync(string ruleId)
        {
            var rule = _rules.FirstOrDefault(r => r.RuleId == ruleId);
            return Task.FromResult(rule);
        }

        public Task<List<InterestRule>> GetAllRulesAsync()
        {
            return Task.FromResult(_rules.OrderBy(r => r.EffectiveDate).ToList());
        }
    }
}

using AwesomeGICBank.Domain.Models;

namespace AwesomeGICBank.Application.Common.Interfaces.Repositories
{
    public interface IInterestRuleRepository
    {
        Task<List<InterestRule>> GetEffectiveRulesAsync(DateTime startDate, DateTime endDate);
        Task SaveRuleAsync(InterestRule rule);
        Task<InterestRule?> GetRuleByIdAsync(string ruleId);
        Task<List<InterestRule>> GetAllRulesAsync();
    }
}

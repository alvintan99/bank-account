using AwesomeGICBank.Application.Commands;
using AwesomeGICBank.Application.Common.Interfaces.Repositories;
using AwesomeGICBank.Application.Common.Results;
using AwesomeGICBank.Domain.Models;

namespace AwesomeGICBank.Application.UseCases.DefineInterestRule
{
    public class DefineInterestRuleUseCase
    {
        private readonly IInterestRuleRepository _repository;

        public DefineInterestRuleUseCase(IInterestRuleRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result<InterestRuleResult>> ProcessAsync(DefineInterestRuleCommand command)
        {
            try
            {
                var rule = InterestRule.Create(command.EffectiveDate, command.RuleId, command.Rate);
                await _repository.SaveRuleAsync(rule);

                return Result<InterestRuleResult>.Ok(new InterestRuleResult(true)
                {
                    RuleId = rule.RuleId,
                    Rate = rule.Rate
                });
            }
            catch (Exception ex)
            {
                return Result<InterestRuleResult>.Fail(ex.Message);
            }
        }

        public async Task<List<InterestRule>> GetAllRulesAsync()
        {
            return await _repository.GetAllRulesAsync();
        }
    }
}

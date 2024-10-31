using AwesomeGICBank.Domain.Models;
using AwesomeGICBank.Infrastructure.Persistence.Repositories;

namespace AwesomeGICBank.Tests.Infrastructure.Persistence.Repositories
{
    public class InMemoryInterestRuleRepositoryTests
    {
        private readonly InMemoryInterestRuleRepository _repository;

        public InMemoryInterestRuleRepositoryTests()
        {
            _repository = new InMemoryInterestRuleRepository();
        }

        [Fact]
        public async Task SaveRuleAsync_NewRule_ShouldPersistRule()
        {
            var rule = InterestRule.Create(DateTime.Today, "RULE1", 1.5m);

            await _repository.SaveRuleAsync(rule);
            var rules = await _repository.GetAllRulesAsync();

            Assert.Single(rules);
            Assert.Equal(rule.RuleId, rules.First().RuleId);
            Assert.Equal(rule.Rate, rules.First().Rate);
        }

        [Fact]
        public async Task SaveRuleAsync_DuplicateEffectiveDate_ShouldOverwriteRule()
        {
            var date = DateTime.Today;
            var rule1 = InterestRule.Create(date, "RULE1", 1.5m);
            var rule2 = InterestRule.Create(date, "RULE2", 2.0m);

            await _repository.SaveRuleAsync(rule1);
            await _repository.SaveRuleAsync(rule2);
            var rules = await _repository.GetAllRulesAsync();

            Assert.Single(rules);
            Assert.Equal(rule2.RuleId, rules.First().RuleId);
            Assert.Equal(2.0m, rules.First().Rate);
        }

        [Fact]
        public async Task GetEffectiveRulesAsync_WithinPeriod_ShouldReturnFilteredRules()
        {
            var date1 = new DateTime(2024, 1, 1);
            var date2 = new DateTime(2024, 1, 15);
            var date3 = new DateTime(2024, 2, 1);

            await _repository.SaveRuleAsync(InterestRule.Create(date1, "RULE1", 1.5m));
            await _repository.SaveRuleAsync(InterestRule.Create(date2, "RULE2", 2.0m));
            await _repository.SaveRuleAsync(InterestRule.Create(date3, "RULE3", 2.5m));

            var rules = await _repository.GetEffectiveRulesAsync(
                new DateTime(2024, 1, 1),
                new DateTime(2024, 1, 31));

            Assert.Equal(2, rules.Count);
            Assert.All(rules, r => Assert.True(r.EffectiveDate >= date1 && r.EffectiveDate <= date2));
        }

        [Fact]
        public async Task GetRuleByIdAsync_ExistingRule_ShouldReturnRule()
        {
            var rule = InterestRule.Create(DateTime.Today, "RULE1", 1.5m);
            await _repository.SaveRuleAsync(rule);

            var result = await _repository.GetRuleByIdAsync("RULE1");

            Assert.NotNull(result);
            Assert.Equal(rule.RuleId, result.RuleId);
            Assert.Equal(rule.Rate, result.Rate);
        }

        [Fact]
        public async Task GetRuleByIdAsync_NonExistentRule_ShouldReturnNull()
        {
            var result = await _repository.GetRuleByIdAsync("NONEXISTENT");

            Assert.Null(result);
        }
    }
}

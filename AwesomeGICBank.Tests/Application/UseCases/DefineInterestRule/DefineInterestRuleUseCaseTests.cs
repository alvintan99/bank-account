using AwesomeGICBank.Application.Commands;
using AwesomeGICBank.Application.Common.Interfaces.Repositories;
using AwesomeGICBank.Application.UseCases.DefineInterestRule;
using AwesomeGICBank.Domain.Models;
using Moq;

namespace AwesomeGICBank.Tests.Application.UseCases.DefineInterestRule
{
    public class DefineInterestRuleUseCaseTests
    {
        private readonly Mock<IInterestRuleRepository> _repository;
        private readonly DefineInterestRuleUseCase _useCase;

        public DefineInterestRuleUseCaseTests()
        {
            _repository = new Mock<IInterestRuleRepository>();
            _useCase = new DefineInterestRuleUseCase(_repository.Object);
        }

        [Fact]
        public async Task ProcessAsync_ValidRule_ShouldSucceed()
        {
            var command = new DefineInterestRuleCommand(DateTime.Today, "RULE1", 1.5m);

            var result = await _useCase.ProcessAsync(command);

            Assert.True(result.Success);
            Assert.Equal("RULE1", result.Value.RuleId);
            Assert.Equal(1.5m, result.Value.Rate);
            _repository.Verify(r => r.SaveRuleAsync(It.IsAny<InterestRule>()), Times.Once);
        }

        [Fact]
        public async Task GetAllRulesAsync_ShouldReturnRules()
        {
            var rules = new List<InterestRule>
            {
                InterestRule.Create(DateTime.Today, "RULE1", 1.5m)
            };
            _repository.Setup(r => r.GetAllRulesAsync()).ReturnsAsync(rules);

            var result = await _useCase.GetAllRulesAsync();

            Assert.NotEmpty(result);
            Assert.Equal(rules.Count, result.Count);
        }
    }
}

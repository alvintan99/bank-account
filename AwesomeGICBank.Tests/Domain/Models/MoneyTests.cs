using AwesomeGICBank.Domain.Exceptions;
using AwesomeGICBank.Domain.Models;

namespace AwesomeGICBank.Tests.Domain.Models
{
    public class MoneyTests
    {
        [Fact]
        public void CreateMoney_WithValidAmount_ShouldSucceed()
        {
            var money = Money.FromDecimal(100.50m);

            Assert.Equal(100.50m, money.ToDecimal());
        }

        [Fact]
        public void Add_TwoAmounts_ShouldReturnCorrectSum()
        {
            var money1 = Money.FromDecimal(100.50m);
            var money2 = Money.FromDecimal(50.25m);

            var result = money1.Add(money2);

            Assert.Equal(150.75m, result.ToDecimal());
        }

        [Fact]
        public void Subtract_TwoAmounts_ShouldReturnCorrectDifference()
        {
            var money1 = Money.FromDecimal(100.50m);
            var money2 = Money.FromDecimal(50.25m);

            var result = money1.Subtract(money2);

            Assert.Equal(50.25m, result.ToDecimal());
        }

        [Theory]
        [InlineData(100.50)]
        [InlineData(0.01)]
        [InlineData(1000000.00)]
        [InlineData(0)]
        public void FromDecimal_ValidAmount_ShouldCreateMoney(decimal amount)
        {
            var money = Money.FromDecimal(amount);

            Assert.Equal(amount, money.ToDecimal());
        }

        [Theory]
        [InlineData(-0.01)]
        [InlineData(-100.00)]
        [InlineData(-100.50)]
        [InlineData(-1000000.00)]
        public void FromDecimal_NegativeAmount_ShouldThrowException(decimal amount)
        {
            Assert.Throws<InvalidMoneyException>(() => Money.FromDecimal(amount));
        }

        [Fact]
        public void Add_TwoValidAmounts_ShouldReturnCorrectSum()
        {
            var money1 = Money.FromDecimal(100.00m);
            var money2 = Money.FromDecimal(50.50m);

            var result = money1.Add(money2);

            Assert.Equal(150.50m, result.ToDecimal());
        }

        [Fact]
        public void Add_WithZero_ShouldReturnSameAmount()
        {
            var money = Money.FromDecimal(100.00m);
            var zero = Money.Zero;

            var result = money.Add(zero);

            Assert.Equal(100.00m, result.ToDecimal());
        }


        [Fact]
        public void Subtract_TwoValidAmounts_ShouldReturnCorrectDifference()
        {
            var money1 = Money.FromDecimal(100.00m);
            var money2 = Money.FromDecimal(50.50m);

            var result = money1.Subtract(money2);

            Assert.Equal(49.50m, result.ToDecimal());
        }

        [Fact]
        public void Subtract_WithZero_ShouldReturnSameAmount()
        {
            var money = Money.FromDecimal(100.00m);
            var zero = Money.Zero;

            var result = money.Subtract(zero);

            Assert.Equal(100.00m, result.ToDecimal());
        }

        [Fact]
        public void IsZero_ZeroAmount_ShouldReturnTrue()
        {
            var money = Money.Zero;

            Assert.True(money.IsZero());
        }

        [Fact]
        public void IsZero_NonZeroAmount_ShouldReturnFalse()
        {
            var money = Money.FromDecimal(100.00m);

            Assert.False(money.IsZero());
        }

        [Theory]
        [InlineData(100.50, "100.50")]
        [InlineData(0.00, "0.00")]
        [InlineData(1000.00, "1000.00")]
        public void ToString_ShouldReturnFormattedString(decimal amount, string expected)
        {
            var money = Money.FromDecimal(amount);

            var result = money.ToString();

            Assert.Equal(expected, result);
        }
    }
}
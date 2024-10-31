using AwesomeGICBank.Domain.Exceptions;

namespace AwesomeGICBank.Domain.Models
{
    public class Money
    {
        private decimal Amount { get; }
        private static readonly int DecimalPlaces = 2;

        private Money(decimal amount)
        {
            Amount = Math.Round(amount, DecimalPlaces);
        }

        public static Money FromDecimal(decimal amount)
        {
            if (amount < 0)
                throw InvalidMoneyException.NegativeAmount();
            return new Money(amount);
        }

        public Money Add(Money other) => new Money(Amount + other.Amount);
        public Money Subtract(Money other) => new Money(Amount - other.Amount);
        public decimal ToDecimal() => Amount;

        // Add ToString override
        public override string ToString()
        {
            return Amount.ToString("F2");
        }

        public static Money Zero => new Money(0);

        public bool IsZero() => Amount == 0;
    }
}

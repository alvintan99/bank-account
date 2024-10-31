namespace AwesomeGICBank.Domain.Exceptions
{
    public class InvalidMoneyException : DomainExceptionBase
    {
        public InvalidMoneyException(string message) : base(message) { }

        public static InvalidMoneyException NegativeAmount() =>
            new InvalidMoneyException("Amount cannot be negative");

        //public static InvalidMoneyException NonPositiveAmount() =>
        //    new InvalidMoneyException("Amount must be greater than zero");

        //public static InvalidMoneyException TooManyDecimalPlaces(int maxDecimalPlaces) =>
        //    new InvalidMoneyException($"Amount cannot have more than {maxDecimalPlaces} decimal places");
    }
}

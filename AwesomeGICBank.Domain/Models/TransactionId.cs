using AwesomeGICBank.Domain.Exceptions;

namespace AwesomeGICBank.Domain.Models
{
    public class TransactionId
    {
        public string Value { get; }

        private TransactionId(string value)
        {
            Value = value;
        }

        public static TransactionId Generate(DateTime date, int sequence)
        {
            if (sequence <= 0)
                throw InvalidTransactionIdException.InvalidSequence();

            return new TransactionId($"{date:yyyyMMdd}-{sequence:D2}");
        }

        public override string ToString() => Value;

        public override bool Equals(object? obj)
        {
            if (obj is TransactionId other)
            {
                return Value.Equals(other.Value);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
    }
}

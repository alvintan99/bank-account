namespace AwesomeGICBank.Application.Common.Results
{
    public class Result<T> : Result where T : class
    {
        private readonly T? _value;

        protected internal Result(T? value, bool success, string error = "")
            : base(success, error)
        {
            _value = value;
        }

        public T Value => Success
            ? _value ?? throw new InvalidOperationException("Value was not set")
            : throw new InvalidOperationException("Cannot access value of failed result");

        public static Result<T> Ok(T value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value), "Value cannot be null for success result");
            return new Result<T>(value, true);
        }

        public static new Result<T> Fail(string error) => new Result<T>(default, false, error);

        public static implicit operator Result<T>(T value) => Ok(value);
    }
}

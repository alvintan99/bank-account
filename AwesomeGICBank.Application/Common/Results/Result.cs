namespace AwesomeGICBank.Application.Common.Results
{
    public class Result
    {
        protected Result(bool success, string error = "")
        {
            Success = success;
            Error = error;
        }

        public bool Success { get; }
        public string Error { get; }
        public bool Failure => !Success;

        public static Result Ok() => new Result(true);
        public static Result Fail(string error) => new Result(false, error);
    }
}

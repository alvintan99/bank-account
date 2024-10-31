namespace AwesomeGICBank.ConsoleApp.Services
{
    public class ConsoleService
    {
        public virtual void Write(string message)
        {
            System.Console.Write(message);
        }

        public virtual void WriteLine(string message)
        {
            System.Console.WriteLine(message);
        }

        public virtual void WriteLine()
        {
            System.Console.WriteLine();
        }

        public virtual string? ReadLine()
        {
            return System.Console.ReadLine();
        }
    }
}

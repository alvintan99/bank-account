namespace AwesomeGICBank.ConsoleApp.UI
{
    public class ConsoleUI
    {
        private readonly MenuHandler _menuHandler;

        public ConsoleUI(MenuHandler menuHandler)
        {
            _menuHandler = menuHandler;
        }

        public async Task RunAsync()
        {
            Console.WriteLine("Welcome to AwesomeGIC Bank! What would you like to do?");

            bool running = true;
            while (running)
            {
                _menuHandler.DisplayMainMenu();
                var choice = Console.ReadLine()?.Trim().ToUpper();

                switch (choice)
                {
                    case "T":
                        await _menuHandler.HandleTransactionInputAsync();
                        break;
                    case "I":
                        await _menuHandler.HandleInterestRuleInputAsync();
                        break;
                    case "P":
                        await _menuHandler.HandlePrintStatementAsync();
                        break;
                    case "Q":
                        running = false;
                        Console.WriteLine("Thank you for banking with AwesomeGIC Bank.");
                        Console.WriteLine("Have a nice day!");
                        break;
                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
            }
        }

        //public async Task HandleTransactionInput()
        //{
        //    Console.WriteLine("Please enter transaction details in <Date> <Account> <Type> <Amount> format");
        //    Console.WriteLine("(or enter blank to go back to main menu):");
        //    Console.Write(">");

        //    var input = Console.ReadLine()?.Trim();
        //    if (string.IsNullOrEmpty(input))
        //        return;

        //    var result = await _menuHandler.ProcessTransactionAsync(input);

        //    if (result.Success)
        //    {
        //        System.Console.WriteLine("Transaction processed successfully.");
        //    }
        //    else
        //    {
        //        System.Console.WriteLine($"Error: {result.Error}");
        //        _logger.LogWarning("Failed to process transaction: {Error}", result.Error);
        //    }
        //}
    }
}

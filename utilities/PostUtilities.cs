namespace BlogApp.Utilities
{
    public static class GetValidInputs
    {
        public static int GetValidInt(string prompt)
        {
            int input;
            while (true)
            {
                Console.WriteLine(prompt);
                bool success = int.TryParse(Console.ReadLine(), out input);
                if (success)
                    return input;

                Console.Clear();
                Console.WriteLine("Invalid input. Please try again.");
            }
        }

        public static string GetValidString(string prompt)
        {
            string input;
            while (true)
            {
                Console.WriteLine(prompt);
                input = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(input))
                    return input;

                Console.Clear();
                Console.WriteLine("Invalid input. Please try again.");
            }
        }

        public static (string username, string password) GetCredentials()
        {
            string username = GetValidString("What is your postgres username?");
            string password = GetValidString("What is your postgres password?");
            return (username, password);
        }
    }
}
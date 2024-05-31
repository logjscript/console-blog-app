namespace BlogApp.Utilities;
public static class GetValidInputs
{
    public static int GetValidInt(string prompt)
    {
        while (true)
        {
            Console.WriteLine(prompt);
            bool success = int.TryParse(Console.ReadLine(), out int input);
            if (success)
                return input;

            Console.Clear();
            Console.WriteLine("Invalid input. Please try again.");
        }
    }

    public static string GetValidString(string prompt)
    {
        string? input;
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
}
namespace Controllers.SelectionMenu;
public class SelectionMenu
{
    private static Dictionary<int, string> possibleSelections = new Dictionary<int, string> { 
        {1, "create"}, {2, "read"}, {3, "update"}, {4, "delete"} 
    };
    public int SelectActionType()
    {
        string displayedSelections = "\n\n 1. Create a post\n\n 2. Read a post\n\n 3. Update a post\n\n 4. Delete a post";
        Console.WriteLine($"Hello. Please select what you would like to do. {displayedSelections}");
        
        int selection;

        while (true)
        {
            if (int.TryParse(Console.ReadLine(), out selection) && possibleSelections.ContainsKey(selection))
                    break;

            Console.Clear();
            Console.WriteLine($"Please make a valid selection. {displayedSelections}");       
        }

        Console.Clear();
        return selection;
    }
}

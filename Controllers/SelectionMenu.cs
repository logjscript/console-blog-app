namespace Controllers.SelectionMenu;
public class SelectionMenu
{
    private static Dictionary<int, string> possibleSelections = new Dictionary<int, string> { 
        {1, "create"}, {2, "read"}, {3, "update"}, {4, "delete"} 
    };
    public int SelectActionType()
    {
        string displayedSelections = "\n 1. Create a post\n 2. Read a post\n 3. Update a post\n 4. Delete a post";
        Console.WriteLine($"Hello. Please select what you would like to do. {displayedSelections}");
        
        int selection;
        while (true)
        {
            try
            {
                 selection = int.Parse(Console.ReadLine());
                 if (possibleSelections.ContainsKey(selection))
                     break;
            } catch {}
            Console.Clear();
            Console.WriteLine($"Please choose a valid selection. {displayedSelections}");
        }
        return selection;
    }
}

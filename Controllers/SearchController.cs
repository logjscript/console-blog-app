using BlogApp.PostRepository;
using BlogApp.Utilities;

namespace Controllers.SearchPosts;

public class SearchPosts
{
    private static Dictionary<int, string> possibleSelections = new Dictionary<int, string> { 
        {1, "title"}, {2, "author"}, {3, "date"}
    };

    public int SelectSearchType()
    {
        return InputValidator.GetValidInt("Please select what you would like to search by. \n\n1. Title\n\n2. Author\n\n3. Date");
    }

    private T GetSearchInput <T>(string searchType)
    {
    
        string prompt = searchType == "date" 
            ? $"Search for {searchType} (MM/DD/YYYY):" 
            : $"Search for {searchType}:";

        Console.Clear();
        string result = InputValidator.GetValidString(prompt);

        if (typeof(T) == typeof(string))
            return (T)(object)result;  
        else if (typeof(T) == typeof(DateTime))
            if (DateTime.TryParse(result, out DateTime parsedDate))
                return (T)(object)parsedDate;
            else 
            {
                Console.WriteLine("Date format is incorrect. Please try again.");
                return GetSearchInput<T>(searchType);
            }
        else
        {
            throw new InvalidOperationException("This search type is not supported.");
        }
    }
    
    public void SearchPostsByType()
    {  
        var postRepository = new PostRepo();

        try
        {
            string search = possibleSelections[SelectSearchType()];

            if (search == "title" || search == "author") 
            {
                string result = GetSearchInput<string>(search);
                postRepository.SearchByUserTypeSelection(search, result);
            }
            else if (search == "date")
            {
                DateTime result = GetSearchInput<DateTime>(search);
                Console.WriteLine("result");
                postRepository.SearchByUserTypeSelection(search, result);
            }
            else
            {
                throw new Exception();
            }
        } catch
        {
            Console.Clear();
            Console.Write("Invalid selection. ");
            SearchPostsByType();
        }      
    }
}

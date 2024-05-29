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
        Console.Clear();     
        int selection = GetValidInputs.GetValidInt("Please select what you would like to search by. \n1. Title\n2. Author\n3. Date");
        return selection;
    }

    private T SearchInput <T>(string searchType)
    {
    
        string prompt = searchType == "date" 
            ? $"Search for {searchType} (MM/DD/YYYY):" 
            : $"Search for {searchType}:";

        Console.Clear();
        string result = GetValidInputs.GetValidString(prompt);

        if (typeof(T) == typeof(string) && searchType == "title" || searchType == "author")
            return (T)(object)result;  
        else
            return (T)(object)DateTime.Parse(result);
    }
    
    public void SearchPostsByType()
    {  
        var postRepository = new PostRepo();
        string search = possibleSelections[SelectSearchType()];

        if (search == "title") 
        {
            string result = SearchInput<string>(search);
            postRepository.SearchByUserTypeSelection(search, result);
        }
        else if (search == "author")
        {
            string result = SearchInput<string>(search);
            postRepository.SearchByUserTypeSelection(search, result);
        }
        else
        {
            DateTime result = SearchInput<DateTime>(search);
            Console.WriteLine("result");
            postRepository.SearchByUserTypeSelection(search, result);
        }
    }
}

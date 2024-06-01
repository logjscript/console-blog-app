using Controllers.SearchPosts;
using Controllers.SelectionMenu;
using BlogApp.Controllers;
using BlogApp.InitializeDatabase;

namespace BlogApp;
class Program
{
    static void Main(string[] args)
    {
        Initialize.InitializeDatabase();
        Initialize.CreateTable();

        while (true)
        {
            Console.Clear();
            var selectionMenu = new SelectionMenu();
            int selection = selectionMenu.SelectActionType();
            var postController = new PostController();
        
            if (selection == 1) 
            {
                postController.CreatePost();
            }
            else
            {
                var searchPosts = new SearchPosts();
                searchPosts.SearchPostsByType();
                
                if (selection == 2)
                {
                    postController.ReadPost();
                }
                else if (selection == 3)
                {
                    postController.UpdatePost();
                }
                else
                {
                    postController.DeletePost();
                }
            }
        }   
    }
}



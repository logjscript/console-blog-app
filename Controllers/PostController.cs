using BlogApp.PostRepository;
using BlogApp.Models;
using BlogApp.Utilities;

namespace BlogApp.Controllers;
public class PostController
{
    private PostRepo _postRepository = new PostRepo();

    public void CreatePost()
    {
        string title = InputValidator.GetValidString("What is the title of this post?");
        string author = InputValidator.GetValidString("What is your name?");
        string content = InputValidator.GetValidString("Share your content here:");
        int id = PostRepo.GetLargestId() + 1;

        var blogPost = new Post(id, title, author, content);
        _postRepository.CreatePost(blogPost.Id, blogPost.Title, blogPost.Author, blogPost.Content, blogPost.Date);
    }

    public void ReadPost()
    {
        int id = InputValidator.GetValidInt("Enter the ID of the post you would like to read.");
        _postRepository.ReadPost(id);
    }

    public void UpdatePost()
    {
        int id = PostRepo.CheckIdForExistingPost("update");
        Console.Clear();
        string content = InputValidator.GetValidString("Write your new content here:");
        PostRepo.UpdatePost(content, id);
    }

    public void DeletePost()
    {
        int id = PostRepo.CheckIdForExistingPost("delete");
        _postRepository.DeletePost(id);
    }
}


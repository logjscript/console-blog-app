using BlogApp.Repositories;
using BlogApp.Models;
using BlogApp.Utilities;

namespace BlogApp.Controllers;
public class PostController
{
    private PostRepository _postRepository = new PostRepository();

    public void CreatePost()
    {
        string title = GetValidInputs.GetValidString("What is the title of this post?");
        string author = GetValidInputs.GetValidString("What is your name?");
        string content = GetValidInputs.GetValidString("Share your content here:");
        int id = PostRepository.GetLargestId() + 1;

        var blogPost = new Post(id, title, author, content);
        _postRepository.CreatePost(blogPost.Id, blogPost.Title, blogPost.Author, blogPost.Content, blogPost.Date);
    }

    public void ReadPost()
    {
        int id = GetValidInputs.GetValidInt("Enter the ID of the post you would like to read.");
        _postRepository.GetPostById(id);
    }

    public void UpdatePost()
    {
        int id = PostRepository.GetIdForExistingPost("update");
        Console.Clear();
        string content = GetValidInputs.GetValidString("Write your new content here:");
        PostRepository.UpdatePost(content, id);
    }

    public void DeletePost()
    {
        int id = PostRepository.GetIdForExistingPost("delete");
        _postRepository.DeletePost(id);
    }
}


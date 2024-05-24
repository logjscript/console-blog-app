namespace BlogApp.Models;

public class Post
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public string Content { get; set; }
    public DateTime Date { get; private set; }

    public Post(int id, string title, string author, string content)
    {
        Id = id;
        Title = title;
        Author = author;
        Content = content;
        Date = DateTime.Now;
    }
}




using Npgsql;
using BlogApp.Utilities;

namespace BlogApp.PostRepository;
public class PostRepo
{
    // Add connection details here for postgres
    private static string connectionString = "Host=localhost;Username=________;Password=________;";

    public static string ConnectionString
    {
        get { return connectionString; }
        set { connectionString = value; }
    }

    public static int GetLargestId()
    { 
        int? result = null;
        var connection = new NpgsqlConnection(connectionString);
    
        try
        {   
            using (connection)
            {
                connection.Open();

                string commandText = $"SELECT MAX(id) AS max_id FROM posts;";
                var command = new NpgsqlCommand(commandText, connection);
                result = (int?)command.ExecuteScalar();
            }
        } catch {}

        if (result.HasValue)
            return (int)result;
        else 
            return 0;        
    }

    static public int CheckIdForExistingPost(string actionType)
    {
        int id;
        while (true)
        {
            id = InputValidator.GetValidInt($"Enter the ID of the post you would like to {actionType}.");
            try
            {   
                var connection = new NpgsqlConnection(connectionString);
                using (connection)
                {
                    connection.Open();

                    string query = $"SELECT * FROM posts WHERE id = @id;";
                    var command = new NpgsqlCommand(query, connection);    

                    var results = command.ExecuteScalar();
                    if (results != null)
                        break;
                }
            } catch {}
            Console.WriteLine("ID doesnt exist. Please try again.");
        }
        return id;
    }
   

    public void SearchByUserTypeSelection<T>(string column, T value)
    {
        Console.Clear();
        try
        {   
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                string query;
                if (column == "date")
                    query = $"SELECT * FROM posts WHERE {column}::date = @value;";
                else 
                    query = $"SELECT * FROM posts WHERE {column}::text ILIKE '%' || @value || '%';";

                var command = new NpgsqlCommand(query, connection);
                command.Parameters.AddWithValue("@column", column);
                if (value != null)
                    command.Parameters.AddWithValue("@value", value);
                else 
                {
                    Console.WriteLine("No value was given to the search.");
                    return;
                }

                using (var reader = command.ExecuteReader())
                {
                    while(reader.Read())
                    {
                        int idColumn = reader.GetInt32(0);
                        string titleColumn = reader.GetString(1);
                        string authorColumn = reader.GetString(2);
                        DateTime dateColumn = reader.GetDateTime(4);

                        Console.WriteLine($"ID: {idColumn}\t Title: {titleColumn}\t Author: {authorColumn}\t Date: {dateColumn}\n");
                    }
                    Console.WriteLine("END\n");
                }
            }
        } catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
    }

    
    public void CreatePost(int id, string title, string author, string content, DateTime date)
    {
        try
        {
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                string commandText = "INSERT INTO posts (id, title, author, content, date) VALUES (@id, @title, @author, @content, @date)";
                var command = new NpgsqlCommand(commandText, connection);

                command.Parameters.AddWithValue("@id", id);
                command.Parameters.AddWithValue("@title", title);
                command.Parameters.AddWithValue("@author", author);
                command.Parameters.AddWithValue("@content", content);
                command.Parameters.AddWithValue("@date", date);

                command.ExecuteNonQuery();

                Console.Clear();
                Console.WriteLine($"Post {id} has been created. \nPress 'enter' to continue.");
                Console.ReadLine();
            }
        } catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
    }

    public void ReadPost(int id)
    {
        try
        {
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                string query = $"SELECT * FROM posts WHERE id = @id;";
                var command = new NpgsqlCommand(query, connection);
                command.Parameters.AddWithValue("@id", id);

                using (var reader = command.ExecuteReader())
                {
                    reader.Read();
                    
                    int idColumn = reader.GetInt32(0);
                    string titleColumn = reader.GetString(1);
                    string authorColumn = reader.GetString(2);
                    string contentColumn = reader.GetString(3);
                    DateTime dateColumn = reader.GetDateTime(4);
                    
                    Console.Clear();
                    Console.WriteLine($"{titleColumn}\n By {authorColumn}\n {dateColumn}\n\n {contentColumn} \n\n");
                    
                }  
            }
        } catch
        {
            Console.Clear();
            Console.WriteLine($"No post with ID {id} was found.");
        }

        while (true)
        {
            Console.WriteLine("Type 'exit' to return to menu");
            string? exitString = Console.ReadLine();
            if (exitString != null && exitString.ToLower() == "exit")
                break;
        }
    }

    static public void UpdatePost(string content, int id)
    {
        try
        {   
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                string commandText = "UPDATE posts SET content = @content, date = @date WHERE id = @id;";
                
                var command = new NpgsqlCommand(commandText, connection);
                command.Parameters.AddWithValue("@content", content);
                command.Parameters.AddWithValue("@date", DateTime.Now);
                command.Parameters.AddWithValue("@id", id);

                command.ExecuteNonQuery();

                Console.Clear();
                Console.WriteLine($"Post {id} has been updated.\n");
            }
        } catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
    
        Console.WriteLine("Press 'enter' to continue\n");
        Console.ReadLine();
    }

     public void DeletePost(int id)
    {
        try
        {   
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                string commandText = $"DELETE FROM posts WHERE id = @id;";
                var command = new NpgsqlCommand(commandText, connection);
                command.Parameters.AddWithValue("@id", id);
                command.ExecuteNonQuery();    

                Console.Clear();
                Console.WriteLine($"Post {id} has been deleted.");
            }
        } catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
        Console.WriteLine("Press 'enter' to continue\n");
        Console.ReadLine();
    }
}
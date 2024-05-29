using Npgsql;
using BlogApp.Utilities;

namespace BlogApp.Repositories;
public class PostRepository
{
    // Add connection details here for postgres
    private static string connectionString = "Host=localhost;Username=_______;Password=_______;";

    public static void InitializeDatabase()
    {
        using (var connection = new NpgsqlConnection(connectionString))
        {
            connection.Open();
           
            string checkDatabaseQuery = "SELECT 1 FROM pg_database WHERE datname = 'blog_app';";
            var checkCommand = new NpgsqlCommand(checkDatabaseQuery, connection);
            var reader = checkCommand.ExecuteReader();

            if (!reader.HasRows)
            {
                reader.Close();

                var createDatabaseText = "CREATE DATABASE blog_app";
                var databaseCommand = new NpgsqlCommand(createDatabaseText, connection);
                databaseCommand.ExecuteNonQuery();
            }
            connectionString = $"Host=localhost;Username=logandietel;Password=password;Database=blog_app;";

            reader.Close();
        }
    } 
    public static void CreateTable()
    {
        using (var connection = new NpgsqlConnection(connectionString))
        {
            connection.Open();
            string createTableQuery = "CREATE TABLE IF NOT EXISTS posts (id SERIAL PRIMARY KEY, title VARCHAR(100), author VARCHAR(50), content TEXT, date TIMESTAMP);";
            var createTableCommand = new NpgsqlCommand(createTableQuery, connection);
            createTableCommand.ExecuteNonQuery();
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
                command.Parameters.AddWithValue("@value", value);

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

    public void GetPostById(int id)
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
            string exitString = Console.ReadLine();
            if (exitString.ToLower() == "exit")
                break;
        }
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

    static public int GetIdForExistingPost(string actionType)
    {
        int id;
        while (true)
        {
            id = GetValidInputs.GetValidInt($"Enter the ID of the post you would like to {actionType}.");
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
}
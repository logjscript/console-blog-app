using Npgsql;
using BlogApp.PostRepository;
using BlogApp.Utilities;

namespace BlogApp.DatabaseRepository;

public class DatabaseRepo
{
    public static void InitializeDatabase()
    {
        using (var connection = new NpgsqlConnection(PostRepo.ConnectionString))
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
            PostRepo.ConnectionString = $"Host=localhost;Username=logandietel;Password=password;Database=blog_app;";

            reader.Close();
        }
    } 
    public static void CreateTable()
    {
        using (var connection = new NpgsqlConnection(PostRepo.ConnectionString))
        {
            connection.Open();
            string createTableQuery = "CREATE TABLE IF NOT EXISTS posts (id SERIAL PRIMARY KEY, title VARCHAR(100), author VARCHAR(50), content TEXT, date TIMESTAMP);";
            var createTableCommand = new NpgsqlCommand(createTableQuery, connection);
            createTableCommand.ExecuteNonQuery();
        }
    } 
}
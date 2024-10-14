using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//dotnet add package Microsoft.Data.Sqlite

namespace MyBlazoMauiApp.Data
{
    public class DatabaseHelper
{
    private readonly string dbPath;

    public DatabaseHelper(string dbPath)
    {
        this.dbPath = dbPath;
        InitializeDatabase();
    }

    private void InitializeDatabase()
    {
        using (var connection = new SqliteConnection($"Data Source={dbPath}"))
        {
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = @"
                CREATE TABLE IF NOT EXISTS MyTable (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Name TEXT NOT NULL
                );";
            command.ExecuteNonQuery();
        }
    }

    public void InsertRecord(string name)
    {
        using (var connection = new SqliteConnection($"Data Source={dbPath}"))
        {
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = @"INSERT INTO MyTable (Name) VALUES ($name)";
            command.Parameters.AddWithValue("$name", name);
            command.ExecuteNonQuery();
        }
    }

    public void UpdateRecord(int id, string name)
    {
        using (var connection = new SqliteConnection($"Data Source={dbPath}"))
        {
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = @"UPDATE MyTable SET Name = $name WHERE Id = $id";
            command.Parameters.AddWithValue("$name", name);
            command.Parameters.AddWithValue("$id", id);
            command.ExecuteNonQuery();
        }
    }

    public void DeleteRecord(int id)
    {
        using (var connection = new SqliteConnection($"Data Source={dbPath}"))
        {
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = @"DELETE FROM MyTable WHERE Id = $id";
            command.Parameters.AddWithValue("$id", id);
            command.ExecuteNonQuery();
        }
    }

    public List<string> GetAllRecords()
    {
        var records = new List<string>();
        using (var connection = new SqliteConnection($"Data Source={dbPath}"))
        {
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = @"SELECT * FROM MyTable";
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    records.Add(reader.GetString(1)); // Assuming the second column is Name
                }
            }
        }
        return records;
    }
}

}

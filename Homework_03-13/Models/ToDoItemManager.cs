using Microsoft.Extensions.Configuration.EnvironmentVariables;
using System.Data.SqlClient;
using System.Xml.Linq;

namespace Homework_03_13.Models
{
    public class ToDoItemManager
    {
        private string _connectionString;

        public ToDoItemManager(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Category> GetCategories()
        {
            var connection = new SqlConnection(_connectionString);
            var command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM Categories";
            connection.Open();
            var reader = command.ExecuteReader();

            List<Category> categories = new();
            while (reader.Read())
            {
                categories.Add(new Category
                {
                    Id = (int)reader["CategoryId"],
                    Name = (string)reader["CategoryName"]
                });
            }
            connection.Close();
            return categories;
        }

        public void UpdateCategory(Category c)
        {
            var connection = new SqlConnection(_connectionString);
            var command = connection.CreateCommand();
            command.CommandText = "UPDATE Categories SET CategoryName = @name WHERE CategoryId = @id";
            command.Parameters.AddWithValue("@name", c.Name);
            command.Parameters.AddWithValue("@id", c.Id);
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
        }


        public Category GetCategoryById(int id)
        {
            var connection = new SqlConnection(_connectionString);
            var command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM Categories WHERE CategoryId = @id";
            command.Parameters.AddWithValue("@id", id);
            connection.Open();
            var reader = command.ExecuteReader();

            if (!reader.Read())
            {
                return null;
            }
            
            Category c = new Category
            {
                Id = (int)reader["CategoryId"],
                Name = (string)reader["CategoryName"]
            };

            connection.Close();
            return c;
        }

        public void AddCategory(Category c)
        {
            var connection = new SqlConnection(_connectionString);
            var command = connection.CreateCommand();
            command.CommandText = "INSERT INTO Categories(CategoryName) " +
                "VALUES(@name)";
            command.Parameters.AddWithValue("@name", c.Name);
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
        }

        public void AddToDoItem(ToDoItem item)
        {
            var connection = new SqlConnection(_connectionString);
            var command = connection.CreateCommand();
            command.CommandText = "INSERT INTO ToDoItems(Title, DueDate, CategoryId) " +
                "VALUES(@title, @dateDue, @catId)";
            command.Parameters.AddWithValue("@title", item.Title);
            command.Parameters.AddWithValue("@dateDue", item.DueDate);
            command.Parameters.AddWithValue("@catId", item.CategoryId);
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
        }

        public List<ToDoItem> GetAllNoncompletedItems()
        {
            var connection = new SqlConnection(_connectionString);
            var command = connection.CreateCommand();
            command.CommandText = "SELECT td.*, c.CategoryName FROM ToDoItems td " +
                "JOIN Categories c " +
                "On td.CategoryId = c.CategoryId " +
                "WHERE td.CompletedDate IS NULL";
            connection.Open();

            var items = new List<ToDoItem>();
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                items.Add(new ToDoItem
                {
                    Id = (int)reader["ItemId"],
                    Title = (string)reader["Title"],
                    DueDate = (DateTime)reader["DueDate"],
                    CategoryId = (int)reader["CategoryId"],
                    CompletedDate = reader.GetOrNull<DateTime?>("CompletedDate"),
                    CategoryName = (string)reader["CategoryName"]
                });

            }
            connection.Close();
            return items;
        }

        public List<ToDoItem> GetItemsForCategory(int id)
        {
            var connection = new SqlConnection(_connectionString);
            var command = connection.CreateCommand();
            command.CommandText = "SELECT td.*, c.CategoryName FROM ToDoItems td " +
                "JOIN Categories c " +
                "ON c.CategoryId = td.CategoryId " +
                "WHERE td.CategoryId = @id";
            command.Parameters.AddWithValue("@id", id);
            connection.Open();

            var items = new List<ToDoItem>();
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                items.Add(new ToDoItem
                {
                    Id = (int)reader["ItemId"],
                    Title = (string)reader["Title"],
                    DueDate = (DateTime)reader["DueDate"],
                    CategoryId = (int)reader["CategoryId"],
                    CompletedDate = reader.GetOrNull<DateTime?>("CompletedDate"),
                    CategoryName = (string)reader["CategoryName"]
                });

            }
            connection.Close();
            return items;
        }


        public List<ToDoItem> GetAllCompletedItems()
        {
            var connection = new SqlConnection(_connectionString);
            var command = connection.CreateCommand();
            command.CommandText = "SELECT td.*, c.CategoryName FROM ToDoItems td " +
                "JOIN Categories c " +
                "On td.CategoryId = c.CategoryId " +
                "WHERE td.CompletedDate IS NOT NULL";
            connection.Open();

            var items = new List<ToDoItem>();
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                items.Add(new ToDoItem
                {
                    Id = (int)reader["ItemId"],
                    Title = (string)reader["Title"],
                    DueDate = (DateTime)reader["DueDate"],
                    CategoryId = (int)reader["CategoryId"],
                    CompletedDate = reader.GetOrNull<DateTime?>("CompletedDate"),
                    CategoryName = (string)reader["CategoryName"]
                });
            }

            connection.Close();
            return items;
        }

        public void MarkAsComplete(int id)
        {
            var connection = new SqlConnection(_connectionString);
            var command = connection.CreateCommand();
            command.CommandText = "UPDATE ToDoItems SET CompletedDate = @date Where ItemId = @id";
            command.Parameters.AddWithValue("@date", DateTime.Now);
            command.Parameters.AddWithValue("@id", id);
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
        }
    }
}


using Microsoft.Data.SqlClient;
using RaceDayAPI.Data;
using RaceDayAPI.DTOs;
using RaceDayAPI.Models;

namespace RaceDayAPI.Services
{
    public class CategoryService
    {
        private readonly DatabaseConnection _database;

        public CategoryService(DatabaseConnection database)
        {
            _database = database;
        }

        public List<Category> GetCategories(int eventID)
        {
            List<Category> categories = new List<Category>();

            using SqlConnection connection = _database.GetConnection();
            connection.Open();

            string query = @"
                SELECT CategoryID, EventID, CategoryName,
                       CategoryType, CategoryValue
                FROM Categories
                WHERE EventID = @EventID";

            using SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@EventID", eventID);

            using SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                categories.Add(new Category
                {
                    CategoryID = (int)reader["CategoryID"],
                    EventID = (int)reader["EventID"],
                    CategoryName = reader["CategoryName"].ToString()!,
                    CategoryType = reader["CategoryType"].ToString()!,
                    CategoryValue = reader["CategoryValue"].ToString()!
                });
            }

            return categories;
        }

        public int CreateCategory(CategoryRequest request)
        {
            using SqlConnection connection = _database.GetConnection();
            connection.Open();

            string query = @"
                INSERT INTO Categories
                (EventID, CategoryName, CategoryType, CategoryValue)
                VALUES
                (@EventID, @CategoryName, @CategoryType, @CategoryValue);

                SELECT CAST(SCOPE_IDENTITY() AS INT);";

            using SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@EventID", request.EventID);
            command.Parameters.AddWithValue(
                "@CategoryName", request.CategoryName);
            command.Parameters.AddWithValue(
                "@CategoryType", request.CategoryType);
            command.Parameters.AddWithValue(
                "@CategoryValue", request.CategoryValue);

            return (int)command.ExecuteScalar()!;
        }
    }
}

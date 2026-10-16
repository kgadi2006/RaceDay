
using Microsoft.Data.SqlClient;
using RaceDayAPI.Data;
using RaceDayAPI.DTOs;

namespace RaceDayAPI.Services
{//start of namespace
    public class ProfileService
    {//start of class
        private readonly DatabaseConnection _database;

        public ProfileService(DatabaseConnection database)
        {
            _database = database;
        }

        public object? GetProfile(int userID)
        {
            using SqlConnection connection = _database.GetConnection();
            connection.Open();

            string query = @"
                SELECT UserID, FullName, Email, Phone, Role
                FROM Users
                WHERE UserID = @UserID";

            using SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@UserID", userID);

            using SqlDataReader reader = command.ExecuteReader();

            if (!reader.Read())
            {
                return null;
            }

            return new
            {
                UserID = (int)reader["UserID"],
                FullName = reader["FullName"].ToString(),
                Email = reader["Email"].ToString(),
                Phone = reader["Phone"] == DBNull.Value
                    ? null
                    : reader["Phone"].ToString(),
                Role = reader["Role"].ToString()
            };
        }

        public bool UpdateProfile(
            int userID,
            UpdateProfileRequest request)
        {
            using SqlConnection connection = _database.GetConnection();
            connection.Open();

            string query = @"
                UPDATE Users
                SET FullName = @FullName,
                    Phone = @Phone
                WHERE UserID = @UserID";

            using SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@UserID", userID);
            command.Parameters.AddWithValue("@FullName", request.FullName);
            command.Parameters.AddWithValue(
                "@Phone",
                (object?)request.Phone ?? DBNull.Value);

            return command.ExecuteNonQuery() > 0;
        }
    }//end of class
}//end of namespace

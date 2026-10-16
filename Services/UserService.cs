using Microsoft.Data.SqlClient;
using RaceDayAPI.Data;
using RaceDayAPI.DTOs;

namespace RaceDayAPI.Services
{
    public class UserService
    {
        private readonly DatabaseConnection _database;
        private readonly PasswordService _passwordService;

        public UserService(DatabaseConnection database, PasswordService passwordService)
        {
            _database = database;
            _passwordService = passwordService;
        }

        public virtual string Register(RegisterRequest request)
        {
            using SqlConnection connection =
                _database.GetConnection();

            connection.Open();

            string checkQuery =
                "SELECT COUNT(*) FROM Userss WHERE Email = @Email";

            using SqlCommand checkCommand =
                new SqlCommand(checkQuery, connection);

            checkCommand.Parameters.AddWithValue(
                "@Email", request.Email);

            int count = (int)checkCommand.ExecuteScalar()!;

            if (count > 0)
            {
                return "Email already exists";
            }

            string hashedPassword =
                _passwordService.HashPassword(request.Password);

            string insertQuery = @"
                INSERT INTO Userss
                (FullName, Email, Password, Phone, Role)
                VALUES
                (@FullName, @Email, @Password, @Phone, @Role)";

            using SqlCommand insertCommand =
                new SqlCommand(insertQuery, connection);

            insertCommand.Parameters.AddWithValue(
                "@FullName", request.FullName);

            insertCommand.Parameters.AddWithValue(
                "@Email", request.Email);

            insertCommand.Parameters.AddWithValue(
                "@Password", hashedPassword);

            insertCommand.Parameters.AddWithValue(
                "@Phone", (object?)request.Phone ?? DBNull.Value);

            insertCommand.Parameters.AddWithValue(
                "@Role", request.Role);

            insertCommand.ExecuteNonQuery();

            return "Registration successful";
        }

        public virtual object? Login(LoginRequest request)
        {
            using SqlConnection connection =
                _database.GetConnection();

            connection.Open();

            string query = @"
                SELECT UserID, FullName, Email, Password, Role
                FROM Userss
                WHERE Email = @Email";

            using SqlCommand command =
                new SqlCommand(query, connection);

            command.Parameters.AddWithValue(
                "@Email", request.Email);

            using SqlDataReader reader =
                command.ExecuteReader();

            if (!reader.Read())
            {
                return null;
            }

            string hashedPassword =
                reader["Password"].ToString()!;

            bool passwordIsValid =
                _passwordService.VerifyPassword(
                    hashedPassword,
                    request.Password);

            if (!passwordIsValid)
            {
                return null;
            }

            return new
            {
                UserID = (int)reader["UserID"],
                FullName = reader["FullName"].ToString(),
                Email = reader["Email"].ToString(),
                Role = reader["Role"].ToString()
            };
        }
    }
}
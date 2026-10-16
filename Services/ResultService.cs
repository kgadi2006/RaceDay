
using Microsoft.Data.SqlClient;
using RaceDayAPI.Data;
using RaceDayAPI.DTOs;
using RaceDayAPI.Models;

namespace RaceDayAPI.Services
{
    public class ResultService
    {
        private readonly DatabaseConnection _database;

        public ResultService(DatabaseConnection database)
        {
            _database = database;
        }

        public List<Result> GetEventResults(int eventID)
        {
            List<Result> results = new List<Result>();

            using SqlConnection connection = _database.GetConnection();
            connection.Open();

            string query = @"
                SELECT r.ResultID, r.EnrolmentID,
                       r.FinishTime, r.FinishingPosition
                FROM Results r
                INNER JOIN EventEnrolments e
                    ON r.EnrolmentID = e.EnrolmentID
                WHERE e.EventID = @EventID
                ORDER BY r.FinishingPosition";

            using SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@EventID", eventID);

            using SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                results.Add(new Result
                {
                    ResultID = (int)reader["ResultID"],
                    EnrolmentID = (int)reader["EnrolmentID"],
                    FinishTime = (TimeSpan)reader["FinishTime"],
                    FinishingPosition = (int)reader["FinishingPosition"]
                });
            }

            return results;
        }

        public List<Result> GetMyResults(int participantID)
        {
            List<Result> results = new List<Result>();

            using SqlConnection connection = _database.GetConnection();
            connection.Open();

            string query = @"
                SELECT r.ResultID, r.EnrolmentID,
                       r.FinishTime, r.FinishingPosition
                FROM Results r
                INNER JOIN EventEnrolments e
                    ON r.EnrolmentID = e.EnrolmentID
                WHERE e.ParticipantID = @ParticipantID
                ORDER BY r.FinishingPosition";

            using SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@ParticipantID", participantID);

            using SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                results.Add(new Result
                {
                    ResultID = (int)reader["ResultID"],
                    EnrolmentID = (int)reader["EnrolmentID"],
                    FinishTime = (TimeSpan)reader["FinishTime"],
                    FinishingPosition = (int)reader["FinishingPosition"]
                });
            }

            return results;
        }

        public int CreateResult(ResultRequest request)
        {
            using SqlConnection connection = _database.GetConnection();
            connection.Open();

            string query = @"
                INSERT INTO Results
                (EnrolmentID, FinishTime, FinishingPosition)
                VALUES
                (@EnrolmentID, @FinishTime, @FinishingPosition);

                SELECT CAST(SCOPE_IDENTITY() AS INT);";

            using SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue(
                "@EnrolmentID", request.EnrolmentID);
            command.Parameters.AddWithValue(
                "@FinishTime", request.FinishTime);
            command.Parameters.AddWithValue(
                "@FinishingPosition", request.FinishingPosition);

            return (int)command.ExecuteScalar()!;
        }

        public bool UpdateResult(int resultID, ResultRequest request)
        {
            using SqlConnection connection = _database.GetConnection();
            connection.Open();

            string query = @"
                UPDATE Results
                SET FinishTime = @FinishTime,
                    FinishingPosition = @FinishingPosition
                WHERE ResultID = @ResultID
                AND EnrolmentID = @EnrolmentID";

            using SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@ResultID", resultID);
            command.Parameters.AddWithValue(
                "@EnrolmentID", request.EnrolmentID);
            command.Parameters.AddWithValue(
                "@FinishTime", request.FinishTime);
            command.Parameters.AddWithValue(
                "@FinishingPosition", request.FinishingPosition);

            return command.ExecuteNonQuery() > 0;
        }
    }
}

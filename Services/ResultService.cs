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

        // Get results for a specific event
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

        // Get results belonging to a specific Participant
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

        // Create a result only if the Organizer owns the event
        public int CreateResult(
            int eventID,
            int organizerID,
            ResultRequest request)
        {
            using SqlConnection connection = _database.GetConnection();
            connection.Open();

            string query = @"
                INSERT INTO Results
                (EnrolmentID, FinishTime, FinishingPosition)

                SELECT
                    ee.EnrolmentID,
                    @FinishTime,
                    @FinishingPosition

                FROM EventEnrolments ee

                INNER JOIN Events e
                    ON ee.EventID = e.EventID

                WHERE ee.EnrolmentID = @EnrolmentID
                AND ee.EventID = @EventID
                AND e.OrganizerID = @OrganizerID;

                SELECT CAST(SCOPE_IDENTITY() AS INT);";

            using SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue(
                "@EnrolmentID", request.EnrolmentID);

            command.Parameters.AddWithValue(
                "@EventID", eventID);

            command.Parameters.AddWithValue(
                "@OrganizerID", organizerID);

            command.Parameters.AddWithValue(
                "@FinishTime", request.FinishTime);

            command.Parameters.AddWithValue(
                "@FinishingPosition", request.FinishingPosition);

            object? result = command.ExecuteScalar();

            if (result == null || result == DBNull.Value)
            {
                return 0;
            }

            return Convert.ToInt32(result);
        }

        // Update a result only if the Organizer owns its event
        public bool UpdateResult(
            int resultID,
            int organizerID,
            ResultRequest request)
        {
            using SqlConnection connection = _database.GetConnection();
            connection.Open();

            string query = @"
                UPDATE r
                SET r.FinishTime = @FinishTime,
                    r.FinishingPosition = @FinishingPosition

                FROM Results r

                INNER JOIN EventEnrolments ee
                    ON r.EnrolmentID = ee.EnrolmentID

                INNER JOIN Events e
                    ON ee.EventID = e.EventID

                WHERE r.ResultID = @ResultID
                AND r.EnrolmentID = @EnrolmentID
                AND e.OrganizerID = @OrganizerID";

            using SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@ResultID", resultID);

            command.Parameters.AddWithValue(
                "@EnrolmentID", request.EnrolmentID);

            command.Parameters.AddWithValue(
                "@OrganizerID", organizerID);

            command.Parameters.AddWithValue(
                "@FinishTime", request.FinishTime);

            command.Parameters.AddWithValue(
                "@FinishingPosition", request.FinishingPosition);

            return command.ExecuteNonQuery() > 0;
        }
    }
}
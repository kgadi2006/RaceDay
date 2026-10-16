
using Microsoft.Data.SqlClient;
using RaceDayAPI.Data;
using RaceDayAPI.DTOs;
using RaceDayAPI.Models;

namespace RaceDayAPI.Services
{
    public class EnrolmentService
    {
        private readonly DatabaseConnection _database;

        public EnrolmentService(DatabaseConnection database)
        {
            _database = database;
        }

        public int CreateEnrolment(
            int participantID,
            EnrolmentRequest request)
        {
            using SqlConnection connection = _database.GetConnection();
            connection.Open();

            string query = @"
                INSERT INTO EventEnrolments
                (ParticipantID, EventID, CategoryID)
                SELECT @ParticipantID, @EventID, @CategoryID
                WHERE EXISTS
                (
                    SELECT 1
                    FROM Categories
                    WHERE CategoryID = @CategoryID
                    AND EventID = @EventID
                );

                SELECT CAST(SCOPE_IDENTITY() AS INT);";

            using SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@ParticipantID", participantID);
            command.Parameters.AddWithValue("@EventID", request.EventID);
            command.Parameters.AddWithValue("@CategoryID", request.CategoryID);

            object? result = command.ExecuteScalar();

            if (result == null || result == DBNull.Value)
            {
                return 0;
            }

            return Convert.ToInt32(result);
        }

        public List<EventEnrolment> GetMyEnrolments(int participantID)
        {
            List<EventEnrolment> enrolments = new List<EventEnrolment>();

            using SqlConnection connection = _database.GetConnection();
            connection.Open();

            string query = @"
                SELECT EnrolmentID, ParticipantID, EventID,
                       CategoryID, EnrolmentDate
                FROM EventEnrolments
                WHERE ParticipantID = @ParticipantID";

            using SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@ParticipantID", participantID);

            using SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                enrolments.Add(new EventEnrolment
                {
                    EnrolmentID = (int)reader["EnrolmentID"],
                    ParticipantID = (int)reader["ParticipantID"],
                    EventID = (int)reader["EventID"],
                    CategoryID = (int)reader["CategoryID"],
                    EnrolmentDate = (DateTime)reader["EnrolmentDate"]
                });
            }

            return enrolments;
        }

        public bool DeleteEnrolment(int enrolmentID, int participantID)
        {
            using SqlConnection connection = _database.GetConnection();
            connection.Open();

            string query = @"
                DELETE FROM EventEnrolments
                WHERE EnrolmentID = @EnrolmentID
                AND ParticipantID = @ParticipantID";

            using SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@EnrolmentID", enrolmentID);
            command.Parameters.AddWithValue("@ParticipantID", participantID);

            return command.ExecuteNonQuery() > 0;
        }
    }
}

using Microsoft.Data.SqlClient;
using RaceDayAPI.Data;
using RaceDayAPI.DTOs;
using RaceDayAPI.Models;
using System.Collections.Generic;

namespace RaceDayAPI.Services
{//start of namespace
    public class EventService
    {//start of class
        private readonly DatabaseConnection _database;

        public EventService(DatabaseConnection database)
        {
            _database = database;
        }

        public List<Event> GetAllEvents()
        {
            List<Event> events = new List<Event>();

            using SqlConnection connection = _database.GetConnection();
            connection.Open();

            string query = @"
                SELECT EventID, OrganizerID, EventTypeID,
                       EventName, Description, EventDate,
                       Location, Distance
                FROM Events";

            using SqlCommand command = new SqlCommand(query, connection);
            using SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                events.Add(new Event
                {
                    EventID = (int)reader["EventID"],
                    OrganizerID = (int)reader["OrganizerID"],
                    EventTypeID = (int)reader["EventTypeID"],
                    EventName = reader["EventName"].ToString()!,
                    Description = reader["Description"] == DBNull.Value
                        ? null
                        : reader["Description"].ToString(),
                    EventDate = (DateTime)reader["EventDate"],
                    Location = reader["Location"].ToString()!,
                    Distance = (decimal)reader["Distance"]
                });
            }

            return events;
        }

        public Event? GetEventByID(int eventID)
        {
            using SqlConnection connection = _database.GetConnection();
            connection.Open();

            string query = @"
                SELECT EventID, OrganizerID, EventTypeID,
                       EventName, Description, EventDate,
                       Location, Distance
                FROM Events
                WHERE EventID = @EventID";

            using SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@EventID", eventID);

            using SqlDataReader reader = command.ExecuteReader();

            if (!reader.Read())
            {
                return null;
            }

            return new Event
            {
                EventID = (int)reader["EventID"],
                OrganizerID = (int)reader["OrganizerID"],
                EventTypeID = (int)reader["EventTypeID"],
                EventName = reader["EventName"].ToString()!,
                Description = reader["Description"] == DBNull.Value
                    ? null
                    : reader["Description"].ToString(),
                EventDate = (DateTime)reader["EventDate"],
                Location = reader["Location"].ToString()!,
                Distance = (decimal)reader["Distance"]
            };
        }

        public virtual int CreateEvent(int organizerID, EventRequest request)
        {
            using SqlConnection connection = _database.GetConnection();
            connection.Open();

            string query = @"
                INSERT INTO Events
                (OrganizerID, EventTypeID, EventName, Description,
                 EventDate, Location, Distance)
                VALUES
                (@OrganizerID, @EventTypeID, @EventName, @Description,
                 @EventDate, @Location, @Distance);

                SELECT CAST(SCOPE_IDENTITY() AS INT);";

            using SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@OrganizerID", organizerID);
            command.Parameters.AddWithValue("@EventTypeID", request.EventTypeID);
            command.Parameters.AddWithValue("@EventName", request.EventName);
            command.Parameters.AddWithValue(
                "@Description",
                (object?)request.Description ?? DBNull.Value);
            command.Parameters.AddWithValue("@EventDate", request.EventDate);
            command.Parameters.AddWithValue("@Location", request.Location);
            command.Parameters.AddWithValue("@Distance", request.Distance);

            return (int)command.ExecuteScalar()!;
        }

        public bool UpdateEvent(
            int eventID,
            int organizerID,
            EventRequest request)
        {
            using SqlConnection connection = _database.GetConnection();
            connection.Open();

            string query = @"
                UPDATE Events
                SET EventTypeID = @EventTypeID,
                    EventName = @EventName,
                    Description = @Description,
                    EventDate = @EventDate,
                    Location = @Location,
                    Distance = @Distance
                WHERE EventID = @EventID
                AND OrganizerID = @OrganizerID";

            using SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@EventID", eventID);
            command.Parameters.AddWithValue("@OrganizerID", organizerID);
            command.Parameters.AddWithValue("@EventTypeID", request.EventTypeID);
            command.Parameters.AddWithValue("@EventName", request.EventName);
            command.Parameters.AddWithValue(
                "@Description",
                (object?)request.Description ?? DBNull.Value);
            command.Parameters.AddWithValue("@EventDate", request.EventDate);
            command.Parameters.AddWithValue("@Location", request.Location);
            command.Parameters.AddWithValue("@Distance", request.Distance);

            return command.ExecuteNonQuery() > 0;
        }

        public bool DeleteEvent(int eventID, int organizerID)
        {
            using SqlConnection connection = _database.GetConnection();
            connection.Open();

            string query = @"
                DELETE FROM Events
                WHERE EventID = @EventID
                AND OrganizerID = @OrganizerID";

            using SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@EventID", eventID);
            command.Parameters.AddWithValue("@OrganizerID", organizerID);

            return command.ExecuteNonQuery() > 0;
        }
    }//end of class
}//end of namespace

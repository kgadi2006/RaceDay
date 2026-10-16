using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using RaceDayAPI.Data;
using Microsoft.AspNetCore.Authorization;
using System;

namespace RaceDayAPI.Controllers
{//start of namespace
    [ApiController]
    [Route("api/[controller]")]
    public class DatabaseTestController : ControllerBase
    {//start of class
        private readonly DatabaseConnection _database;

        public DatabaseTestController(DatabaseConnection database)
        {
            _database = database;
        }

        [Authorize]
        [HttpGet]
        public IActionResult TestConnection()
        {
            try
            {
                using SqlConnection connection = _database.GetConnection();

                connection.Open();

                using SqlCommand command = new SqlCommand(
                    "SELECT DB_NAME()", connection);

                string? databaseName = command.ExecuteScalar()?.ToString();

                return Ok(new
                {
                    message = "Database connection successful",
                    database = databaseName
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    error = ex.Message
                });
            }
        }
    }//start of class
}//start of namespace

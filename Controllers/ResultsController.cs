using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RaceDayAPI.DTOs;
using RaceDayAPI.Services;

namespace RaceDayAPI.Controllers
{
    [ApiController]
    [Route("api")]
    public class ResultsController : ControllerBase
    {
        private readonly ResultService _resultService;

        public ResultsController(ResultService resultService)
        {
            _resultService = resultService;
        }

        [HttpGet("events/{eventID}/results")]
        public IActionResult GetEventResults(int eventID)
        {
            if (eventID <= 0)
            {
                return BadRequest(new
                {
                    message = "Invalid event ID."
                });
            }

            return Ok(_resultService.GetEventResults(eventID));
        }

        [Authorize(Roles = "Participant")]
        [HttpGet("results/me")]
        public IActionResult GetMyResults()
        {
            string? userID = User.FindFirst(
                ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(userID, out int participantID))
            {
                return Unauthorized();
            }

            return Ok(_resultService.GetMyResults(participantID));
        }

        [Authorize(Roles = "Organizer")]
        [HttpPost("events/{eventID}/results")]
        public IActionResult CreateResult(
            int eventID,
            ResultRequest request)
        {
            if (eventID <= 0 ||
                request.EnrolmentID <= 0 ||
                request.FinishTime < TimeSpan.Zero ||
                request.FinishingPosition <= 0)
            {
                return BadRequest(new
                {
                    message = "Please provide valid result details."
                });
            }

            try
            {
                int resultID = _resultService.CreateResult(request);

                return StatusCode(201, new
                {
                    message = "Result recorded successfully.",
                    resultID
                });
            }
            catch (Microsoft.Data.SqlClient.SqlException ex)
                when (ex.Number == 2627 || ex.Number == 2601)
            {
                return Conflict(new
                {
                    message = "A result already exists for this enrolment."
                });
            }
        }

        [Authorize(Roles = "Organizer")]
        [HttpPut("results/{id}")]
        public IActionResult UpdateResult(int id, ResultRequest request)
        {
            if (id <= 0 ||
                request.EnrolmentID <= 0 ||
                request.FinishTime < TimeSpan.Zero ||
                request.FinishingPosition <= 0)
            {
                return BadRequest(new
                {
                    message = "Please provide valid result details."
                });
            }

            bool updated = _resultService.UpdateResult(id, request);

            if (!updated)
            {
                return NotFound(new
                {
                    message = "Result not found."
                });
            }

            return Ok(new
            {
                message = "Result updated successfully."
            });
        }
    }
}



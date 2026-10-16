using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RaceDayAPI.DTOs;
using RaceDayAPI.Services;

namespace RaceDayAPI.Controllers
{//start of namespace
    [ApiController]
    [Route("api")]
    public class EnrolmentsController : ControllerBase
    {//start of class
        private readonly EnrolmentService _enrolmentService;

        public EnrolmentsController(EnrolmentService enrolmentService)
        {
            _enrolmentService = enrolmentService;
        }

        [Authorize(Roles = "Participant")]
        [HttpPost("events/{eventID}/enrolments")]
        public IActionResult CreateEnrolment(
            int eventID,
            EnrolmentRequest request)
        {
            if (eventID <= 0 || request.CategoryID <= 0)
            {
                return BadRequest(new
                {
                    message = "Invalid event or category ID."
                });
            }

            if (request.EventID != 0 && request.EventID != eventID)
            {
                return BadRequest(new
                {
                    message = "The event ID does not match the request."
                });
            }

            string? userID = User.FindFirst(
                ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(userID, out int participantID))
            {
                return Unauthorized();
            }

            request.EventID = eventID;

            try
            {
                int enrolmentID = _enrolmentService.CreateEnrolment(
                    participantID, request);

                if (enrolmentID == 0)
                {
                    return BadRequest(new
                    {
                        message = "The selected category does not belong to this event."
                    });
                }

                return StatusCode(201, new
                {
                    message = "Enrolment successful.",
                    enrolmentID
                });
            }
            catch (Microsoft.Data.SqlClient.SqlException ex)
                when (ex.Number == 2627 || ex.Number == 2601)
            {
                return Conflict(new
                {
                    message = "You have already enrolled in this event."
                });
            }
        }

        [Authorize(Roles = "Participant")]
        [HttpGet("enrolments/me")]
        public IActionResult GetMyEnrolments()
        {
            string? userID = User.FindFirst(
                ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(userID, out int participantID))
            {
                return Unauthorized();
            }

            return Ok(_enrolmentService.GetMyEnrolments(participantID));
        }

        [Authorize(Roles = "Participant")]
        [HttpDelete("enrolments/{id}")]
        public IActionResult DeleteEnrolment(int id)
        {
            if (id <= 0)
            {
                return BadRequest(new
                {
                    message = "Invalid enrolment ID."
                });
            }

            string? userID = User.FindFirst(
                ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(userID, out int participantID))
            {
                return Unauthorized();
            }

            bool deleted = _enrolmentService.DeleteEnrolment(
                id, participantID);

            if (!deleted)
            {
                return NotFound(new
                {
                    message = "Enrolment not found."
                });
            }

            return Ok(new
            {
                message = "Enrolment cancelled successfully."
            });
        }
    }//end of class
}//end of namespace


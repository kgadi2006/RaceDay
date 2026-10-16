using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using RaceDayAPI.DTOs;
using RaceDayAPI.Services;

namespace RaceDayAPI.Controllers
{//start of namespace
    [ApiController]
    [Route("api/events")]
    public class EventsController : ControllerBase
    {//start of class
        private readonly EventService _eventService;

        public EventsController(EventService eventService)
        {
            _eventService = eventService;
        }

        [HttpGet]
        public IActionResult GetAllEvents()
        {
            return Ok(_eventService.GetAllEvents());
        }

        [HttpGet("{id}")]
        public IActionResult GetEventByID(int id)
        {
            if (id <= 0)
            {
                return BadRequest(new
                {
                    message = "Invalid event ID."
                });
            }

            var eventItem = _eventService.GetEventByID(id);

            if (eventItem == null)
            {
                return NotFound(new
                {
                    message = "Event not found."
                });
            }

            return Ok(eventItem);
        }

        [Authorize(Roles = "Organizer")]
        [HttpPost]
        public IActionResult CreateEvent(EventRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.EventName) ||
                string.IsNullOrWhiteSpace(request.Location) ||
                request.EventTypeID <= 0 ||
                request.Distance <= 0 ||
                request.EventDate.Date < DateTime.Today)
            {
                return BadRequest(new
                {
                    message = "Please provide valid event details."
                });
            }

            string? userID = User.FindFirst(
                ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(userID, out int organizerID))
            {
                return Unauthorized();
            }

            int eventID = _eventService.CreateEvent(organizerID, request);

            return CreatedAtAction(
                nameof(GetEventByID),
                new { id = eventID },
                new
                {
                    message = "Event created successfully.",
                    eventID
                });
        }

        [Authorize(Roles = "Organizer")]
        [HttpPut("{id}")]
        public IActionResult UpdateEvent(int id, EventRequest request)
        {
            if (id <= 0 ||
                string.IsNullOrWhiteSpace(request.EventName) ||
                string.IsNullOrWhiteSpace(request.Location) ||
                request.EventTypeID <= 0 ||
                request.Distance <= 0 ||
                request.EventDate.Date < DateTime.Today)
            {
                return BadRequest(new
                {
                    message = "Please provide valid event details."
                });
            }

            string? userID = User.FindFirst(
                ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(userID, out int organizerID))
            {
                return Unauthorized();
            }

            bool updated = _eventService.UpdateEvent(
                id, organizerID, request);

            if (!updated)
            {
                return NotFound(new
                {
                    message = "Event not found or you do not own this event."
                });
            }

            return Ok(new
            {
                message = "Event updated successfully."
            });
        }

        [Authorize(Roles = "Organizer")]
        [HttpDelete("{id}")]
        public IActionResult DeleteEvent(int id)
        {
            if (id <= 0)
            {
                return BadRequest(new
                {
                    message = "Invalid event ID."
                });
            }

            string? userID = User.FindFirst(
                ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(userID, out int organizerID))
            {
                return Unauthorized();
            }

            bool deleted = _eventService.DeleteEvent(id, organizerID);

            if (!deleted)
            {
                return NotFound(new
                {
                    message = "Event not found or you do not own this event."
                });
            }

            return Ok(new
            {
                message = "Event deleted successfully."
            });
        }
    }//end of class
}//end of namespace

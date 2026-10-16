using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RaceDayAPI.Controllers;
using System.Reflection;
using Xunit;
using Microsoft.AspNetCore.Http;
using Moq;
using RaceDayAPI.DTOs;
using RaceDayAPI.Services;
using System.Security.Claims;

namespace RaceDay.Tests
{
    public class EventsControllerTests
    {
        [Fact]
        public void CreateEvent_RequiresOrganizerRole()
        {
            var method = typeof(EventsController).GetMethod("CreateEvent");

            var attribute = method?.GetCustomAttribute<AuthorizeAttribute>();

            Assert.NotNull(attribute);
            Assert.Equal("Organizer", attribute!.Roles);
        }

        [Fact]
        public void UpdateEvent_RequiresOrganizerRole()
        {
            var method = typeof(EventsController).GetMethod("UpdateEvent");

            var attribute = method?.GetCustomAttribute<AuthorizeAttribute>();

            Assert.NotNull(attribute);
            Assert.Equal("Organizer", attribute!.Roles);
        }

        [Fact]
        public void DeleteEvent_RequiresOrganizerRole()
        {
            var method = typeof(EventsController).GetMethod("DeleteEvent");

            var attribute = method?.GetCustomAttribute<AuthorizeAttribute>();

            Assert.NotNull(attribute);
            Assert.Equal("Organizer", attribute!.Roles);
        }


        [Fact]
        public void CreateEvent_InvalidDetails_ReturnsBadRequest()
        {
            var eventService = new Mock<EventService>(null!);

            var controller = new EventsController(eventService.Object);

            var request = new EventRequest
            {
                EventName = "",
                Location = "Johannesburg",
                EventTypeID = 1,
                Distance = 10,
                EventDate = DateTime.Today.AddDays(7)
            };

            var result = controller.CreateEvent(request);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void CreateEvent_ValidDetails_ReturnsCreated()
        {
            var eventService = new Mock<EventService>(null!);

            eventService
                .Setup(service => service.CreateEvent(
                    5, It.IsAny<EventRequest>()))
                .Returns(10);

            var controller = new EventsController(eventService.Object);

            var claims = new[]
            {
        new Claim(ClaimTypes.NameIdentifier, "5"),
        new Claim(ClaimTypes.Role, "Organizer")
    };

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(
                        new ClaimsIdentity(claims, "Test"))
                }
            };

            var request = new EventRequest
            {
                EventName = "Test Race",
                Location = "Johannesburg",
                EventTypeID = 1,
                Distance = 10,
                EventDate = DateTime.Today.AddDays(7)
            };

            var result = controller.CreateEvent(request);

            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal("GetEventByID", createdResult.ActionName);
            eventService.Verify(
                service => service.CreateEvent(5, request),
                Times.Once);
        }

    }
}

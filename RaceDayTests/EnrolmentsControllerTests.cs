using Microsoft.AspNetCore.Authorization;
using RaceDayAPI.Controllers;
using System.Reflection;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Moq;
using RaceDayAPI.DTOs;
using RaceDayAPI.Services;
using System.Security.Claims;

namespace RaceDay.Tests
{
    public class EnrolmentsControllerTests
    {
        [Fact]
        public void CreateEnrolment_RequiresParticipantRole()
        {
            var method = typeof(EnrolmentsController)
                .GetMethod("CreateEnrolment");

            var attribute = method?.GetCustomAttribute<AuthorizeAttribute>();

            Assert.NotNull(attribute);
            Assert.Equal("Participant", attribute!.Roles);
        }

        [Fact]
        public void GetMyEnrolments_RequiresParticipantRole()
        {
            var method = typeof(EnrolmentsController)
                .GetMethod("GetMyEnrolments");

            var attribute = method?.GetCustomAttribute<AuthorizeAttribute>();

            Assert.NotNull(attribute);
            Assert.Equal("Participant", attribute!.Roles);
        }

        [Fact]
        public void DeleteEnrolment_RequiresParticipantRole()
        {
            var method = typeof(EnrolmentsController)
                .GetMethod("DeleteEnrolment");

            var attribute = method?.GetCustomAttribute<AuthorizeAttribute>();

            Assert.NotNull(attribute);
            Assert.Equal("Participant", attribute!.Roles);
        }


        [Fact]
        public void CreateEnrolment_InvalidEventID_ReturnsBadRequest()
        {
            var enrolmentService = new Mock<EnrolmentService>(null!);
            var controller = new EnrolmentsController(enrolmentService.Object);

            var request = new EnrolmentRequest
            {
                EventID = 0,
                CategoryID = 1
            };

            var result = controller.CreateEnrolment(0, request);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void CreateEnrolment_InvalidCategoryID_ReturnsBadRequest()
        {
            var enrolmentService = new Mock<EnrolmentService>(null!);
            var controller = new EnrolmentsController(enrolmentService.Object);

            var request = new EnrolmentRequest
            {
                EventID = 1,
                CategoryID = 0
            };

            var result = controller.CreateEnrolment(1, request);

            Assert.IsType<BadRequestObjectResult>(result);
        }

    }
}

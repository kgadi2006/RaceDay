using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RaceDayAPI.Controllers;
using System.Reflection;
using Xunit;
using Moq;
using RaceDayAPI.DTOs;
using RaceDayAPI.Services;

namespace RaceDay.Tests
{
    public class ResultsControllerTests
    {
        [Fact]
        public void GetEventResults_DoesNotRequireAuthorization()
        {
            var method = typeof(ResultsController)
                .GetMethod("GetEventResults");

            var attribute = method?.GetCustomAttribute<AuthorizeAttribute>();

            Assert.Null(attribute);
        }

        [Fact]
        public void GetMyResults_RequiresParticipantRole()
        {
            var method = typeof(ResultsController)
                .GetMethod("GetMyResults");

            var attribute = method?.GetCustomAttribute<AuthorizeAttribute>();

            Assert.NotNull(attribute);
            Assert.Equal("Participant", attribute!.Roles);
        }

        [Fact]
        public void CreateResult_RequiresOrganizerRole()
        {
            var method = typeof(ResultsController)
                .GetMethod("CreateResult");

            var attribute = method?.GetCustomAttribute<AuthorizeAttribute>();

            Assert.NotNull(attribute);
            Assert.Equal("Organizer", attribute!.Roles);
        }

        [Fact]
        public void UpdateResult_RequiresOrganizerRole()
        {
            var method = typeof(ResultsController)
                .GetMethod("UpdateResult");

            var attribute = method?.GetCustomAttribute<AuthorizeAttribute>();

            Assert.NotNull(attribute);
            Assert.Equal("Organizer", attribute!.Roles);
        }


        [Fact]
        public void CreateResult_InvalidEnrolmentID_ReturnsBadRequest()
        {
            var resultService = new Mock<ResultService>(null!);
            var controller = new ResultsController(resultService.Object);

            var request = new ResultRequest
            {
                EnrolmentID = 0,
                FinishTime = TimeSpan.FromMinutes(30),
                FinishingPosition = 1
            };

            var result = controller.CreateResult(1, request);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void CreateResult_InvalidFinishingPosition_ReturnsBadRequest()
        {
            var resultService = new Mock<ResultService>(null!);
            var controller = new ResultsController(resultService.Object);

            var request = new ResultRequest
            {
                EnrolmentID = 1,
                FinishTime = TimeSpan.FromMinutes(30),
                FinishingPosition = 0
            };

            var result = controller.CreateResult(1, request);

            Assert.IsType<BadRequestObjectResult>(result);
        }

    }
}

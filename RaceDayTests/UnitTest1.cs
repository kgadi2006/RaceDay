
using Microsoft.AspNetCore.Mvc;
using Moq;
using RaceDayAPI.Controllers;
using RaceDayAPI.DTOs;
using RaceDayAPI.Services;
using Xunit;

namespace RaceDay.Tests
{
    public class AuthControllerTests
    {
        private readonly Mock<UserService> _userService;
        private readonly Mock<TokenService> _tokenService;
        private readonly AuthController _controller;

        public AuthControllerTests()
        {
            _userService = new Mock<UserService>(
                null!, null!);

            _tokenService = new Mock<TokenService>(
                null!);

            _controller = new AuthController(
                _userService.Object,
                _tokenService.Object);
        }

        [Fact]
        public void Register_MissingName_ReturnsBadRequest()
        {
            // Arrange
            var request = new RegisterRequest
            {
                FullName = "",
                Email = "test@example.com",
                Password = "RaceDay123!",
                Role = "Participant"
            };

            // Act
            var result = _controller.Register(request);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void Register_InvalidRole_ReturnsBadRequest()
        {
            // Arrange
            var request = new RegisterRequest
            {
                FullName = "Test User",
                Email = "test@example.com",
                Password = "RaceDay123!",
                Role = "Admin"
            };

            // Act
            var result = _controller.Register(request);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void Login_MissingEmail_ReturnsBadRequest()
        {
            // Arrange
            var request = new LoginRequest
            {
                Email = "",
                Password = "RaceDay123!"
            };

            // Act
            var result = _controller.Login(request);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }


        [Fact]
        public void Register_DuplicateEmail_ReturnsConflict()
        {
            // Arrange
            var request = new RegisterRequest
            {
                FullName = "Test User",
                Email = "participant@test.com",
                Password = "RaceDay123!",
                Role = "Participant"
            };

            _userService
                .Setup(service => service.Register(request))
                .Returns("Email already exists");

            // Act
            var result = _controller.Register(request);

            // Assert
            Assert.IsType<ConflictObjectResult>(result);
        }

        [Fact]
        public void Login_InvalidCredentials_ReturnsUnauthorized()
        {
            // Arrange
            var request = new LoginRequest
            {
                Email = "wrong@test.com",
                Password = "WrongPassword123!"
            };

            _userService
                .Setup(service => service.Login(request))
                .Returns((object?)null);

            // Act
            var result = _controller.Login(request);

            // Assert
            Assert.IsType<UnauthorizedObjectResult>(result);
        }


        [Fact]
        public void Register_ValidDetails_ReturnsCreated()
        {
            // Arrange
            var request = new RegisterRequest
            {
                FullName = "Test User",
                Email = "newuser@test.com",
                Password = "RaceDay123!",
                Role = "Participant"
            };

            _userService
                .Setup(service => service.Register(request))
                .Returns("Registration successful");

            // Act
            var result = _controller.Register(request);

            // Assert
            var createdResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(201, createdResult.StatusCode);
        }


        [Fact]
        public void Login_MissingPassword_ReturnsBadRequest()
        {
            // Arrange
            var request = new LoginRequest
            {
                Email = "participant@test.com",
                Password = ""
            };

            // Act
            var result = _controller.Login(request);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }


 
    }
}

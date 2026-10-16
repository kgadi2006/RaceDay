using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RaceDayAPI.DTOs;
using RaceDayAPI.Services;
using System;

namespace RaceDayAPI.Controllers
{//start of namespace
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {//start of class
        private readonly UserService _userService;
        private readonly TokenService _tokenService;

        public AuthController( UserService userService, TokenService tokenService)
        {
            _userService = userService;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public IActionResult Register(RegisterRequest request)
        {//start of register method
            if (string.IsNullOrWhiteSpace(request.FullName) ||
                string.IsNullOrWhiteSpace(request.Email) ||
                string.IsNullOrWhiteSpace(request.Password))
            {
                return BadRequest(new
                {
                    message = "Name, email and password are required."
                });
            }

            if (request.Role != "Organizer" &&
                request.Role != "Participant")
            {
                return BadRequest(new
                {
                    message = "Role must be Organizer or Participant."
                });
            }

            try
            {
                string result = _userService.Register(request);

                if (result == "Email already exists")
                {
                    return Conflict(new { message = result });
                }

                return StatusCode(201, new { message = result });
            }
           
catch (Exception ex)
{
    return StatusCode(500, new
    {
        message = ex.ToString()
    });
}

        }//end of register method


        [HttpPost("login")]
        public IActionResult Login(LoginRequest request)
        {//start of login method
            if (string.IsNullOrWhiteSpace(request.Email) ||
                string.IsNullOrWhiteSpace(request.Password))
            {
                return BadRequest(new
                {
                    message = "Email and password are required."
                });
            }

            try
            {
                object? user = _userService.Login(request);

                if (user == null)
                {
                    return Unauthorized(new
                    {
                        message = "Invalid email or password."
                    });
                }

                var loggedInUser = (dynamic)user;

                string token = _tokenService.CreateToken(
                    loggedInUser.UserID,
                    loggedInUser.Email,
                    loggedInUser.Role);

                return Ok(new
                {
                    message = "Login successful.",
                    token = token,
                    user = loggedInUser
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = ex.Message
                });
            }
        }//end of login method

    }//end of class
}//end of namespace

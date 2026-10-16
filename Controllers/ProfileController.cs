using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RaceDayAPI.DTOs;
using RaceDayAPI.Services;

namespace RaceDayAPI.Controllers
{//start of namespace
    [ApiController]
    [Route("api/profile")]
    [Authorize]
    public class ProfileController : ControllerBase
    {//start of class
        private readonly ProfileService _profileService;

        public ProfileController(ProfileService profileService)
        {
            _profileService = profileService;
        }

        [HttpGet]
        public IActionResult GetProfile()
        {
            string? userID = User.FindFirst(
                ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(userID, out int id))
            {
                return Unauthorized();
            }

            var profile = _profileService.GetProfile(id);

            if (profile == null)
            {
                return NotFound(new
                {
                    message = "Profile not found."
                });
            }

            return Ok(profile);
        }

        [HttpPut]
        public IActionResult UpdateProfile(
            UpdateProfileRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.FullName))
            {
                return BadRequest(new
                {
                    message = "Full name is required."
                });
            }

            string? userID = User.FindFirst(
                ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(userID, out int id))
            {
                return Unauthorized();
            }

            bool updated = _profileService.UpdateProfile(id, request);

            if (!updated)
            {
                return NotFound(new
                {
                    message = "Profile not found."
                });
            }

            return Ok(new
            {
                message = "Profile updated successfully."
            });
        }
    }//end of class
}//end of namespace


using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RaceDayAPI.DTOs;
using RaceDayAPI.Services;

namespace RaceDayAPI.Controllers
{
    [ApiController]
    [Route("api")]
    public class CategoriesController : ControllerBase
    {
        private readonly CategoryService _categoryService;

        public CategoriesController(CategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet("events/{eventID}/categories")]
        public IActionResult GetCategories(int eventID)
        {
            if (eventID <= 0)
            {
                return BadRequest(new
                {
                    message = "Invalid event ID."
                });
            }

            return Ok(_categoryService.GetCategories(eventID));
        }

        [Authorize(Roles = "Organizer")]
        [HttpPost("categories")]
        public IActionResult CreateCategory(CategoryRequest request)
        {
            if (request.EventID <= 0 ||
                string.IsNullOrWhiteSpace(request.CategoryName) ||
                string.IsNullOrWhiteSpace(request.CategoryType) ||
                string.IsNullOrWhiteSpace(request.CategoryValue))
            {
                return BadRequest(new
                {
                    message = "All category details are required."
                });
            }

            if (request.CategoryType != "Age" &&
                request.CategoryType != "Distance")
            {
                return BadRequest(new
                {
                    message = "Category type must be Age or Distance."
                });
            }

            int categoryID = _categoryService.CreateCategory(request);

            return StatusCode(201, new
            {
                message = "Category created successfully.",
                categoryID
            });
        }
    }
}


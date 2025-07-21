using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentManagement.Business.DTOs;
using StudentManagement.Business.Interfaces;
using System.Security.Claims;

namespace StudentManagement.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Student")]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _studentService;

        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }
        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            if (!TryGetUserId(out var userId))
                return Unauthorized();

            var dto = await _studentService.GetProfileAsync(userId);
            if (dto == null)
                return NotFound(new { message = "Profile not found" });

            return Ok(dto);
        }
        [HttpPost("profile")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> CreateProfile([FromForm] StudentDto dto)
        {
            Console.WriteLine($"DTO: Name={dto.Name}, Email={dto.Email}, Phone={dto.Phone}, Age={dto.Age}, CardImage={dto.CardImage?.FileName}");
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!TryGetUserId(out var userId))
                return Unauthorized();

            try
            {
                await _studentService.CreateProfileAsync(dto, userId);
                return Ok(new { message = "Profile created successfully" });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

      
        [HttpPut("profile")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> EditProfile([FromForm] StudentDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!TryGetUserId(out var userId))
                return Unauthorized();

            try
            {
                await _studentService.UpdateProfileAsync(dto, userId);
                return Ok(new { message = "Profile updated successfully" });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("card-image")]
        public async Task<IActionResult> GetCardImage()
        {
            if (!TryGetUserId(out var userId))
                return Unauthorized();

            var result = await _studentService.GetCardImageAsync(userId);
            if (result == null)
                return NotFound(new { message = "Image not found" });

            return File(result.Value.fileBytes, result.Value.contentType);
        }

        [HttpGet("enrolled-courses")]
        public async Task<IActionResult> GetEnrolledCourses()
        {
            if (!TryGetUserId(out var userId))
                return Unauthorized();

            var courses = await _studentService.GetEnrolledCoursesAsync(userId);
            return Ok(courses);
        }
        private bool TryGetUserId(out int userId)
        {
            userId = 0;
            var userIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.TryParse(userIdStr, out userId);
        }


    }
}

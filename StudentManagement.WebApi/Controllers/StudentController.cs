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

        [AllowAnonymous]
        [HttpPost("profile")]
        public async Task<IActionResult> CreateProfile([FromBody] StudentDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!TryGetUserId(out var userId))
                return Unauthorized();

            await _studentService.CreateProfileAsync(dto, userId);
            return Ok(new { message = "Profile created successfully" });
        }

      
        [HttpPut("profile")]
        public async Task<IActionResult> EditProfile([FromBody] StudentDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!TryGetUserId(out var userId))
                return Unauthorized();

            await _studentService.UpdateProfileAsync(dto, userId);
            return Ok(new { message = "Profile updated successfully" });
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

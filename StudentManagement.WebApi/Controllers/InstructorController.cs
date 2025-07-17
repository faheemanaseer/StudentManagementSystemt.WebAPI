using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentManagement.Business.Interfaces;
using StudentManagement.Business.DTOs;


namespace StudentManagement.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class InstructorController : ControllerBase
    {
        private readonly IInstructorService _instructorService;
        private readonly ICourseService _courseService;

        public InstructorController(IInstructorService instructorService, ICourseService courseService)
        {
            _instructorService = instructorService;
            _courseService = courseService;
        }

       
        [HttpGet]
        public async Task<IActionResult> GetAllInstructors()
        {
            var instructors = await _instructorService.GetAllAsync();
            return Ok(instructors);
        }

      
        [HttpPost]
        public async Task<IActionResult> AddInstructor([FromBody] InstructorDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _instructorService.AddAsync(dto);
            return Ok(new { message = "Instructor added successfully." });
        }

        
        [HttpGet("courses")]
        public async Task<IActionResult> GetCoursesWithInstructors()
        {
            var result = await _courseService.GetCoursesWithInstructorsAsync();
            return Ok(result);
        }

       
        [HttpGet("select")]
        public async Task<IActionResult> GetSelectableInstructors()
        {
            var instructors = await _courseService.GetAllInstructorsAsync();
            return Ok(instructors);
        }

        [HttpPost("assign")]
        public async Task<IActionResult> AssignInstructorToCourse([FromBody] AssignInstructorDto model)
        {
            await _courseService.AssignInstructorAsync(model.CourseId, model.InstructorId);
            return Ok(new { message = "Instructor assigned to course successfully." });
        }
    }
}

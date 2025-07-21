using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentManagement.Business.DTOs;
using StudentManagement.Business.Interfaces;

namespace StudentManagement.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly ICourseService _courseService;
        private readonly IStudentCourseService _studentCourseService;
        private readonly IStudentService _studentService;
         public AdminController(ICourseService courseService,IStudentCourseService studentCourseService,IStudentService studentService)
         {
            _courseService = courseService;
            _studentCourseService = studentCourseService;
            _studentService = studentService;
         }
        [HttpGet("courses")]
        public async Task<IActionResult> GetCourses()
        {
            var courses = await _courseService.GetAllAsync();
            return Ok(courses);
        }
        [HttpGet("courses/{id}")]
        public async Task<IActionResult> GetCourseById(int id)
        {
            var course = await _courseService.GetByIdAsync(id);
            return Ok(course);
        }
        [HttpPost("courses")]
        public async Task<IActionResult> AddCourse([FromBody] CourseDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _courseService.AddAsync(model);
            return Ok(new { message = "Course added successfully" });
        }
        [HttpPut("courses/{id}")]
        public async Task<IActionResult> UpdateCourse(int id, [FromBody] CourseDto model)
        {
            if (id != model.SId)
                return BadRequest("Course ID mismatch");

            await _courseService.UpdateAsync(model);
            return Ok(new { message = "Course updated successfully" });
        }
        [HttpDelete("courses/{id}")]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            await _courseService.DeleteAsync(id);
            return Ok(new { message = "Course deleted successfully" });
        }
        [HttpGet("students")]
        public async Task<IActionResult> GetAllStudents()
        {
            var students = await _studentCourseService.GetAllStudentsAsync();
            return Ok(students);
        }
        [HttpPost("assign-course")]
        public async Task<IActionResult> AssignCourse([FromQuery] int studentId, [FromQuery] int courseId)
        {
            if (await _studentCourseService.IsAlreadyEnrolledAsync(studentId, courseId))
                return BadRequest("This student is already enrolled in the selected course.");

            await _studentCourseService.AssignCourseAsync(studentId, courseId);
            return Ok(new { message = "Course assigned successfully" });
        }
        [HttpGet("assigned-courses/{studentId}")]
        public async Task<IActionResult> GetAssignedCourses(int studentId)
        {
            var assigned = await _studentCourseService.GetAssignedCoursesAsync(studentId);
            return Ok(assigned);
        }
        [HttpGet("search")]
        public async Task<IActionResult> SearchStudents(string name)
        {
            var result = await _studentService.SearchByNameAsync(name);
            return Ok(result);
        }
    }
}

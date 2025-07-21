using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentManagement.Application.Entities;
using StudentManagement.Business.DTOs;
using StudentManagement.Business.Interfaces;
using StudentManagement.DataAccess.Data;
using System.Security.Claims;

namespace StudentManagementSystem.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] 
    public class ExamResultController : ControllerBase
    {
        private readonly IExamResultService _examResultService;
        private readonly ApplicationDbContext _context;

        public ExamResultController(IExamResultService examResultService , ApplicationDbContext context)
        {
            _examResultService = examResultService;
            _context = context;
        }

        // Admin: Add new exam result
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Add([FromBody] CreateExamResultDto dto)
        {
            try
            {
                await _examResultService.AddExamResultAsync(dto);
                return Ok("Exam result added successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Admin: Get all exam results
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll()
        {
            var results = await _examResultService.GetAllAsync();
            return Ok(results);
        }

        // Admin: Get exam result by ID
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var result = await _examResultService.GetByIdAsync(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        // Admin: Get results by student ID
        [HttpGet("Getbystudent/{studentId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetByStudentId(int studentId)
        {
            var results = await _examResultService.GetByStudentIdAsync(studentId);
            return Ok(results);
        }

        // Admin: Get results by course ID
        [HttpGet("Getbycourse/{courseId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetByCourseId(int courseId)
        {
            var results = await _examResultService.GetByCourseIdAsync(courseId);
            return Ok(results);
        }

        // Admin: Search by student name and optional course
        [HttpGet("searchByName")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Search([FromQuery] string query, [FromQuery] int? courseId)
        {
            var results = await _examResultService.SearchByStudentNameAsync(query, courseId);
            return Ok(results);
        }

        // Admin: Update exam result
        [HttpPut("Updateby{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] CreateExamResultDto dto)
        {
            try
            {
                await _examResultService.UpdateExamResultAsync(id, dto);
                return Ok("Exam result updated successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Admin: Delete exam result
        [HttpDelete("DeleteBy{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _examResultService.DeleteExamResultAsync(id);
                return Ok("Exam result deleted successfully.");
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        // Student: Get my results
        [HttpGet("my-results")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> GetMyResults()
        {
            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
            var student = await _context.Students
                .FirstOrDefaultAsync(s => s.User.Email == userEmail);

            if (student == null)
                return NotFound("Student not found.");

            var results = await _examResultService.GetByStudentIdAsync(student.UId);
            return Ok(results);
        }
    }
}
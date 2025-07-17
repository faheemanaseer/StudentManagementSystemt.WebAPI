using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentManagement.Business.DTOs;
using StudentManagement.Business.Interfaces;


namespace StudentManagement.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class AttendanceController : ControllerBase
    {
        private readonly IAttendanceeService _attendanceService;

        public AttendanceController(IAttendanceeService attendanceService)
        {
            _attendanceService = attendanceService;
        }

       
        [HttpGet]
        public async Task<IActionResult> GetAttendance([FromQuery] int courseId, [FromQuery] DateTime? date)
        {
            if (!date.HasValue)
                date = DateTime.Today;

            var result = await _attendanceService.GetAttendanceAsync(courseId, date.Value);
            return Ok(result);
        }

       
        [HttpPost("save")]
        public async Task<IActionResult> SaveAttendance([FromBody] List<AttendanceeDto> attendanceList, [FromQuery] DateTime date)
        {
            if (attendanceList == null || !attendanceList.Any())
                return BadRequest(new { message = "No attendance data received." });

            foreach (var a in attendanceList)
                a.Date = date;

            int courseId = attendanceList[0].CourseId;

            await _attendanceService.DeleteExistingRecordsAsync(courseId, date);
            await _attendanceService.SaveAttendanceAsync(attendanceList);

            return Ok(new { message = "Attendance saved successfully." });
        }
    }
}

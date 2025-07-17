using StudentManagement.Business.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.Business.Interfaces
{
    public interface IAttendanceeService
    {
        Task DeleteExistingRecordsAsync(int courseId, DateTime date);
        Task<List<AttendanceeDto>> GetAttendanceAsync(int courseId, DateTime date);
        Task SaveAttendanceAsync(List<AttendanceeDto> attendanceList);

    }
}

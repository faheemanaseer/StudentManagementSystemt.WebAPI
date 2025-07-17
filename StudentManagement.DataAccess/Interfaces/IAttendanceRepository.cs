using StudentManagement.Application.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.DataAccess.Interfaces
{
    public interface IAttendanceeRepository
    {
        Task<List<Attendance>> GetByCourseAndDateAsync(int courseId, DateTime date);
        Task AddRangeAsync(List<Attendance> attendances);
        Task AddAsync(Attendance attendance);

        Task UpdateAsync(Attendance attendance);
        Task<Attendance> GetByCourseAndDateAndStudentAsync(int courseId, DateTime date, int studentId);
        Task DeleteByCourseAndDateAsync(int courseId, DateTime date);
    }
}

using StudentManagement.Application.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.DataAccess.Interfaces
{
    public interface IStudentCourseRepository
    {
        Task AddAsync(StudentCourse studentCourse);
        Task<List<StudentCourse>> GetByStudentIdAsync(int studentId);
        Task<StudentCourse?> GetByStudentAndCourseAsync(int studentId, int courseId);
        Task DeleteAsync(StudentCourse studentCourse);
    }
}

using StudentManagement.Business.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.Business.Interfaces
{
    public interface IStudentCourseService
    {
        Task AssignCourseAsync(int studentId, int courseId);
        Task<bool> IsAlreadyEnrolledAsync(int studentId, int courseId);

        Task<List<CourseDto>> GetAllCoursesAsync();
        Task<List<StudentWithCoursesDto>> GetAllStudentsAsync();
        Task<List<CourseDto>> GetAssignedCoursesAsync(int studentId);
    }
}

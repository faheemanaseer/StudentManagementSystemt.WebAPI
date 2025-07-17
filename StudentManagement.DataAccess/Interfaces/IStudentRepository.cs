using StudentManagement.Application.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.DataAccess.Interfaces
{
    public interface IStudentRepository
    {
        Task<Student> GetByUserIdAsync(int userId);
        Task CreateAsync(Student student);
        Task UpdateAsync(Student student);
        Task SaveAsync();
        Task<List<Student>> SearchByNameAsync(string name);

        Task<List<Student>> GetStudentsByCourseId(int courseId);

        Task<List<Student>> GetAllAsync();
        Task<Student?> GetByIdAsync(int id);
        Task<List<Course>> GetEnrolledCoursesAsync(int userId);


    }
}

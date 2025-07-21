using StudentManagement.Business.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.Business.Interfaces
{
    public interface IStudentService
    {
        Task<StudentDto> GetProfileAsync(int userId);
        Task CreateProfileAsync(StudentDto dto, int userId);
        Task UpdateProfileAsync(StudentDto dto, int userId);
        Task<List<CourseDto>> GetEnrolledCoursesAsync(int userId);
        Task<List<StudentDto>> SearchByNameAsync(string name);

        Task<(byte[] fileBytes, string contentType)?> GetCardImageAsync(int userId);

    }
}

using Microsoft.AspNetCore.Mvc.Rendering;
using StudentManagement.Application.Entities;
using StudentManagement.Business.DTOs;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.Business.Interfaces
{
    public interface ICourseService
    {
        Task<List<CourseDto>> GetAllAsync();
        Task<CourseDto> GetByIdAsync(int id);
        Task AddAsync(CourseDto dto);
        Task UpdateAsync(CourseDto dto);
        Task<List<CourseInstructorDto>> GetCoursesWithInstructorsAsync();

        Task<List<SelectListItem>> GetAllStudentsAsync();
        Task<List<SelectListItem>> GetAllCoursesAsync();
        Task AssignCourseAsync(int studentId, int courseId);
        Task DeleteAsync(int id);
        Task AssignInstructorAsync(int courseId, int instructorId);
        Task<List<Instructor>> GetAllInstructorsAsync();

    }
}

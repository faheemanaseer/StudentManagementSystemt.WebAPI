using AutoMapper;
using Microsoft.AspNetCore.Mvc.Rendering;
using StudentManagement.Application.Entities;
using StudentManagement.Business.DTOs;
using StudentManagement.Business.Interfaces;
using StudentManagement.DataAccess.Interfaces;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.Business.Services
{
    public class CourseService : ICourseService
    {
        private readonly ICourseRepository _courseRepo;
        private readonly IStudentRepository _studentRepo;
        private readonly IStudentCourseRepository _studentCourseRepo;
        private readonly IMapper _mapper;
        private readonly IInstructorRepository _instructorRepo;
        public CourseService(ICourseRepository courseRepo, IStudentRepository studentRepo,  IMapper mapper,IStudentCourseRepository studentCourseRepo,IInstructorRepository instructorRepo)
        {
            _courseRepo = courseRepo;
            _studentRepo = studentRepo;
            _studentCourseRepo = studentCourseRepo;
            _mapper = mapper;
            _instructorRepo = instructorRepo;
        }

        public async Task<List<CourseDto>> GetAllAsync()
        {
            var courses = await _courseRepo.GetAllAsync();
            return _mapper.Map<List<CourseDto>>(courses);
        }

        public async Task<CourseDto> GetByIdAsync(int id)
        {
            var c = await _courseRepo.GetByIdAsync(id);
            return _mapper.Map<CourseDto>(c);
        }

        public async Task AddAsync(CourseDto dto)
        {
            var course = _mapper.Map<Course>(dto);
            course.Instructor = null; 
            course.InstructorId = null;
            await _courseRepo.AddAsync(course);
        }

        public async Task UpdateAsync(CourseDto dto)
        {
            var course = await _courseRepo.GetByIdAsync(dto.SId);
            if (course == null) return;

            _mapper.Map(dto, course);

            await _courseRepo.UpdateAsync(course);
        }

        public async Task DeleteAsync(int id)
        {
            var course = await _courseRepo.GetByIdAsync(id);
            await _courseRepo.DeleteAsync(course);
        }
        public async Task<List<SelectListItem>> GetAllStudentsAsync()
        {
            var students = await _studentRepo.GetAllAsync();
            return students.Select(s => new SelectListItem
            {
                Value = s.UId.ToString(),
                Text = s.Name
            }).ToList();
        }

        public async Task<List<SelectListItem>> GetAllCoursesAsync()
        {
            var courses = await _courseRepo.GetAllAsync();
            return courses.Select(c => new SelectListItem
            {
                Value = c.SId.ToString(),
                Text = c.Title,

            }).ToList();
        }

        public async Task AssignCourseAsync(int studentId, int courseId)
        {
            var assignment = new StudentCourse
            {
                StudentId = studentId,
                CourseId = courseId
            };
            await _studentCourseRepo.AddAsync(assignment);
        }
        public async Task AssignInstructorAsync(int courseId, int instructorId)
        {
            var course = await _courseRepo.GetByIdAsync(courseId);
            if (course != null)
            {
                course.InstructorId = instructorId;
                await _courseRepo.UpdateAsync(course);
            }
        }
        public async Task<List<CourseInstructorDto>> GetCoursesWithInstructorsAsync()
        {
            var courses = await _courseRepo.GetAllAsyncWithInstructor();
            var result = courses.Select(c => new CourseInstructorDto
            {
                CourseId = c.SId,
                Title = c.Title,
                InstructorName = c.Instructor != null ? c.Instructor.Name : "Not Assigned"
            }).ToList();

            return result;
        }


        public async Task<List<Instructor>> GetAllInstructorsAsync()
        {
            return await _instructorRepo.GetAllAsync();
        }

    }
}

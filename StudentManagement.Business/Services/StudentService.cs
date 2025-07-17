using AutoMapper;
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
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepo;
        private readonly IStudentCourseRepository _studentCourseRepo;
        private readonly IMapper _mapper;

        public StudentService(IStudentRepository studentRepo, IMapper mapper,IStudentCourseRepository studentCourseRepo)
        {
            _studentRepo = studentRepo;
            _studentCourseRepo = studentCourseRepo;
            _mapper = mapper;
        }

        public async Task<StudentDto> GetProfileAsync(int userId)
        {
            var student = await _studentRepo.GetByUserIdAsync(userId);
            if (student == null)
                return null;

            return _mapper.Map<StudentDto>(student);
        }

        public async Task CreateProfileAsync(StudentDto dto, int userId)
        {
            Console.WriteLine("DTO Email: " + dto.Email);
            var student = _mapper.Map<Student>(dto);
            student.UserId = userId;


            student.Email = dto.Email;

            if (string.IsNullOrEmpty(student.Email))
                throw new ArgumentException("Email must not be null");

            await _studentRepo.CreateAsync(student);
            await _studentRepo.SaveAsync();
        }
        public async Task UpdateProfileAsync(StudentDto dto, int userId)
        {
            var student = await _studentRepo.GetByUserIdAsync(userId);
            if (student == null) return;
            student.Name = dto.Name;
            student.Phone = dto.Phone;
            student.Age = dto.Age;



            await _studentRepo.UpdateAsync(student);
            await _studentRepo.SaveAsync();
        }
        public async Task<List<CourseDto>> GetEnrolledCoursesAsync(int userId)
        {
            var courses = await _studentRepo.GetEnrolledCoursesAsync(userId);

            return courses.Select(c => new CourseDto
            {
                SId = c.SId,
                Title = c.Title,
                InstructorName = c.Instructor != null ? c.Instructor.Name : "Not Assigned"
            }).ToList();
        }

        public async Task AssignCourseToStudentAsync(int studentId, int courseId)
        {
            var existing = await _studentCourseRepo.GetByStudentAndCourseAsync(studentId, courseId);
            if (existing != null) return;

            var newAssignment = new StudentCourse
            {
                StudentId = studentId,
                CourseId = courseId
            };

            await _studentCourseRepo.AddAsync(newAssignment);
        }
        public async Task<List<StudentDto>> SearchByNameAsync(string name)
        {
            var students = await _studentRepo.SearchByNameAsync(name);

            return students
                .Select(s => new StudentDto
                {
                    UId = s.UId,
                    Name = s.Name,
                    Email = s.Email,
                    Phone = s.Phone,
                    Age = s.Age
                })
                .ToList();
        }
    }
}

using AutoMapper;
using Microsoft.AspNetCore.Http;
using StudentManagement.Application.Entities;
using StudentManagement.Business.DTOs;
using StudentManagement.Business.Interfaces;
using StudentManagement.DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
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
        private readonly string _ImageFolderPath;

        public StudentService(IStudentRepository studentRepo, IMapper mapper,IStudentCourseRepository studentCourseRepo)
        {
            _studentRepo = studentRepo;
            _studentCourseRepo = studentCourseRepo;
            _mapper = mapper;
            _ImageFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "Images");

            if (!Directory.Exists(_ImageFolderPath))
            {
                Directory.CreateDirectory(_ImageFolderPath);
            }
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
            if(dto.CardImage  == null)
            {
                throw new ArgumentException("Student Card Image is Required");
            }

            //Console.WriteLine("DTO Email: " + dto.Email);
            var student = _mapper.Map<Student>(dto);
            student.UserId = userId;
            student.Email = dto.Email;
            if (string.IsNullOrEmpty(student.Email))
                throw new ArgumentException("Email must not be null");


            student.CardImagePath = await SaveImageAsync(dto.CardImage);
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

            if(dto.CardImage != null)
            {
                if (!string.IsNullOrEmpty(student.CardImagePath))
                {
                    var oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), student.CardImagePath);
                    if (File.Exists(oldImagePath))
                    {
                        File.Delete(oldImagePath);
                    }
                }
                
                student.CardImagePath = await SaveImageAsync(dto.CardImage);
            }


            await _studentRepo.UpdateAsync(student);
            await _studentRepo.SaveAsync();
        }

        private async Task<string> SaveImageAsync(IFormFile image)
        {
            if (image == null || image.Length == 0)
                throw new ArgumentException("Invalid image file");

            
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
            var extension = Path.GetExtension(image.FileName).ToLower();
            if (!allowedExtensions.Contains(extension))
                throw new ArgumentException("Only .jpg, .jpeg, and .png files are allowed");

            
            var fileName = $"{Guid.NewGuid()}{extension}";
            var filePath = Path.Combine(_ImageFolderPath, fileName);

            
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(stream);
            }

            
            return Path.Combine("Images", fileName).Replace("\\", "/");
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


        public async Task<(byte[] fileBytes, string contentType)?> GetCardImageAsync(int userId)
        {
            var student = await _studentRepo.GetByUserIdAsync(userId); 
            if (student == null || string.IsNullOrEmpty(student.CardImagePath))
                return null;

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), student.CardImagePath);
            if (!File.Exists(filePath))
                return null;

            var fileBytes = await File.ReadAllBytesAsync(filePath);
            var extension = Path.GetExtension(filePath).ToLower();
            var contentType = extension switch
            {
                ".jpg" => "image/jpeg",
                ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                _ => "application/octet-stream"
            };

            return (fileBytes, contentType);
        }
    }
}

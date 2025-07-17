using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StudentManagement.Application.Entities;
using StudentManagement.Business.DTOs;
using StudentManagement.Business.Interfaces;
using StudentManagement.DataAccess.Data;
using StudentManagement.DataAccess.Interfaces;

namespace StudentManagement.Business.Services
{
    public class ExamResultService : IExamResultService
    {
        private readonly IExamResultRepository _examResultRepository;
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _context;

        public ExamResultService(IExamResultRepository examResultRepository, IMapper mapper , ApplicationDbContext context)
        {
            _examResultRepository = examResultRepository;
            _mapper = mapper;
            _context = context;
        }

        public async Task AddExamResultAsync(CreateExamResultDto dto)
        {
            if (await _examResultRepository.ExistsAsync(dto.StudentId, dto.CourseId))
                throw new Exception("Student already has a result for this course.");

            var isEnrolled = await _context.StudentCourses.AnyAsync(sc => sc.StudentId == dto.StudentId && sc.CourseId == dto.CourseId);
            if (!isEnrolled)
                throw new Exception("Student is not enrolled in the selected course.");

            var examResult = _mapper.Map<ExamResult>(dto);
            examResult.Grade = CalculateGrade(dto.Marks);

            await _examResultRepository.AddAsync(examResult);
        }

        public async Task<ExamResultDto> GetByIdAsync(int id)
        {
            var examResult = await _examResultRepository.GetByIdAsync(id);
            if (examResult == null)
                throw new Exception("Exam result not found.");
            return _mapper.Map<ExamResultDto>(examResult);
        }

        public async Task<List<ExamResultDto>> GetByCourseIdAsync(int courseId)
        {
            var results = await _examResultRepository.GetByCourseIdAsync(courseId);
            return _mapper.Map<List<ExamResultDto>>(results);
        }

        public async Task<List<ExamResultDto>> GetByStudentIdAsync(int studentId)
        {
            var results = await _examResultRepository.GetByStudentIdAsync(studentId);
            return _mapper.Map<List<ExamResultDto>>(results);
        }

        public async Task<List<ExamResultDto>> GetAllAsync()
        {
            var results = await _examResultRepository.GetAllAsync();
            return _mapper.Map<List<ExamResultDto>>(results);
        }

        public async Task<List<ExamResultDto>> SearchByStudentNameAsync(string query, int? courseId)
        {
            var results = await _examResultRepository.SearchByStudentNameAsync(query, courseId);
            return _mapper.Map<List<ExamResultDto>>(results);
        }

        public async Task UpdateExamResultAsync(int id, CreateExamResultDto dto)
        {
            var examResult = await _examResultRepository.GetByIdAsync(id);
            if (examResult == null)
                throw new Exception("Exam result not found.");

            _mapper.Map(dto, examResult);
            examResult.Grade = CalculateGrade(dto.Marks);
            await _examResultRepository.UpdateAsync(examResult);
        }

        public async Task DeleteExamResultAsync(int id)
        {
            var examResult = await _examResultRepository.GetByIdAsync(id);
            if (examResult == null)
                throw new Exception("Exam result not found.");
            await _examResultRepository.DeleteAsync(id);
        }

        private static string CalculateGrade(double marks)
        {
            if (marks >= 85) return "A";
            if (marks >= 70) return "B";
            if (marks > 50) return "C";
            return "F";
        }
    }
}
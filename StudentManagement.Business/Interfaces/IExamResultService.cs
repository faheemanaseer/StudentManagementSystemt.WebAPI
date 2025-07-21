using StudentManagement.Business.DTOs;

namespace StudentManagement.Business.Interfaces
{
    public interface IExamResultService
    {
        Task AddExamResultAsync(CreateExamResultDto dto);
        Task<ExamResultDto> GetByIdAsync(int id);
        Task<List<ExamResultDto>> GetByCourseIdAsync(int courseId);
        Task<List<ExamResultDto>> GetByStudentIdAsync(int studentId);
        Task<List<ExamResultDto>> GetAllAsync();
        Task<List<ExamResultDto>> SearchByStudentNameAsync(string query, int? courseId);
        Task UpdateExamResultAsync(int id, CreateExamResultDto dto);
        Task DeleteExamResultAsync(int id);
    }
}
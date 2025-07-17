
using StudentManagement.Application.Entities;

namespace StudentManagement.DataAccess.Interfaces
{
    public interface IExamResultRepository
    {
        Task AddAsync(ExamResult examResult);
        Task<ExamResult> GetByIdAsync(int id);
        Task<List<ExamResult>> GetByCourseIdAsync(int courseId);
        Task<List<ExamResult>> GetByStudentIdAsync(int studentId);
        Task<List<ExamResult>> GetAllAsync();
        Task<List<ExamResult>> SearchByStudentNameAsync(string query, int? courseId);
        Task UpdateAsync(ExamResult examResult);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int studentId, int courseId);
    }
}
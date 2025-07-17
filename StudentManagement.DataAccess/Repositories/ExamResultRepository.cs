using Microsoft.EntityFrameworkCore;
using StudentManagement.Application.Entities;
using StudentManagement.DataAccess.Data;
using StudentManagement.DataAccess.Interfaces;



namespace StudentManagement.DataAccess.Repositories
{
    public class ExamResultRepository : IExamResultRepository
    {
        private readonly ApplicationDbContext _context;

        public ExamResultRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(ExamResult examResult)
        {
            _context.ExamResult.Add(examResult);
            await _context.SaveChangesAsync();
        }

        public async Task<ExamResult> GetByIdAsync(int id)
        {
            return await _context.ExamResult
                .Include(er => er.Student)
                .Include(er => er.Course)
                .FirstOrDefaultAsync(er => er.Id == id);
        }

        public async Task<List<ExamResult>> GetByCourseIdAsync(int courseId)
        {
            return await _context.ExamResult
                .Include(er => er.Student)
                .Include(er => er.Course)
                .Where(er => er.CourseId == courseId)
                .ToListAsync();
        }

        public async Task<List<ExamResult>> GetByStudentIdAsync(int studentId)
        {
            return await _context.ExamResult
                .Include(er => er.Student)
                .Include(er => er.Course)
                .Where(er => er.StudentId == studentId)
                .ToListAsync();
        }

        public async Task<List<ExamResult>> GetAllAsync()
        {
            return await _context.ExamResult
                .Include(er => er.Student)
                .Include(er => er.Course)
                .ToListAsync();
        }

        public async Task<List<ExamResult>> SearchByStudentNameAsync(string query, int? courseId)
        {
            var results = _context.ExamResult
                .Include(er => er.Student)
                .Include(er => er.Course)
                .Where(er => er.Student.Name.Contains(query));

            if (courseId.HasValue)
                results = results.Where(er => er.CourseId == courseId.Value);

            return await results.ToListAsync();
        }

        public async Task UpdateAsync(ExamResult examResult)
        {
            _context.ExamResult.Update(examResult);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var examResult = await _context.ExamResult.FindAsync(id);
            if (examResult != null)
            {
                _context.ExamResult.Remove(examResult);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int studentId, int courseId)
        {
            return await _context.ExamResult.AnyAsync(er => er.StudentId == studentId && er.CourseId == courseId);
        }
    }
}
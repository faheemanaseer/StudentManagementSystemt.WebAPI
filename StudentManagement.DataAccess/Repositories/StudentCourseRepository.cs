using Microsoft.EntityFrameworkCore;
using StudentManagement.Application.Entities;
using StudentManagement.DataAccess.Data;
using StudentManagement.DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.DataAccess.Repositories
{
    public class StudentCourseRepository : IStudentCourseRepository
    {
        private readonly ApplicationDbContext _context;

        public StudentCourseRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(StudentCourse studentCourse)
        {
            _context.StudentCourses.Add(studentCourse);
            await _context.SaveChangesAsync();
        }

        public async Task<List<StudentCourse>> GetByStudentIdAsync(int studentId)
        {
            return await _context.StudentCourses
                .Include(sc => sc.Course)
                .Where(sc => sc.StudentId == studentId)
                .ToListAsync();
        }

        public async Task<StudentCourse?> GetByStudentAndCourseAsync(int studentId, int courseId)
        {
            return await _context.StudentCourses
                .FirstOrDefaultAsync(sc => sc.StudentId == studentId && sc.CourseId == courseId);
        }

        public async Task DeleteAsync(StudentCourse studentCourse)
        {
            _context.StudentCourses.Remove(studentCourse);
            await _context.SaveChangesAsync();
        }
    }
}

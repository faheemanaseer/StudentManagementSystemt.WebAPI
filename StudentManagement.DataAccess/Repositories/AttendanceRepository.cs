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
    public class AttendanceeRepository : IAttendanceeRepository
    {
        private readonly ApplicationDbContext _context;
        public AttendanceeRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<Attendance>> GetByCourseAndDateAsync(int courseId, DateTime date)
        {
            return await _context.Attendances
                .Include(a => a.Student)
                .Where(a => a.CourseId == courseId && a.Date == date)
                .ToListAsync();
        }
        public async Task AddRangeAsync(List<Attendance> attendances)
        {
            _context.Attendances.AddRange(attendances);
            await _context.SaveChangesAsync();
        }
        public async Task<Attendance?> GetByCourseAndDateAndStudentAsync(int courseId, DateTime date, int studentId)
        {
            return await _context.Attendances
                .FirstOrDefaultAsync(a =>
                    a.CourseId == courseId &&
                    a.Date.Date == date.Date &&
                    a.StudentId == studentId);
        }
        public async Task AddAsync(Attendance attendance)
        {
            await _context.Attendances.AddAsync(attendance);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(Attendance attendance)
        {
            _context.Attendances.Update(attendance);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteByCourseAndDateAsync(int courseId, DateTime date)
        {
            var records = _context.Attendances
                .Where(a => a.CourseId == courseId && a.Date == date);

            _context.Attendances.RemoveRange(records);
            await _context.SaveChangesAsync();
        }
    }
}

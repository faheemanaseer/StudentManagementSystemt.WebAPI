using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using StudentManagement.Application.Entities;
using StudentManagement.DataAccess.Data;
using StudentManagement.DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.DataAccess.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        public StudentRepository(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        private IDbConnection Connection => new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        public async Task<Student> GetByUserIdAsync(int userId)
        {
            return await _context.Students
                .Include(s => s.StudentCourses)
                .ThenInclude(sc => sc.Course)
                .FirstOrDefaultAsync(s => s.UserId == userId);
        }
        public async Task CreateAsync(Student student)
        {
            await _context.Students.AddAsync(student);
        }
        public async Task UpdateAsync(Student student)
        {
            _context.Students.Update(student);
        }
        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
        public async Task<List<Student>> GetStudentsByCourseId(int courseId)
        {
            return await _context.StudentCourses
                .Where(sc => sc.CourseId == courseId)
                .Include(sc => sc.Student)
                .Select(sc => sc.Student)
                .ToListAsync();
        }
        public async Task<List<Student>> SearchByNameAsync(string name)
        {
            using var db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));

            var param = new { Name = name };

            var result = await db.QueryAsync<Student>(
                "SearchStudentsByName",
                param,
                commandType: CommandType.StoredProcedure
            );

            return result.ToList();
        }
        public async Task<List<Student>> GetAllAsync()
        {
            using var db = Connection;
            var result = await db.QueryAsync<Student>("GetAllStudents", commandType: CommandType.StoredProcedure);

            return result.ToList();
        }
        public async Task<Student?> GetByIdAsync(int id)
        {
            return await _context.Students.FindAsync(id);
        }
        public async Task<List<Course>> GetEnrolledCoursesAsync(int userId)
        {
            return await _context.StudentCourses
                .Where(sc => sc.Student.UserId == userId)
                .Include(sc => sc.Course)
                    .ThenInclude(c => c.Instructor)
                .Select(sc => sc.Course)
                .ToListAsync();
        }
    }
}

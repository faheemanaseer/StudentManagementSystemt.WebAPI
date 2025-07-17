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
    public class CourseRepository : ICourseRepository
    {
        private readonly ApplicationDbContext _context;

        public CourseRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Course>> GetAllAsync()
            => await _context.Courses.ToListAsync();

        public async Task<Course> GetByIdAsync(int id)
            => await _context.Courses.FindAsync(id);


        public async Task AddAsync(Course course)
        {
            if (course.Instructor != null && string.IsNullOrEmpty(course.Instructor.Email))
            {

                course.Instructor = null;
            }

            _context.Courses.Add(course);
            await _context.SaveChangesAsync();
        }


        public async Task UpdateAsync(Course course)
        {
            _context.Courses.Update(course);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Course course)
        {
            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();
        }
        public async Task<List<Course>> GetAllAsyncWithInstructor()
        {
            return await _context.Courses
                .Include(c => c.Instructor)
                .ToListAsync();
        }

    }
}

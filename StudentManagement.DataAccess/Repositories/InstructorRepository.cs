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
    public class InstructorRepository : IInstructorRepository
    {
        private readonly ApplicationDbContext _context;
        public InstructorRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Instructor>> GetAllAsync() => await _context.Instructors.ToListAsync();
        public async Task<Instructor> GetByIdAsync(int id) => await _context.Instructors.FindAsync(id);
        public async Task AddAsync(Instructor instructor)
        {
            _context.Instructors.Add(instructor);
            await _context.SaveChangesAsync();
        }
    }
}

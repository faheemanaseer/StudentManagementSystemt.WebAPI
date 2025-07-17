using StudentManagement.Application.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.DataAccess.Interfaces
{
    public interface IInstructorRepository
    {
        Task<List<Instructor>> GetAllAsync();
        Task<Instructor> GetByIdAsync(int id);
        Task AddAsync(Instructor instructor);
    }
}

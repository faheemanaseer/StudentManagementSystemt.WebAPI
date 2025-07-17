using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentManagement.Application.Entities;

namespace StudentManagement.Business.Interfaces
{
   public interface IUserService
   {
        Task RegisterAsync(User user);

        Task<string?> LoginAsync(string email, string password);
        Task<string?> GetRoleNameByIdAsync(int roleId);

        Task<User> GetByEmailAsync(string email);

    }
}

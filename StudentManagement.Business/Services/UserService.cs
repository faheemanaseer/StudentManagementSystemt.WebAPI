//using StudentManagement.Application.Entities;
//using StudentManagement.Business.Interfaces;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using StudentManagement.DataAccess.Data;
//using Microsoft.EntityFrameworkCore;
//namespace StudentManagement.Business.Services
//{
//    public class UserService : IUserService
//    {
//        private readonly ApplicationDbContext _context;

//        public UserService(ApplicationDbContext context)
//        {
//            _context = context;
//        }
//        public async Task<string?> GetRoleNameByIdAsync(int roleId)
//        {
//            var role = await _context.Roles.FirstOrDefaultAsync(r => r.Id == roleId);
//            return role?.Name;
//        }

//        //public async Task RegisterAsync(User user)
//        //{
//        //    var existing = await _context.Users.AnyAsync(u => u.Email == user.Email);
//        //    if (existing)
//        //        throw new Exception("User already exists.");

//        //    _context.Users.Add(user);
//        //    await _context.SaveChangesAsync();
//        //}
//        public async Task RegisterAsync(User user)
//        {
//            var existing = await _context.Users.AnyAsync(u => u.Email == user.Email);
//            if (existing)
//                throw new Exception("User already exists.");

//            try
//            {
//                _context.Users.Add(user);
//                await _context.SaveChangesAsync();
//            }
//            catch (Exception ex)
//            {
//                throw new Exception("EF Save Error: " + ex.Message + " — Inner: " + ex.InnerException?.Message);
//            }
//        }

//        public async Task<User> LoginAsync(string email, string password)
//        {
//            return await _context.Users
//                .Include(u => u.Role)
//                .FirstOrDefaultAsync(u => u.Email == email && u.Password == password);
//        }

//        public async Task<User> GetByEmailAsync(string email)
//        {
//            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
//        }

//    }
//}
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using StudentManagement.Application.Entities;
using StudentManagement.Business.Interfaces;
using StudentManagement.DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace StudentManagement.Business.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _config;

        public UserService(ApplicationDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        public async Task<string?> GetRoleNameByIdAsync(int roleId)
        {
            var role = await _context.Roles.FirstOrDefaultAsync(r => r.Id == roleId);
            return role?.Name;
        }

        public async Task RegisterAsync(User user)
        {
            var existing = await _context.Users.AnyAsync(u => u.Email == user.Email);
            if (existing)
                throw new Exception("User already exists.");

            
            user.RoleId = null;

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }



        public async Task<string?> LoginAsync(string email, string password)
        {
            var user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Email == email && u.Password == password);

            if (user == null)
                return null;

            return GenerateJwtToken(user);
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        private string GenerateJwtToken(User user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role?.Name ?? "Pending"),
                new Claim(ClaimTypes.Email, user.Email)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

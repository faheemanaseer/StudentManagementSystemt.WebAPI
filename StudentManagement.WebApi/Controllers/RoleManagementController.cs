using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentManagement.Application.Entities;
using StudentManagement.DataAccess.Data;


namespace StudentManagement.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    [Authorize(Roles ="SuperAdmin")]
    public class RoleManagementController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public RoleManagementController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet("users-roles")]
        public async Task<ActionResult<RoleAssignViewModel>> GetUsersWithRoles()
        {
            var users = await _context.Users.ToListAsync();
            var roles = await _context.Roles
                .Select(r => new Role { Id = r.Id, Name = r.Name })
                .ToListAsync();

            return Ok(new RoleAssignViewModel
            {
                Users = users,
                Roles = roles
            });
        }
        [HttpPost("assign")]
        public async Task<IActionResult> AssignRole([FromBody] AssignRoleDto dto)
        {
            var user = await _context.Users.FindAsync(dto.UserId);
            if (user == null)
                return NotFound(new { message = "User not found." });

            var role = await _context.Roles.FindAsync(dto.RoleId);
            if (role == null)
                return BadRequest(new { message = "Invalid RoleId." });

            user.RoleId = dto.RoleId;
            await _context.SaveChangesAsync();

            return Ok(new { message = "Role assigned successfully." });
        }
        public class RoleAssignViewModel
        {
            public List<User> Users { get; set; }
            public List<Role> Roles { get; set; }
        }
        public class AssignRoleDto
        {
            public int UserId { get; set; }
            public int RoleId { get; set; }
        }
    }
}

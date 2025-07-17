using Microsoft.AspNetCore.Mvc;
using StudentManagement.Application.Entities;
using StudentManagement.Business.Interfaces;
using System.Threading.Tasks;

namespace StudentManagement.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] User user)
        {
            try
            {
                await _userService.RegisterAsync(user);
                return Ok(new { message = "User registered successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
        


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest model)
        {
            var token = await _userService.LoginAsync(model.Email, model.Password);

            if (token == null)
                return Unauthorized(new { message = "Invalid credentials." });

            return Ok(new { token });
        }

       // [HttpGet("role/{id}")]
       // public async Task<IActionResult> GetRoleName(int id)
       // {
       //     var roleName = await _userService.GetRoleNameByIdAsync(id);
       //     if (roleName == null)
       //         return NotFound(new { message = "Role not found." });

       //     return Ok(new { role = roleName });
       // }


       //[HttpGet("email/{email}")]
       // public async Task<IActionResult> GetByEmail(string email)
       // {
       //     var user = await _userService.GetByEmailAsync(email);
       //     if (user == null)
       //         return NotFound(new { message = "User not found." });

       //     return Ok(user);
       // }
    }
    public class LoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}

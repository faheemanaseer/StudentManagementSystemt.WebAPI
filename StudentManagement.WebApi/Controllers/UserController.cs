using Microsoft.AspNetCore.Mvc;
using StudentManagement.Application.Entities;
using StudentManagement.Business.Interfaces;
using StudentManagement.Business.DTOs;
using System.Threading.Tasks;
using AutoMapper;

namespace StudentManagement.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UsersController(IUserService userService,IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            try
            {
                var user = _mapper.Map<User>(dto); 
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
    }
    public class LoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}

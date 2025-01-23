using Microsoft.AspNetCore.Mvc;
using EducationPlatformAPI.Data;
using EducationPlatformAPI.Models;

namespace EducationPlatformAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly EducationPlatformContext _context;

        public AuthController(EducationPlatformContext context)
        {
            _context = context;
        }

        // Авторизация пользователя
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            var user = _context.Users.FirstOrDefault(u => u.Login == request.Login && u.Password == request.Password);

            if (user == null)
                return Unauthorized("Invalid login or password");

            return Ok(new
            {
                user.UserID,
                user.FirstName,
                user.LastName,
                user.Role
            });
        }

        // Выход (опционально)
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            // Если на сервере нужно делать логику выхода, можно добавить здесь
            return Ok("Logout successful");
        }
    }

    public class LoginRequest
    {
        public string Login { get; set; }
        public string Password { get; set; }
    }
}

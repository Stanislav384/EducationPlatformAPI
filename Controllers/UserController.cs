using Microsoft.AspNetCore.Mvc;
using EducationPlatformAPI.Data;
using EducationPlatformAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace EducationPlatformAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly EducationPlatformContext _context;

        public UserController(EducationPlatformContext context)
        {
            _context = context;
        }

        // Получить всех пользователей
        [HttpGet]
        public IActionResult GetAllUsers()
        {
            var users = _context.Users.ToList();
            return Ok(users);
        }

        // Получить пользователя по ID
        [HttpGet("{id}")]
        public IActionResult GetUserById(int id)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserID == id);
            if (user == null)
                return NotFound("User not found");
            return Ok(user);
        }

        // Добавить ученика
        [HttpPost("add-student")]
        public IActionResult AddStudent([FromBody] AddStudentRequest request)
        {
            // Генерация логина и пароля
            var login = GenerateLogin(request.FirstName, request.LastName);
            var password = GeneratePassword();

            var student = new User
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Role = "student",
                Login = login,
                Password = password,
                CreatedAt = DateTime.Now
            };

            _context.Users.Add(student);
            _context.SaveChanges();

            return Ok(new { Login = login, Password = password });
        }

        // Добавить учителя
        [HttpPost("add-teacher")]
        public IActionResult AddTeacher([FromBody] AddTeacherRequest request)
        {
            // Генерация логина и пароля
            var login = GenerateLogin(request.FirstName, request.LastName);
            var password = GeneratePassword();

            var teacher = new User
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Role = "teacher",
                Login = login,
                Password = password,
                CreatedAt = DateTime.Now
            };

            _context.Users.Add(teacher);
            _context.SaveChanges();

            return Ok(new { Login = login, Password = password });
        }

        // Редактировать пользователя
        [HttpPut("{id}")]
        public IActionResult UpdateUser(int id, [FromBody] UpdateUserRequest request)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserID == id);
            if (user == null)
                return NotFound("User not found");

            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.Role = request.Role;

            _context.SaveChanges();
            return NoContent();
        }

        // Удалить пользователя
        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserID == id);
            if (user == null)
                return NotFound("User not found");

            _context.Users.Remove(user);
            _context.SaveChanges();
            return NoContent();
        }

        // Получить пользователей по роли
        [HttpGet("role/{role}")]
        public IActionResult GetUsersByRole(string role)
        {
            var users = _context.Users.Where(u => u.Role == role).ToList();
            return Ok(users);
        }

        // Генерация логина
        private string GenerateLogin(string firstName, string lastName)
        {
            return Transliterate(firstName + lastName);
        }

        // Генерация случайного пароля
        private string GeneratePassword()
        {
            var random = new Random();
            return random.Next(1000, 9999).ToString();
        }

        // Транслитерация для генерации логина
        private string Transliterate(string input)
        {
            var translitMap = new Dictionary<char, string>
            {
                {'а', "a"}, {'б', "b"}, {'в', "v"}, {'г', "g"}, {'д', "d"},
                {'е', "e"}, {'ё', "yo"}, {'ж', "zh"}, {'з', "z"}, {'и', "i"},
                {'й', "y"}, {'к', "k"}, {'л', "l"}, {'м', "m"}, {'н', "n"},
                {'о', "o"}, {'п', "p"}, {'р', "r"}, {'с', "s"}, {'т', "t"},
                {'у', "u"}, {'ф', "f"}, {'х', "kh"}, {'ц', "ts"}, {'ч', "ch"},
                {'ш', "sh"}, {'щ', "shch"}, {'ы', "y"}, {'э', "e"}, {'ю', "yu"},
                {'я', "ya"}
            };

            var result = new StringBuilder();
            foreach (var c in input.ToLower())
            {
                result.Append(translitMap.ContainsKey(c) ? translitMap[c] : c.ToString());
            }
            return result.ToString();
        }
    }

    // Запросы для добавления и обновления
    public class AddStudentRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    public class AddTeacherRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    public class UpdateUserRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Role { get; set; }
    }
}

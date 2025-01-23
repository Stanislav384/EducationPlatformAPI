using Microsoft.AspNetCore.Mvc;
using EducationPlatformAPI.Data;
using EducationPlatformAPI.Models;
using System.Linq;

namespace EducationPlatformAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly EducationPlatformContext _context;

        public CourseController(EducationPlatformContext context)
        {
            _context = context;
        }

        // Получить все курсы
        [HttpGet]
        public IActionResult GetAllCourses()
        {
            var courses = _context.Courses.ToList();
            return Ok(courses);
        }

        // Получить курс по ID
        [HttpGet("{id}")]
        public IActionResult GetCourseById(int id)
        {
            var course = _context.Courses.FirstOrDefault(c => c.CourseID == id);
            if (course == null)
                return NotFound("Course not found");
            return Ok(course);
        }

        // Создать новый курс
        [HttpPost]
        public IActionResult CreateCourse([FromBody] CreateCourseRequest request)
        {
            var course = new Course
            {
                CourseName = request.CourseName,
                Description = request.Description,
                CreatedAt = DateTime.Now
            };

            _context.Courses.Add(course);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetCourseById), new { id = course.CourseID }, course);
        }

        // Обновить курс
        [HttpPut("{id}")]
        public IActionResult UpdateCourse(int id, [FromBody] UpdateCourseRequest request)
        {
            var course = _context.Courses.FirstOrDefault(c => c.CourseID == id);
            if (course == null)
                return NotFound("Course not found");

            course.CourseName = request.CourseName;
            course.Description = request.Description;

            _context.SaveChanges();
            return NoContent();
        }

        // Удалить курс
        [HttpDelete("{id}")]
        public IActionResult DeleteCourse(int id)
        {
            var course = _context.Courses.FirstOrDefault(c => c.CourseID == id);
            if (course == null)
                return NotFound("Course not found");

            _context.Courses.Remove(course);
            _context.SaveChanges();
            return NoContent();
        }
    }

    // Запросы для создания и обновления курсов
    public class CreateCourseRequest
    {
        public string CourseName { get; set; }
        public string Description { get; set; }
    }

    public class UpdateCourseRequest
    {
        public string CourseName { get; set; }
        public string Description { get; set; }
    }
}

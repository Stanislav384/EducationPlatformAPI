using Microsoft.AspNetCore.Mvc;
using EducationPlatformAPI.Data;
using EducationPlatformAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace EducationPlatformAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LessonController : ControllerBase
    {
        private readonly EducationPlatformContext _context;

        public LessonController(EducationPlatformContext context)
        {
            _context = context;
        }

        // Получить все уроки
        [HttpGet]
        public IActionResult GetAllLessons()
        {
            var lessons = _context.Lessons.Include(l => l.Course).ToList();
            return Ok(lessons);
        }

        // Получить урок по ID
        [HttpGet("{id}")]
        public IActionResult GetLessonById(int id)
        {
            var lesson = _context.Lessons.Include(l => l.Course).FirstOrDefault(l => l.LessonID == id);
            if (lesson == null)
                return NotFound("Lesson not found");
            return Ok(lesson);
        }

        // Получить уроки по курсу
        [HttpGet("course/{courseId}")]
        public IActionResult GetLessonsByCourse(int courseId)
        {
            var lessons = _context.Lessons.Where(l => l.CourseID == courseId).ToList();
            return Ok(lessons);
        }

        // Создать новый урок
        [HttpPost]
        public IActionResult CreateLesson([FromBody] CreateLessonRequest request)
        {
            var lesson = new Lesson
            {
                CourseID = request.CourseID,
                LessonName = request.LessonName,
                LessonContent = request.LessonContent,
                CreatedAt = DateTime.Now
            };

            _context.Lessons.Add(lesson);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetLessonById), new { id = lesson.LessonID }, lesson);
        }

        // Обновить урок
        [HttpPut("{id}")]
        public IActionResult UpdateLesson(int id, [FromBody] UpdateLessonRequest request)
        {
            var lesson = _context.Lessons.FirstOrDefault(l => l.LessonID == id);
            if (lesson == null)
                return NotFound("Lesson not found");

            lesson.LessonName = request.LessonName;
            lesson.LessonContent = request.LessonContent;

            _context.SaveChanges();
            return NoContent();
        }

        // Удалить урок
        [HttpDelete("{id}")]
        public IActionResult DeleteLesson(int id)
        {
            var lesson = _context.Lessons.FirstOrDefault(l => l.LessonID == id);
            if (lesson == null)
                return NotFound("Lesson not found");

            _context.Lessons.Remove(lesson);
            _context.SaveChanges();
            return NoContent();
        }
    }

    // Запросы для создания и обновления уроков
    public class CreateLessonRequest
    {
        public int CourseID { get; set; }
        public string LessonName { get; set; }
        public string LessonContent { get; set; }
    }

    public class UpdateLessonRequest
    {
        public string LessonName { get; set; }
        public string LessonContent { get; set; }
    }
}

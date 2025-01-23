using Microsoft.AspNetCore.Mvc;
using EducationPlatformAPI.Data;
using EducationPlatformAPI.Models;
using System.Linq;

namespace EducationPlatformAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttendanceController : ControllerBase
    {
        private readonly EducationPlatformContext _context;

        public AttendanceController(EducationPlatformContext context)
        {
            _context = context;
        }

        // Получить всю посещаемость
        [HttpGet]
        public IActionResult GetAllAttendance()
        {
            var attendance = _context.Attendance.ToList();
            return Ok(attendance);
        }

        // Получить посещаемость по уроку
        [HttpGet("lesson/{lessonId}")]
        public IActionResult GetAttendanceByLesson(int lessonId)
        {
            var attendance = _context.Attendance.Where(a => a.LessonID == lessonId).ToList();
            return Ok(attendance);
        }

        // Получить посещаемость ученика
        [HttpGet("student/{studentId}")]
        public IActionResult GetAttendanceByStudent(int studentId)
        {
            var attendance = _context.Attendance.Where(a => a.StudentID == studentId).ToList();
            return Ok(attendance);
        }

        // Добавить запись о посещаемости
        [HttpPost]
        public IActionResult MarkAttendance([FromBody] MarkAttendanceRequest request)
        {
            var attendance = new Attendance
            {
                LessonID = request.LessonID,
                StudentID = request.StudentID,
                WasPresent = request.WasPresent,
                MarkedAt = DateTime.Now
            };

            _context.Attendance.Add(attendance);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetAttendanceByLesson), new { lessonId = attendance.LessonID }, attendance);
        }

        // Обновить запись о посещаемости
        [HttpPut("{id}")]
        public IActionResult UpdateAttendance(int id, [FromBody] UpdateAttendanceRequest request)
        {
            var attendance = _context.Attendance.FirstOrDefault(a => a.AttendanceID == id);
            if (attendance == null)
                return NotFound("Attendance record not found");

            attendance.WasPresent = request.WasPresent;
            _context.SaveChanges();
            return NoContent();
        }

        // Удалить запись о посещаемости
        [HttpDelete("{id}")]
        public IActionResult DeleteAttendance(int id)
        {
            var attendance = _context.Attendance.FirstOrDefault(a => a.AttendanceID == id);
            if (attendance == null)
                return NotFound("Attendance record not found");

            _context.Attendance.Remove(attendance);
            _context.SaveChanges();
            return NoContent();
        }
    }

    // Запросы для добавления и обновления посещаемости
    public class MarkAttendanceRequest
    {
        public int LessonID { get; set; }
        public int StudentID { get; set; }
        public bool WasPresent { get; set; }
    }

    public class UpdateAttendanceRequest
    {
        public bool WasPresent { get; set; }
    }
}

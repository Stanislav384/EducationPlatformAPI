using Microsoft.AspNetCore.Mvc;
using EducationPlatformAPI.Data;
using EducationPlatformAPI.Models;
using System.Linq;

namespace EducationPlatformAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PerformanceController : ControllerBase
    {
        private readonly EducationPlatformContext _context;

        public PerformanceController(EducationPlatformContext context)
        {
            _context = context;
        }

        // Получить успеваемость всех студентов
        [HttpGet]
        public IActionResult GetAllPerformance()
        {
            var performance = _context.Performance.ToList();
            return Ok(performance);
        }

        // Получить успеваемость студента по ID
        [HttpGet("student/{studentId}")]
        public IActionResult GetPerformanceByStudentId(int studentId)
        {
            var performance = _context.Performance.FirstOrDefault(p => p.StudentID == studentId);
            if (performance == null)
                return NotFound("Performance record not found");

            return Ok(performance);
        }

        // Создать или обновить запись успеваемости
        [HttpPost("update")]
        public IActionResult UpdatePerformance([FromBody] UpdatePerformanceRequest request)
        {
            var performance = _context.Performance.FirstOrDefault(p => p.StudentID == request.StudentID);

            if (performance == null)
            {
                performance = new Performance
                {
                    StudentID = request.StudentID,
                    TotalAssignments = request.TotalAssignments,
                    TotalGrades = request.TotalGrades,
                    AverageGrade = request.AverageGrade
                };

                _context.Performance.Add(performance);
            }
            else
            {
                performance.TotalAssignments = request.TotalAssignments;
                performance.TotalGrades = request.TotalGrades;
                performance.AverageGrade = request.AverageGrade;
            }

            _context.SaveChanges();
            return Ok(performance);
        }

        // Удалить запись успеваемости студента
        [HttpDelete("student/{studentId}")]
        public IActionResult DeletePerformance(int studentId)
        {
            var performance = _context.Performance.FirstOrDefault(p => p.StudentID == studentId);
            if (performance == null)
                return NotFound("Performance record not found");

            _context.Performance.Remove(performance);
            _context.SaveChanges();
            return NoContent();
        }
    }

    // Запрос для обновления успеваемости
    public class UpdatePerformanceRequest
    {
        public int StudentID { get; set; }
        public int TotalAssignments { get; set; }
        public int TotalGrades { get; set; }
        public double AverageGrade { get; set; }
    }
}

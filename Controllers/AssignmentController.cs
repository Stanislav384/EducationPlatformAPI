using Microsoft.AspNetCore.Mvc;
using EducationPlatformAPI.Data;
using EducationPlatformAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace EducationPlatformAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssignmentController : ControllerBase
    {
        private readonly EducationPlatformContext _context;

        public AssignmentController(EducationPlatformContext context)
        {
            _context = context;
        }

        // Получить все задания
        [HttpGet]
        public IActionResult GetAllAssignments()
        {
            var assignments = _context.Assignments
                .Include(a => a.Lesson)
                .Include(a => a.Group)
                .Include(a => a.Student)
                .Include(a => a.Teacher)
                .ToList();
            return Ok(assignments);
        }

        // Получить задание по ID
        [HttpGet("{id}")]
        public IActionResult GetAssignmentById(int id)
        {
            var assignment = _context.Assignments
                .Include(a => a.Lesson)
                .Include(a => a.Group)
                .Include(a => a.Student)
                .Include(a => a.Teacher)
                .FirstOrDefault(a => a.AssignmentID == id);

            if (assignment == null)
                return NotFound("Assignment not found");

            return Ok(assignment);
        }

        // Создать новое задание
        [HttpPost]
        public IActionResult CreateAssignment([FromBody] CreateAssignmentRequest request)
        {
            var assignment = new Assignment
            {
                LessonID = request.LessonID,
                GroupID = request.GroupID,
                StudentID = request.StudentID,
                TeacherID = request.TeacherID,
                Content = request.Content,
                Status = "Not Submitted",
                CreatedAt = DateTime.Now
            };

            _context.Assignments.Add(assignment);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetAssignmentById), new { id = assignment.AssignmentID }, assignment);
        }

        // Обновить задание
        [HttpPut("{id}")]
        public IActionResult UpdateAssignment(int id, [FromBody] UpdateAssignmentRequest request)
        {
            var assignment = _context.Assignments.FirstOrDefault(a => a.AssignmentID == id);
            if (assignment == null)
                return NotFound("Assignment not found");

            assignment.Content = request.Content;
            assignment.Comment = request.Comment;
            assignment.Grade = request.Grade;
            assignment.Status = request.Status;
            assignment.CheckedAt = request.CheckedAt;

            _context.SaveChanges();
            return NoContent();
        }

        // Удалить задание
        [HttpDelete("{id}")]
        public IActionResult DeleteAssignment(int id)
        {
            var assignment = _context.Assignments.FirstOrDefault(a => a.AssignmentID == id);
            if (assignment == null)
                return NotFound("Assignment not found");

            _context.Assignments.Remove(assignment);
            _context.SaveChanges();
            return NoContent();
        }

        // Получить задания группы
        [HttpGet("group/{groupId}")]
        public IActionResult GetAssignmentsByGroup(int groupId)
        {
            var assignments = _context.Assignments
                .Where(a => a.GroupID == groupId)
                .Include(a => a.Lesson)
                .ToList();
            return Ok(assignments);
        }

        // Получить задания ученика
        [HttpGet("student/{studentId}")]
        public IActionResult GetAssignmentsByStudent(int studentId)
        {
            var assignments = _context.Assignments
                .Where(a => a.StudentID == studentId)
                .Include(a => a.Lesson)
                .ToList();
            return Ok(assignments);
        }

        // Отметить задание как выполненное
        [HttpPost("{id}/submit")]
        public IActionResult SubmitAssignment(int id)
        {
            var assignment = _context.Assignments.FirstOrDefault(a => a.AssignmentID == id);
            if (assignment == null)
                return NotFound("Assignment not found");

            assignment.Status = "Submitted";
            assignment.SubmittedAt = DateTime.Now;

            _context.SaveChanges();
            return Ok("Assignment submitted successfully");
        }
    }

    // Запросы для создания и обновления задания
    public class CreateAssignmentRequest
    {
        public int LessonID { get; set; }
        public int? GroupID { get; set; }
        public int? StudentID { get; set; }
        public int TeacherID { get; set; }
        public string Content { get; set; }
    }

    public class UpdateAssignmentRequest
    {
        public string Content { get; set; }
        public string Comment { get; set; }
        public int? Grade { get; set; }
        public string Status { get; set; }
        public DateTime? CheckedAt { get; set; }
    }
}

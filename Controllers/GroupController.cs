using Microsoft.AspNetCore.Mvc;
using EducationPlatformAPI.Data;
using EducationPlatformAPI.Models;
using System.Linq;

namespace EducationPlatformAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupController : ControllerBase
    {
        private readonly EducationPlatformContext _context;

        public GroupController(EducationPlatformContext context)
        {
            _context = context;
        }

        // Получить все группы
        [HttpGet]
        public IActionResult GetAllGroups()
        {
            var groups = _context.Groups.ToList();
            return Ok(groups);
        }

        // Получить группу по ID
        [HttpGet("{id}")]
        public IActionResult GetGroupById(int id)
        {
            var group = _context.Groups.FirstOrDefault(g => g.GroupID == id);
            if (group == null)
                return NotFound("Group not found");
            return Ok(group);
        }

        // Создать новую группу
        [HttpPost]
        public IActionResult CreateGroup([FromBody] CreateGroupRequest request)
        {
            var group = new Group
            {
                GroupName = request.GroupName,
                GroupType = request.GroupType,
                TeacherID = request.TeacherID,
                CreatedAt = DateTime.Now
            };

            _context.Groups.Add(group);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetGroupById), new { id = group.GroupID }, group);
        }

        // Редактировать группу
        [HttpPut("{id}")]
        public IActionResult UpdateGroup(int id, [FromBody] UpdateGroupRequest request)
        {
            var group = _context.Groups.FirstOrDefault(g => g.GroupID == id);
            if (group == null)
                return NotFound("Group not found");

            group.GroupName = request.GroupName;
            group.GroupType = request.GroupType;

            _context.SaveChanges();
            return NoContent();
        }

        // Удалить группу
        [HttpDelete("{id}")]
        public IActionResult DeleteGroup(int id)
        {
            var group = _context.Groups.FirstOrDefault(g => g.GroupID == id);
            if (group == null)
                return NotFound("Group not found");

            _context.Groups.Remove(group);
            _context.SaveChanges();
            return NoContent();
        }

        // Добавить ученика в группу
        [HttpPost("{groupId}/add-student")]
        public IActionResult AddStudentToGroup(int groupId, [FromBody] AddStudentToGroupRequest request)
        {
            var group = _context.Groups.FirstOrDefault(g => g.GroupID == groupId);
            if (group == null)
                return NotFound("Group not found");

            var student = _context.Users.FirstOrDefault(u => u.UserID == request.StudentID && u.Role == "student");
            if (student == null)
                return NotFound("Student not found");

            var groupStudent = new GroupStudent
            {
                GroupID = groupId,
                StudentID = request.StudentID
            };

            _context.GroupStudents.Add(groupStudent);
            _context.SaveChanges();

            return Ok("Student added to group successfully");
        }

        // Получить список учеников в группе
        [HttpGet("{groupId}/students")]
        public IActionResult GetStudentsInGroup(int groupId)
        {
            var students = _context.GroupStudents
                .Where(gs => gs.GroupID == groupId)
                .Select(gs => new
                {
                    gs.StudentID,
                    StudentName = _context.Users
                        .Where(u => u.UserID == gs.StudentID)
                        .Select(u => u.FirstName + " " + u.LastName)
                        .FirstOrDefault()
                })
                .ToList();

            return Ok(students);
        }
    }

    // Запросы для создания и обновления группы
    public class CreateGroupRequest
    {
        public string GroupName { get; set; }
        public string GroupType { get; set; } // Individual, MiniGroup, Group
        public int TeacherID { get; set; }
    }

    public class UpdateGroupRequest
    {
        public string GroupName { get; set; }
        public string GroupType { get; set; }
    }

    public class AddStudentToGroupRequest
    {
        public int StudentID { get; set; }
    }
}

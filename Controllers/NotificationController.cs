using Microsoft.AspNetCore.Mvc;
using EducationPlatformAPI.Data;
using EducationPlatformAPI.Models;
using System.Linq;

namespace EducationPlatformAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly EducationPlatformContext _context;

        public NotificationController(EducationPlatformContext context)
        {
            _context = context;
        }

        // Получить все уведомления пользователя
        [HttpGet("user/{userId}")]
        public IActionResult GetNotificationsByUser(int userId)
        {
            var notifications = _context.Notifications.Where(n => n.UserID == userId).ToList();
            return Ok(notifications);
        }

        // Получить непрочитанные уведомления пользователя
        [HttpGet("user/{userId}/unread")]
        public IActionResult GetUnreadNotifications(int userId)
        {
            var notifications = _context.Notifications.Where(n => n.UserID == userId && !n.IsRead).ToList();
            return Ok(notifications);
        }

        // Отметить уведомление как прочитанное
        [HttpPut("{id}/mark-as-read")]
        public IActionResult MarkNotificationAsRead(int id)
        {
            var notification = _context.Notifications.FirstOrDefault(n => n.NotificationID == id);
            if (notification == null)
                return NotFound("Notification not found");

            notification.IsRead = true;
            _context.SaveChanges();

            return NoContent();
        }

        // Создать уведомление
        [HttpPost]
        public IActionResult CreateNotification([FromBody] CreateNotificationRequest request)
        {
            var notification = new Notification
            {
                UserID = request.UserID,
                Message = request.Message,
                IsRead = false,
                CreatedAt = DateTime.Now
            };

            _context.Notifications.Add(notification);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetNotificationsByUser), new { userId = notification.UserID }, notification);
        }

        // Удалить уведомление
        [HttpDelete("{id}")]
        public IActionResult DeleteNotification(int id)
        {
            var notification = _context.Notifications.FirstOrDefault(n => n.NotificationID == id);
            if (notification == null)
                return NotFound("Notification not found");

            _context.Notifications.Remove(notification);
            _context.SaveChanges();
            return NoContent();
        }
    }

    // Запрос для создания уведомления
    public class CreateNotificationRequest
    {
        public int UserID { get; set; }
        public string Message { get; set; }
    }
}

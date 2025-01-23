namespace EducationPlatformAPI.Models
{
    public class Assignment
    {
        public int AssignmentID { get; set; }
        public int LessonID { get; set; }
        public int? GroupID { get; set; }
        public int? StudentID { get; set; }
        public int TeacherID { get; set; }
        public string Content { get; set; }
        public string Comment { get; set; }
        public int? Grade { get; set; }
        public string Status { get; set; } = "Not Submitted";
        public DateTime? SubmittedAt { get; set; }
        public DateTime? CheckedAt { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now; // Добавлено свойство

        // Связи с другими сущностями
        public Lesson Lesson { get; set; }
        public Group Group { get; set; }
        public User Student { get; set; }
        public User Teacher { get; set; }
    }
}

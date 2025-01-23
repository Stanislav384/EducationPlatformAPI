namespace EducationPlatformAPI.Models
{
    public class Attendance
    {
        public int AttendanceID { get; set; }
        public int LessonID { get; set; }
        public int StudentID { get; set; }
        public bool WasPresent { get; set; }
        public DateTime MarkedAt { get; set; }

        public Lesson Lesson { get; set; }
        public User Student { get; set; }
    }
}

namespace EducationPlatformAPI.Models
{
    public class Lesson
    {
        public int LessonID { get; set; }
        public int CourseID { get; set; }
        public string LessonName { get; set; }
        public string LessonContent { get; set; }
        public DateTime CreatedAt { get; set; }

        public Course Course { get; set; }
    }
}

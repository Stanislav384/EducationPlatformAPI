namespace EducationPlatformAPI.Models
{
    public class GroupStudent
    {
        public int GroupStudentID { get; set; }
        public int GroupID { get; set; }
        public int StudentID { get; set; }

        public Group Group { get; set; }
        public User Student { get; set; }
    }
}

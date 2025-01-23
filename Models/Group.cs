namespace EducationPlatformAPI.Models
{
    public class Group
    {
        public int GroupID { get; set; }
        public string GroupName { get; set; }
        public string GroupType { get; set; } // Individual, MiniGroup, Group
        public int TeacherID { get; set; }
        public DateTime CreatedAt { get; set; }

        public User Teacher { get; set; }
    }
}

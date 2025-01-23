namespace EducationPlatformAPI.Models
{
    public class User
    {
        public int UserID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Role { get; set; } // student, teacher, admin
        public string Login { get; set; }
        public string Password { get; set; }
        public string Avatar { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

namespace EducationPlatformAPI.Models
{
    public class Performance
    {
        public int StudentID { get; set; }
        public int TotalAssignments { get; set; } = 0;
        public int TotalGrades { get; set; } = 0;
        public double AverageGrade { get; set; } = 0.0;

        public User Student { get; set; }
    }
}

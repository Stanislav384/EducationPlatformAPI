using Microsoft.EntityFrameworkCore;
using EducationPlatformAPI.Models;

namespace EducationPlatformAPI.Data
{
    public class EducationPlatformContext : DbContext
    {
        public EducationPlatformContext(DbContextOptions<EducationPlatformContext> options)
            : base(options) { }

        // Таблицы базы данных
        public DbSet<User> Users { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Lesson> Lessons { get; set; }
        public DbSet<Assignment> Assignments { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<GroupStudent> GroupStudents { get; set; }
        public DbSet<Attendance> Attendance { get; set; }
        public DbSet<Performance> Performance { get; set; }
        public DbSet<Notification> Notifications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Указываем первичный ключ для Performance
            modelBuilder.Entity<Performance>()
                .HasKey(p => p.StudentID);

            // Указываем связи
            modelBuilder.Entity<GroupStudent>()
                .HasOne(gs => gs.Group)
                .WithMany()
                .HasForeignKey(gs => gs.GroupID);

            modelBuilder.Entity<GroupStudent>()
                .HasOne(gs => gs.Student)
                .WithMany()
                .HasForeignKey(gs => gs.StudentID);

            modelBuilder.Entity<Assignment>()
                .HasOne(a => a.Lesson)
                .WithMany()
                .HasForeignKey(a => a.LessonID);

            modelBuilder.Entity<Assignment>()
                .HasOne(a => a.Group)
                .WithMany()
                .HasForeignKey(a => a.GroupID);

            modelBuilder.Entity<Assignment>()
                .HasOne(a => a.Student)
                .WithMany()
                .HasForeignKey(a => a.StudentID);

            modelBuilder.Entity<Assignment>()
                .HasOne(a => a.Teacher)
                .WithMany()
                .HasForeignKey(a => a.TeacherID);

            modelBuilder.Entity<Attendance>()
                .HasOne(at => at.Lesson)
                .WithMany()
                .HasForeignKey(at => at.LessonID);

            modelBuilder.Entity<Attendance>()
                .HasOne(at => at.Student)
                .WithMany()
                .HasForeignKey(at => at.StudentID);

            modelBuilder.Entity<Notification>()
                .HasOne(n => n.User)
                .WithMany()
                .HasForeignKey(n => n.UserID);
        }
    }
}

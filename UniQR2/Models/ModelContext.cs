using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UniQR2.Models
{
    public class ModelContext : DbContext
    {
        public ModelContext(DbContextOptions opt) : base(opt)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<AttendanceList> AttendanceLists { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Classroom> Classrooms { get; set; }
        public DbSet<CourseClassroom> CourseClassrooms { get; set; }
        public DbSet<CourseStudentRel> CourseStudentRels { get; set; }
        public DbSet<Participation> Participations { get; set; }
        public DbSet<Floor> Floors { get; set; }
        public DbSet<File> Files { get; set; }
        public DbSet<Announcement> Announcements { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(new User
            {
                UserID = 1,
                Email = "kamren2@ethereal.email",
                Password = "123123",
                FullName = "System Admin",
                IsActive = true,
                UserRole = UserRole.Administrator
            });
        }
    }
}

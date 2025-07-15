using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentManagement.Application.Entities;
namespace StudentManagement.DataAccess.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<Student> Students { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Attendance> Attendances { get; set; }
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<StudentCourse> StudentCourses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<StudentCourse>()
            .HasOne(sc => sc.Student)
            .WithMany(s => s.StudentCourses)
            .HasForeignKey(sc => sc.StudentId)
            .HasPrincipalKey(s => s.UId);



            modelBuilder.Entity<StudentCourse>()
           .HasOne(sc => sc.Course)
           .WithMany(c => c.StudentCourses)
           .HasForeignKey(sc => sc.CourseId)
           .HasPrincipalKey(c => c.SId);

        }


    }
}

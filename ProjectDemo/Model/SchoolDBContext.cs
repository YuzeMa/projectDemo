using System;
using Microsoft.EntityFrameworkCore;
using ProjectDemo.Model.User;

namespace ProjectDemo.Model
{
    public class SchoolDBContext:DbContext
    {
        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Lecturer> Lecturers { get; set; }
        public DbSet<StudentsCourses> StudentsCourses { get; set; }
        public DbSet<LecturersCourses> LecturersCourses { get; set; }
        public DbSet<UserDetail> Users { get; set; }

        public SchoolDBContext(DbContextOptions<SchoolDBContext> options) : base(options)
        {
            Database.EnsureCreated();
            //Database.Migrate();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>().Property(a => a.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Student>().HasKey(a => a.Id);

            modelBuilder.Entity<Course>().Property(a => a.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Course>().HasKey(a => a.Id);

            modelBuilder.Entity<Lecturer>().Property(a => a.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Lecturer>().HasKey(a => a.Id);

            //User Database
            modelBuilder.Entity<UserDetail>().HasKey(u => u.Id);
            modelBuilder.Entity<UserDetail>().Property(u => u.Password).IsRequired();

            //Lecturer to Courses
            modelBuilder.Entity<LecturersCourses>()
                       .HasKey(lc => new { lc.CourseId, lc.LecturerId });

            modelBuilder.Entity<LecturersCourses>()
                        .HasOne(lc => lc.Course)
                        .WithMany(c => c.LecturersCourses)
                        .HasForeignKey(sc => sc.CourseId);

            modelBuilder.Entity<LecturersCourses>()
                        .HasOne(lc => lc.Lecturer)
                        .WithMany(l => l.LecturersCourses)
                        .HasForeignKey(lc => lc.LecturerId);


            //Students to Courses
            modelBuilder.Entity<StudentsCourses>()
                        .HasKey(sc => new {sc.StudentId,sc.CourseId});

            modelBuilder.Entity<StudentsCourses>()
                        .HasOne(sc => sc.Student)
                        .WithMany(s => s.StudentsCourses)
                        .HasForeignKey(sc => sc.StudentId);

            modelBuilder.Entity<StudentsCourses>()
                        .HasOne(sc => sc.Course)
                        .WithMany(c => c.StudentsCourses)
                        .HasForeignKey(sc => sc.CourseId);
        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = "server=localhost;userid=root;pwd=Ma950522;port=3306;database=projectTest;sslmode=none";
            //var connectionString = "server=54.252.222.91;userid=yuzema_test;pwd=Ma950522;port=3306;database=database_test;sslmode=none";
            optionsBuilder.UseMySQL(connectionString);
            base.OnConfiguring(optionsBuilder);
        }
    }
}

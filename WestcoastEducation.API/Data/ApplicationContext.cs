using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WestcoastEducation.API.Data.Entities;

namespace WestcoastEducation.API.Data;

public class ApplicationContext : IdentityDbContext
{
    public ApplicationContext(DbContextOptions<ApplicationContext> options)
        : base(options)
    {
        
    }
    
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Course> Courses => Set<Course>();
    public DbSet<Student> Students => Set<Student>();
    public DbSet<StudentCourse> StudentCourses => Set<StudentCourse>();
    public DbSet<Teacher> Teachers => Set<Teacher>();
    public DbSet<TeacherCompetency> TeacherCompetencies => Set<TeacherCompetency>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<TeacherCompetency>(e =>
        {
            e.HasKey(tc => new { tc.TeacherId, tc.CategoryId });
            
            e.HasOne<Teacher>()
                .WithMany(t => t.TeacherCompetencies)
                .HasForeignKey(tc => tc.TeacherId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            e.HasOne<Category>()
                .WithMany(c => c.TeacherCompetencies)
                .HasForeignKey(tc => tc.CategoryId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        });
        
        builder.Entity<StudentCourse>(e =>
        {
            e.HasKey(sc => new { sc.StudentId, sc.CourseId });

            e.HasOne<Student>()
                .WithMany(s => s.StudentCourses)
                .HasForeignKey(sc => sc.StudentId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            e.HasOne<Course>()
                .WithMany(c => c.StudentCourses)
                .HasForeignKey(sc => sc.CourseId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<Course>(e =>
        {
            e.HasOne<Category>()
                .WithMany(c => c.Courses)
                .HasForeignKey(c => c.CategoryId);

            e.HasOne<Teacher>()
                .WithMany(t => t.Courses)
                .HasForeignKey(c => c.TeacherId);
        });
    }
}
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WestcoastEducation.API.Data.Entities;

namespace WestcoastEducation.API.Data;

public class ApplicationContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationContext(DbContextOptions options)
        : base(options) { }
    
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Course> Courses => Set<Course>();
    public DbSet<Student> Students => Set<Student>();
    public DbSet<Teacher> Teachers => Set<Teacher>();
    public DbSet<ApplicationUser> ApplicationUsers => Set<ApplicationUser>();
    public DbSet<StudentCourse> StudentCourses => Set<StudentCourse>();
    public DbSet<TeacherCompetency> TeacherCompetencies => Set<TeacherCompetency>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<TeacherCompetency>(e =>
        {
            e.HasKey(tc => new { tc.TeacherId, tc.CategoryId });
            
            e.HasOne(tc => tc.Teacher)
                .WithMany(t => t.TeacherCompetencies)
                .HasForeignKey(tc => tc.TeacherId);

            e.HasOne(tc => tc.Category)
                .WithMany(c => c.TeacherCompetencies)
                .HasForeignKey(tc => tc.CategoryId);
        });
        
        builder.Entity<StudentCourse>(e =>
        {
            e.HasKey(sc => new { sc.StudentId, sc.CourseId });

            e.HasOne(sc => sc.Student)
                .WithMany(s => s.StudentCourses)
                .HasForeignKey(sc => sc.StudentId);

            e.HasOne(sc => sc.Course)
                .WithMany(c => c.StudentCourses)
                .HasForeignKey(sc => sc.CourseId);
        });

        builder.Entity<Course>(e =>
        {
            e.HasIndex(c => c.CourseNo)
                .IsUnique();
            
            e.HasOne(c => c.Category)
                .WithMany(c => c.Courses)
                .HasForeignKey(c => c.CategoryId);

            e.HasOne(c => c.Teacher)
                .WithMany(t => t.Courses)
                .HasForeignKey(c => c.TeacherId);
        });

        builder.Entity<Teacher>(e =>
        {
            e.HasOne(t => t.ApplicationUser)
                .WithOne()
                .HasForeignKey<Teacher>(t => t.ApplicationUserId);
            
        });

        builder.Entity<Student>(e =>
        {
            e.HasOne(t => t.ApplicationUser)
                .WithOne()
                .HasForeignKey<Student>(s => s.ApplicationUserId);
        });

        base.OnModelCreating(builder);
    }
}
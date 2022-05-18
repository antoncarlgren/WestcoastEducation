namespace WestcoastEducation.API.Data.Entities;

public class StudentCourse
{
    public Student? Student { get; set; }
    public string? StudentId { get; set; }

    public Course? Course { get; set; }
    public string? CourseId { get; set; }
}
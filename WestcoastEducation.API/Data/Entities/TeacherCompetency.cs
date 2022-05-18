namespace WestcoastEducation.API.Data.Entities;

public class TeacherCompetency
{
    public Teacher? Teacher { get; set; }
    public string? TeacherId { get; set; }

    public Category? Category { get; set; }
    public string? CategoryId { get; set; }
}
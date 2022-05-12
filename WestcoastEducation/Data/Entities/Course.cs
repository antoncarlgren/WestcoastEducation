using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WestcoastEducation.API.Data.Entities;

public class Course : IEntity
{
    [Key]
    public int? Id { get; set; }
    
    public int? CourseNo { get; set; }
    public string? Title { get; set; }
    public string? Details { get; set; }
    public string? Description { get; set; }
    public string? Length { get; set; }
    
    [ForeignKey("CategoryId")]
    public Category? Category { get; set; }
    public int? CategoryId { get; set; }
    
    [ForeignKey("TeacherId")]
    public Teacher? Teacher { get; set; }
    public int? TeacherId { get; set; }
    
    public ICollection<Student>? Students { get; set; }
}
﻿namespace WestcoastEducation.API.ViewModels.Course;

public class CourseViewModel
{
    public int? CourseNo { get; set; }
    public string? Title { get; set; }
    public string? Details { get; set; }
    public string? Description { get; set; }
    public string? Length { get; set; }
    public Data.Entities.Category? Category { get; set; }
    public Data.Entities.Teacher? Teacher { get; set; }
}
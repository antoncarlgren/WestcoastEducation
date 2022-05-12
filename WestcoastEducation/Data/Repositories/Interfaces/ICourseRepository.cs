using WestcoastEducation.API.ViewModels.Course;

namespace WestcoastEducation.API.Data.Repositories.Interfaces;

public interface ICourseRepository
    : IRepository<CourseViewModel, PostCourseViewModel, PostCourseViewModel>
{
    
}
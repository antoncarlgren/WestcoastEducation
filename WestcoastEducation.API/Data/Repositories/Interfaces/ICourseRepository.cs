using WestcoastEducation.API.ViewModels.Category;
using WestcoastEducation.API.ViewModels.Course;

namespace WestcoastEducation.API.Data.Repositories.Interfaces;

public interface ICourseRepository
    : IRepository<CourseViewModel, PostCourseViewModel, PatchCourseViewModel>
{
    Task<List<CourseOverviewViewModel>> GetAllCourseOverviews();
    Task<CourseViewModel> GetCourseByCourseNo(int courseNo);
}
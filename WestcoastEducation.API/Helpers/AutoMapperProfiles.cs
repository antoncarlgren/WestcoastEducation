using AutoMapper;
using WestcoastEducation.API.Data.Entities;
using WestcoastEducation.API.ViewModels.Course;

namespace WestcoastEducation.API.Helpers;

public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        CreateMap<PostCourseViewModel, Course>();
        CreateMap<Course, CourseViewModel>()
            .ForMember(dest => dest.CourseId, options => options.MapFrom(src => src.Id))
            .ForMember(dest => dest.Category, options => options.MapFrom(src => src.Category!.Name))
            .ForMember(dest => dest.Teacher, options => options.MapFrom(src => 
                string.Concat(src.Teacher!.FirstName, " ", src.Teacher.LastName)));
    }
}
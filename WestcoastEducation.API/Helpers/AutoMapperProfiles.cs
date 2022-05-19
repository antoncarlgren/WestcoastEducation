using AutoMapper;
using WestcoastEducation.API.Data.Entities;
using WestcoastEducation.API.ViewModels.Course;
using WestcoastEducation.API.ViewModels.Student;

namespace WestcoastEducation.API.Helpers;

public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        CreateMap<Student, StudentViewModel>()
            .ForMember(dest => dest.Email, options => options.MapFrom(src => src.ApplicationUser.Email))
            .ForMember(dest => dest.Address, options => options.MapFrom(src => src.ApplicationUser.Address))
            .ForMember(dest => dest.PhoneNumber, options => options.MapFrom(src => src.ApplicationUser.PhoneNumber))
            .ForMember(dest => dest.Courses, options => options.MapFrom(src =>
                src.StudentCourses.Select(sc => sc.Course!.Title)))
            .ForMember(dest => dest.Name, options => options.MapFrom(src => 
                string.Concat(src.ApplicationUser.FirstName, " ", src.ApplicationUser.LastName)));
        
        CreateMap<PostCourseViewModel, Course>();
        CreateMap<Course, CourseViewModel>()
            .ForMember(dest => dest.CourseId, options => options.MapFrom(src => src.Id))
            .ForMember(dest => dest.Category, options => options.MapFrom(src => src.Category!.Name))
            .ForMember(dest => dest.Teacher, options => options.MapFrom(src => 
                string.Concat(src.Teacher!.ApplicationUser.FirstName, " ", src.Teacher.ApplicationUser.LastName)));
    }
}
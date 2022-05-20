using AutoMapper;
using WestcoastEducation.API.Data.Entities;
using WestcoastEducation.API.ViewModels.Address;
using WestcoastEducation.API.ViewModels.Category;
using WestcoastEducation.API.ViewModels.Course;
using WestcoastEducation.API.ViewModels.Student;
using WestcoastEducation.API.ViewModels.Teacher;

namespace WestcoastEducation.API.Helpers;

public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        CreateAddressMaps();
        CreateStudentMaps();
        CreateCourseMaps();
        CreateCategoryMaps();
        CreateTeacherMaps();
    }

    private void CreateAddressMaps()
    {
        CreateMap<Address, PostAddressViewModel>();
        CreateMap<AddressViewModel, Address>();
    }

    private void CreateStudentMaps()
    {
        CreateMap<Student, StudentViewModel>()
            .ForMember(dest => dest.Email, options => options.MapFrom(src => src.ApplicationUser.Email))
            .ForMember(dest => dest.PhoneNumber, options => options.MapFrom(src => src.ApplicationUser.PhoneNumber))
            .ForMember(dest => dest.Courses, options => options.MapFrom(src =>
                src.StudentCourses
                    .Select(sc => sc.Course!.Title)))
            .ForMember(dest => dest.Name, options => options.MapFrom(src => 
                string.Concat(src.ApplicationUser.FirstName, " ", src.ApplicationUser.LastName)))
            .ForMember(dest => dest.Address, options => options.MapFrom(src => 
                string.Concat(
                    src.ApplicationUser.Address!.StreetName, 
                    ", ", 
                    src.ApplicationUser.Address.ZipCode,
                    " ",
                    src.ApplicationUser.Address.City)));
    }

    private void CreateCourseMaps()
    {
        CreateMap<PostCourseViewModel, Course>();
        CreateMap<Course, CourseOverviewViewModel>();

        CreateMap<Course, CourseViewModel>()
            .ForMember(dest => dest.CourseId, options => options.MapFrom(src => src.Id))
            .ForMember(dest => dest.Category, options => options.MapFrom(src => src.Category!.Name))
            .ForMember(dest => dest.Teacher, options => options.MapFrom(src => 
                string.Concat(src.Teacher!.ApplicationUser.FirstName, " ", src.Teacher.ApplicationUser.LastName)));
    }

    private void CreateCategoryMaps()
    {
        CreateMap<Category, CategoryViewModel>();
    }

    private void CreateTeacherMaps()
    {
        CreateMap<Teacher, TeacherViewModel>()
            .ForMember(dest => dest.Email, options => options.MapFrom(src => src.ApplicationUser.Email))
            .ForMember(dest => dest.PhoneNumber, options => options.MapFrom(src => src.ApplicationUser.PhoneNumber))
            .ForMember(dest => dest.Courses, options => options.MapFrom(src =>
                src.Courses!.Select(sc => sc.Title)))
            .ForMember(dest => dest.Competencies, options => options.MapFrom(src => 
                src.TeacherCompetencies!
                    .Select(tc => tc.Category!.Name)))
            .ForMember(dest => dest.Name, options => options.MapFrom(src => 
                string.Concat(src.ApplicationUser.FirstName, " ", src.ApplicationUser.LastName)))
            .ForMember(dest => dest.Address, options => options.MapFrom(src => 
                string.Concat(
                    src.ApplicationUser.Address!.StreetName, 
                    ", ", 
                    src.ApplicationUser.Address.ZipCode,
                    " ",
                    src.ApplicationUser.Address.City)));
    }
}
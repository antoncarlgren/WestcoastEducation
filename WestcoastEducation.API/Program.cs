using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WestcoastEducation.API.Data;
using WestcoastEducation.API.Data.Repositories;
using WestcoastEducation.API.Data.Repositories.Interfaces;
using WestcoastEducation.API.Helpers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationContext>(options => 
    options.UseSqlite(builder.Configuration.GetConnectionString("ApplicationDatabase")));

builder.Services.AddControllers();

builder.Services
    .AddScoped<ICategoryRepository, CategoryRepository>()
    .AddScoped<ICourseRepository, CourseRepository>()
    .AddScoped<IStudentRepository, StudentRepository>()
    .AddScoped<ITeacherRepository, TeacherRepository>();

builder.Services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);

var app = builder.Build();

if (builder.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

await app.RunAsync();
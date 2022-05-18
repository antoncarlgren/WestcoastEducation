using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WestcoastEducation.API.Data;
using WestcoastEducation.API.Data.Repositories;
using WestcoastEducation.API.Data.Repositories.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<ApplicationContext>(options => 
    options.UseSqlServer(builder.Configuration.GetConnectionString("ApplicationDatabase")));

builder.Services
    .AddScoped<ICategoryRepository, CategoryRepository>()
    .AddScoped<ICourseRepository, CourseRepository>()
    .AddScoped<IStudentRepository, StudentRepository>()
    .AddScoped<ITeacherRepository, TeacherRepository>();



var app = builder.Build();

if (builder.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

await app.RunAsync();
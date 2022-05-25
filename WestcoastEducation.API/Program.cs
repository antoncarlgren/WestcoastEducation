using System.Runtime.Versioning;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.EntityFrameworkCore;
using WestcoastEducation.API.Data;
using WestcoastEducation.API.Data.DataSeeder;
using WestcoastEducation.API.Data.Entities;
using WestcoastEducation.API.Data.Repositories;
using WestcoastEducation.API.Data.Repositories.Interfaces;
using WestcoastEducation.API.Helpers;

var builder = WebApplication.CreateBuilder(args);

// builder.Services.AddDbContext<ApplicationContext>(options => 
//     options.UseSqlite(builder.Configuration.GetConnectionString("Sqlite")));

builder.Services.AddDbContext<ApplicationContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer")));

builder.Services
    .AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationContext>();

builder.Services.AddAuthentication();

builder.Services
    .AddControllers()
    .AddJsonOptions(options =>
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

builder.Services.AddSwaggerGen();

builder.Services
    .AddScoped<ICategoryRepository, CategoryRepository>()
    .AddScoped<ICourseRepository, CourseRepository>()
    .AddScoped<IStudentRepository, StudentRepository>()
    .AddScoped<ITeacherRepository, TeacherRepository>()
    .AddTransient<UserManager<ApplicationUser>>();


builder.Services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);

var app = builder.Build();

if (builder.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    try
    {
        var context = services.GetRequiredService<ApplicationContext>();
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

        var dataSeeder = new DataSeeder(context, userManager);

        await dataSeeder.LoadCategories();
        await dataSeeder.LoadStudents();
        await dataSeeder.LoadTeachers();
        await dataSeeder.LoadCourses();
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occured upon database migration.");
    }
}

await app.RunAsync();
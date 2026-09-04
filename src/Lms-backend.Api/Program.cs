using Lms_backend.Application.Interfaces;
using Lms_backend.Application.Services;
using Lms_backend.Infrastructure;
using Lms_backend.Infrastructure.Interfaces;
using Lms_backend.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Postgres config
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("postgres")
    ?? throw new InvalidProgramException()
));

// Dependency injections
builder.Services.AddScoped<ICourseRepository, CourseRepository>();
builder.Services.AddScoped<IModuleRepository, ModuleRepository>();
builder.Services.AddScoped<IActivityRepository, ActivityRepository>();
builder.Services.AddScoped<IResourceRepository, ResourceRepository>();
builder.Services.AddScoped<ICoursesService, CoursesService>();
builder.Services.AddScoped<IModulesService, ModulesService>();
builder.Services.AddScoped<IActivitiesService, ActivitiesService>();
builder.Services.AddScoped<IResourcesService, ResourcesService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

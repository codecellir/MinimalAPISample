using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MinimalAPISample.Data;
using MinimalAPISample.Entities;
using MinimalAPISample.Filters;
using MinimalAPISample.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<AppDbContext>(cfg => cfg.UseSqlServer("Server=.;Database=dms_minima;User ID=sa;Password=Password;TrustServerCertificate=True"));

builder.Services.AddScoped<IStudentRepository, StudentRepository>();
var app = builder.Build();

var student = app.MapGroup("/api/students");

student.MapGet("{id}", GetStudentByIdAsync).WithName("GetStudentById");

student.MapPost("", async (Student std, IStudentRepository repo) =>
{
    await repo.CreateAsync(std);
    return Results.CreatedAtRoute("GetStudentById", new { std.Id }, std);
}).AddEndpointFilter<StudentValidationFilter>();

student.MapGet("", async (IStudentRepository repo) =>
{
    return Results.Ok(await repo.GetAllAsync());
});

student.MapPut("", async (Student std, IStudentRepository repo) =>
{
    await repo.EditAsync(std);
    return Results.NoContent();
}).AddEndpointFilter<StudentValidationFilter>();

student.MapDelete("{id}", async (int id, IStudentRepository repo) =>
{
    await repo.DeleteAsync(id);
    return Results.NoContent();
});

//app.MapGet("api/students/get", async ([FromQuery]int id, IStudentRepository repo) =>
//{
//    var student = await repo.GetAsync(id);
//    return Results.Ok(student);
//});

async Task<IResult> GetStudentByIdAsync(int id,IStudentRepository repo)
{
    var student = await repo.GetAsync(id);
    return Results.Ok(student);
}

app.Run();
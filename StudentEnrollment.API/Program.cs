using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentEnrollment.Data;
using StudentEnrollment.API.Endpoints;

var builder = WebApplication.CreateBuilder(args);

var conn = builder.Configuration.GetConnectionString("StudentEnrollmentDbConnection");
builder.Services.AddDbContext<StudentEnrollmentDbContext>(options =>
{
    options.UseSqlServer(conn);
});
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy => policy.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.MapGet("/courses", async ([FromServices] StudentEnrollmentDbContext context) => 
{
    return await context.Courses.ToListAsync();
    // na ovom endpointu dolazimo do lite kurseva iz baze

}); // kako dolazim do ovog endpointa

app.MapGet("courses/id", async ([FromServices] StudentEnrollmentDbContext context, int id) => // zelimo da dohvatimo podatak po idju
{
    return await context.Courses.FindAsync(id) is Course course ? Results.Ok(course) : Results.NotFound();

});
// metoda za dodavanje novog podatka u bazu
app.MapPost("courses/", async (StudentEnrollmentDbContext context, Course course) => 
{
    await context.AddAsync(course);
    await context.SaveChangesAsync();

    return Results.Created($"/courses/{course.Id}", course);

});
// metoda za menjanje vec postojecih podataka, po idju
app.MapPut("courses/{id}", async (StudentEnrollmentDbContext context, Course course, int id) =>
{
    var recordExists = await context.Courses.AnyAsync(q => q.Id == course.Id);
    if (!recordExists) return Results.NotFound();

    context.Update(course);
    await context.SaveChangesAsync();

    return Results.Created($"/courses/{course.Id}", course);

});
// brisanje podatka po idju
app.MapDelete("courses/{id}", async (StudentEnrollmentDbContext context, Course course, int id) =>
{
    var record = await context.Courses.FindAsync(id);
    if (record == null) return Results.NotFound();

    context.Remove(record);
    await context.SaveChangesAsync();
    return Results.NoContent();


});

app.MapStudentEndpoints();

app.MapEnrollmentEndpoints();

app.MapCourseEndpoints();

app.Run();


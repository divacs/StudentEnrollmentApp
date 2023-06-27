using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentEnrollment.Data;

var builder = WebApplication.CreateBuilder(args);

var conn = builder.Configuration.GetConnectionString("StudentEnrollmentDbConnection");
builder.Services.AddDbContext<StudentEnrollmentDBContext>(options =>
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

app.MapGet("/courses", async ([FromServices]StudentEnrollmentDBContext context) => 
{
    await context.Courses.ToListAsync();
    // na ovom endpointu dolazimo do lite kurseva iz baze

}); // kako dolazim do ovog endpointa

app.MapGet("courses/id", async ([FromServices] StudentEnrollmentDBContext context, int id) => // zelimo da dohvatimo podatak po idju
{
    return await context.Courses.FindAsync(id) is Course course ? Results.Ok(course) : Results.NotFound();

}); 

app.Run();

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

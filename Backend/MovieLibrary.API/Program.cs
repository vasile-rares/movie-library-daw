using Microsoft.EntityFrameworkCore;
using MovieLibrary.API.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<MovieLibraryDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MovieLibraryDb")));

builder.Services.AddControllers();

builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Seed the database
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<MovieLibraryDbContext>();
        DbSeeder.SeedData(context);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"An error occurred while seeding the database: {ex.Message}");
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
using Microsoft.EntityFrameworkCore;
using WaterTrail.Api.Data;
using WaterTrail.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure SQLite
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? "Data Source=/app/data/kayak_trips.db";
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(connectionString));

// Register services
builder.Services.AddScoped<ITripService, TripService>();

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:3000", "http://frontend:3000")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowFrontend");

app.UseAuthorization();

app.MapControllers();

// Ensure database is created and seeded
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    // Ensure the data directory exists
    var dbPath = connectionString.Replace("Data Source=", "");
    var directory = Path.GetDirectoryName(dbPath);
    if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
    {
        Directory.CreateDirectory(directory);
    }

    DbInitializer.Initialize(context);
}

app.Run();

using Asp.Versioning;
using Library.Data;
using Library.Repository;
using Library.Service;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using RestWebApi.Service;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

// Load environment-specific appsettings automatically
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables(); // for Azure or CI/CD overrides

// Add services to the container.
builder.Services.AddControllers();

// Add EF Core
builder.Services.AddDbContext<LibraryContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register repositories and services
builder.Services.AddScoped<ILibraryQueryRepository, LibraryRepository>();
builder.Services.AddScoped<ILibraryCommandRepository, LibraryRepository>();
builder.Services.AddScoped<ILibraryService, LibraryService>();
builder.Services.AddScoped(typeof(IQueryRepo<>), typeof(EfQueryRepo<>));
builder.Services.AddScoped(typeof(ICommandRepo<>), typeof(EFCommandRepo<>));

// Add logging
builder.Logging.AddConsole();
builder.Logging.AddDebug();

// Add rate limiting policy
builder.Services.AddRateLimiter(options =>
{
    // "fixed" window rate limit — 5 requests per minute
    options.AddFixedWindowLimiter("FixedPolicy", opt =>
    {
        opt.Window = TimeSpan.FromMinutes(1);   // 1-minute window
        opt.PermitLimit = 5;                    // Max 5 requests per window
        opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        opt.QueueLimit = 0;                     // No queuing, reject immediately
    });
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

////Add API versioning
//builder.Services.AddApiVersioning(options =>
//{
//    options.DefaultApiVersion = new ApiVersion(1, 0); // Default version: 1.0
//    options.AssumeDefaultVersionWhenUnspecified = true;
//    options.ReportApiVersions = true;
//    // Combine multiple versioning schemes
//    options.ApiVersionReader = ApiVersionReader.Combine(
//        new QueryStringApiVersionReader("version"),
//        new UrlSegmentApiVersionReader(),
//        new HeaderApiVersionReader("X-API-Version"),
//        new MediaTypeApiVersionReader("version")
//    );
//});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

// Enable rate limiting globally
app.UseRateLimiter();

app.MapControllers().RequireRateLimiting("FixedPolicy");

await app.RunAsync();
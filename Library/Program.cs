using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Library.Data;
using Library.Features.Queries;
using Library.Repository;
using Library.Service;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
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

// Register Mediatr commands, repositories and services
builder.Services.AddScoped<ILibraryQueryRepository, LibraryRepository>();
builder.Services.AddScoped<ILibraryCommandRepository, LibraryRepository>();
builder.Services.AddScoped<ILibraryService, LibraryService>();
builder.Services.AddScoped(typeof(IQueryRepo<>), typeof(EfQueryRepo<>));
builder.Services.AddScoped(typeof(ICommandRepo<>), typeof(EfCommandRepo<>));
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(GetBooksQueryHandler).Assembly));

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

// API Versioning
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0); // Default version: 1.0
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
    options.ApiVersionReader = new UrlSegmentApiVersionReader();
})
.AddMvc() // because we’re using controllers
.AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV"; // e.g., v1, v2
    options.SubstituteApiVersionInUrl = true;
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    // Define Swagger documents for different versions
    options.SwaggerDoc("v1", new Microsoft.OpenApi.OpenApiInfo
    {
        Version = "v1",
        Title = "Library API v1",
        Description = "API documentation for version 1"
    });
    options.SwaggerDoc("v2", new Microsoft.OpenApi.OpenApiInfo
    {
        Version = "v2",
        Title = "Library API v2",
        Description = "API documentation for version 2"
    });
    // Use the ConflictingActionsResolver workaround
    options.ResolveConflictingActions(apiDescriptions =>
    {
        // Your conflict resolution strategy here
        return apiDescriptions.First();
    });
});

var app = builder.Build();

// --- Configure Swagger ---
if (app.Environment.IsDevelopment())
{
    var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        provider.ApiVersionDescriptions.Select(desc => desc.GroupName).ToList().ForEach(version =>
        {
            options.SwaggerEndpoint($"/swagger/{version}/swagger.json", $"Library API {version.ToUpperInvariant()}");
        });
    });
}

app.UseHttpsRedirection();

// Enable rate limiting globally
app.UseRateLimiter();

app.UseAuthorization();
app.MapControllers()
   .RequireRateLimiting("FixedPolicy");

await app.RunAsync();
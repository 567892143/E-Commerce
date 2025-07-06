using APILayer.Extension;
using Microsoft.AspNetCore.Diagnostics;
using ServiceLayer.DbSeeder;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddExceptionHandler(options =>
{
    options.ExceptionHandlingPath = "/error";
});// ✅ Fixed line

builder.Services.AddControllers();

// Register your application dependencies
builder.Services.ConfigurePostgresDbContext(builder.Configuration);
builder.Services.ConfigureRepositories();
builder.Services.ConfigureApplicationServices();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
        options.RoutePrefix = ""; // Serve Swagger at root
    });
}

app.Map("/error", (HttpContext httpContext) =>
{
    var exception = httpContext.Features.Get<IExceptionHandlerFeature>()?.Error;
    return Results.Problem(title: "An unexpected error occurred", detail: exception?.Message);
});

// Seed the database
if (true || app.Environment.IsProduction())
{
    SeedData.SeedDatabase(app.Services);
}

app.UseHttpsRedirection();
app.UseExceptionHandler(); // ✅ Correct place

app.MapControllers();

app.Run();

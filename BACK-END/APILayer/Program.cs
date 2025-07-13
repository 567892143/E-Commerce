using System.Text;
using APILayer.Extension;
using Microsoft.AspNetCore.Diagnostics;
using ServiceLayer.DbSeeder;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "E-Commerce API", Version = "v1" });

    // 🔐 Define the Bearer token authentication scheme
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer' followed by a space and your token.\n\nExample: `Bearer eyJhbGciOi...`"
    });

    // ✅ Apply it globally to all operations
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});
builder.Services.AddExceptionHandler<Shared.Exceptions.GlobalException>();

builder.Services.AddControllers();

// Register your application dependencies
builder.Services.ConfigurePostgresDbContext(builder.Configuration);
builder.Services.ConfigureRepositories();
builder.Services.ConfigureApplicationServices();

// ✅ Add Auth services
var secretKey = builder.Configuration["JwtConfig:SecretKey"];
if (string.IsNullOrWhiteSpace(secretKey))
    throw new InvalidOperationException("JWT SecretKey is missing in configuration");
var key = Encoding.UTF8.GetBytes(secretKey);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(key)
        };
    });

builder.Services.AddAuthorization();

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

// Seed the database
if (app.Environment.IsProduction())
{
    SeedData.SeedDatabase(app.Services);
}


app.UseHttpsRedirection();
app.UseExceptionHandler(_ => { });
// ✅ Correct place
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();

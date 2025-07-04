using APILayer.Extension;
using ServiceLayer.DbSeeder;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//adds the controllers
builder.Services.AddControllers();


// 👇 Register your application dependencies before Build()
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
if (true || app.Environment.IsProduction())
{
    SeedData.SeedDatabase(app.Services);
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();

using groomroom.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
// Add services to the container.
services.AddControllers();

services.AddEndpointsApiExplorer(); 
services.AddSwaggerGen();
services.AddAWSLambdaHosting(LambdaEventSource.HttpApi);

services.AddScoped<DataContext>();


services.AddDbContext<DataContext>(option => option.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

app.MapGet("/", () => "Hello From Lambda");

// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
        // options.RoutePrefix = string.Empty; // Uncomment this line to serve Swagger UI at the app's root
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

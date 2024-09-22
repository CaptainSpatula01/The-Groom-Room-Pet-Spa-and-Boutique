using System.Text;
using groomroom.Data;
using groomroom.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Writers;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

// Add services to the container.
services.AddControllers();

services.AddDbContext<DataContext>(options => 
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlServerOptions => sqlServerOptions.EnableRetryOnFailure()
    )
);

services.AddIdentity<User, Role>()
    .AddEntityFrameworkStores<DataContext>()
    .AddDefaultTokenProviders();

services.AddEndpointsApiExplorer(); 

services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "My API V1", Version = "v1" });
});

services.AddScoped<DataContext>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var servicesProvider = scope.ServiceProvider;

    var roleManager = servicesProvider.GetRequiredService<RoleManager<Role>>();

    if (!await roleManager.RoleExistsAsync("USER"))
    {
        await roleManager.CreateAsync(new Role {Name = "USER", NormalizedName = "USER"});
    }
}


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
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapControllerRoute(
    name: "admin",
    pattern: "Admin/{action=Dashboard}/{id?}",
    defaults: new { controller = "Admin" }
);
app.Run();

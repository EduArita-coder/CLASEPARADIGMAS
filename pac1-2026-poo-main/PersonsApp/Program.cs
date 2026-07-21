using Microsoft.EntityFrameworkCore;
using PersonsApp.Database;
using PersonsApp.Extensions;
using PersonsApp.Services.Auth;
using PersonsApp.Services.Persons;
using PersonsApp.Services.Roles;
using PersonsApp.Services.Statistics;
using PersonsApp.Services.Users;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddDbContext<PersonsDbContext>(options => 
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// builder.Services.AddScoped
// builder.Services.AddSingleton
builder.Services.AddTransient<IPersonService, PersonService>();
builder.Services.AddTransient<IRoleService, RoleService>();
builder.Services.AddTransient<IStatisticsService, StatisticsService>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IAuthService, AuthService>();

builder.Services.AddCorsConfiguration(builder.Configuration);
builder.Services.AddAuthenticationConfig(builder.Configuration);


builder.Services.AddOpenApi();

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseCors("CorsPolicy");

app.UseAuthorization();

app.MapControllers();

app.Run();
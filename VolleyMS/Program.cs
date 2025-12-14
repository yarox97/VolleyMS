using Microsoft.AspNetCore.Identity;
using System.Text.Json.Serialization;
using VolleyMS.BusinessLogic.Features.Authorisation;
using VolleyMS.BusinessLogic.Features.Authorisation.Registration;
using VolleyMS.BusinessLogic.Services;
using VolleyMS.Core.Models;
using VolleyMS.Core.Repositories;
using VolleyMS.Core.Services;
using VolleyMS.DataAccess;
using VolleyMS.DataAccess.Repositories;
using VolleyMS.Extensions;
using VolleyMS.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDataBase();
// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerAuth();

builder.Services.AddScoped<JwtService>();
builder.Services.Configure<AuthConfiguration>(builder.Configuration
                                                     .GetSection("AuthConfiguration"));
builder.Services.AddAuth(builder.Configuration);

builder.Logging.AddConsole();
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
builder.Services.AddScoped<IClubService, ClubService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IClubRepository, ClubRepository>();

//builder.Services.AddScoped<ClubService>();
//builder.Services.AddScoped<NotificationRepository>();
//builder.Services.AddScoped<NotificationService>();
//builder.Services.AddScoped<JoinClubRepository>();
//builder.Services.AddScoped<JoinClubService>();
builder.Services.AddMediatR(cfg => 
    cfg.RegisterServicesFromAssembly(typeof(RegistrationCommandHandler).Assembly));

builder.Services.AddHttpContextAccessor();

var app = builder.Build();
app.UseMiddleware<ExceptionHandlerMiddleware>();

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.MapControllers();

app.Run();

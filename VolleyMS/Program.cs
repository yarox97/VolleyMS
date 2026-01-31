using FluentValidation;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Identity;
using Quartz;
using System.Text.Json.Serialization;
using VolleyMS.BusinessLogic.BackgorundJobs;
using VolleyMS.BusinessLogic.Features.Users.Authorisation;
using VolleyMS.BusinessLogic.Features.Users.Authorisation.Registration;
using VolleyMS.BusinessLogic.Services;
using VolleyMS.Core.Models;
using VolleyMS.Core.Repositories;
using VolleyMS.Core.Services;
using VolleyMS.DataAccess;
using VolleyMS.DataAccess.Interceptors;
using VolleyMS.DataAccess.Repositories;
using VolleyMS.Extensions;
using VolleyMS.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<ConvertDomainEventToOutboxMessageInterceptor>();

builder.Services.AddDataBase();

builder.Services.AddQuartz(configure =>
{
    var jobKey = new JobKey(nameof(ProcessOutboxMessagesJob));

    configure
        .AddJob<ProcessOutboxMessagesJob>(opts => opts.WithIdentity(jobKey))
        .AddTrigger(trigger =>
            trigger.ForJob(jobKey)
                .WithSimpleSchedule(schedule =>
                    schedule.WithIntervalInSeconds(10)
                            .RepeatForever()));
});

builder.Services.AddQuartzHostedService(options =>
{
    options.WaitForJobsToComplete = true;
});

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

builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly, includeInternalTypes: true);

builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
builder.Services.AddScoped<IClubService, ClubService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IClubRepository, ClubRepository>();
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<IUserNotificationRepository, UserNotificationRepository>();
builder.Services.AddScoped<IJoinClubRepository, JoinClubRepository>();
builder.Services.AddScoped<IUserClubRepository, UserClubRepository>();
builder.Services.AddScoped<IContractRepository, ContractRepository>();
builder.Services.AddTransient<IEmailService, EmailService>();

builder.Services.AddMediatR(cfg => 
    cfg.RegisterServicesFromAssembly(typeof(RegistrationCommandHandler).Assembly));

builder.Services.AddHttpContextAccessor();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy =>
        {
            policy
                .WithOrigins("http://localhost:5173")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        });
});

var app = builder.Build();

app.UseCors("AllowFrontend");
app.UseMiddleware<ExceptionHandlerMiddleware>();

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();
app.UseRouting();

app.UseCookiePolicy(new CookiePolicyOptions
{
    MinimumSameSitePolicy = SameSiteMode.Strict,
    HttpOnly = HttpOnlyPolicy.Always,
    Secure = CookieSecurePolicy.Always
});

app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.MapControllers();


app.Run();

using VolleyMS.BusinessLogic.Authorisation;
using VolleyMS.BusinessLogic.Services;
using VolleyMS.DataAccess;
using VolleyMS.DataAccess.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDataBase();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<JwtService>();
builder.Services.Configure<AuthConfiguration>(builder.Configuration
                                                     .GetSection("AuthConfiguration"));
builder.Services.AddAuth(builder.Configuration);

builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<ClubRepository>();
builder.Services.AddScoped<ClubService>();

var app = builder.Build();
app.UseAuthentication();
app.UseAuthorization();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

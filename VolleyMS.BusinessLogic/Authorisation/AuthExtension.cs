using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace VolleyMS.BusinessLogic.Authorisation
{
    public static class AuthExtension
    {
        public static IServiceCollection AddAuth(this IServiceCollection services, IConfiguration configuration)
        {
            var authConfiguration = configuration.GetSection(nameof(AuthConfiguration)).Get<AuthConfiguration>();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authConfiguration.SecretKey))
                    };
                });
            services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminOnly", policy => policy.RequireRole("UserType", UserType.Admin.ToString()));
                options.AddPolicy("AdminAndCoach", policy => policy.RequireRole("UserType", UserType.Coach.ToString(), UserType.Admin.ToString()));
                options.AddPolicy("CoachOnly", policy => policy.RequireRole("UserType", UserType.Coach.ToString()));
                options.AddPolicy("PlayerOnly", policy => policy.RequireRole("UserType", UserType.Player.ToString()));
            });

            return services;
        }
    }
}

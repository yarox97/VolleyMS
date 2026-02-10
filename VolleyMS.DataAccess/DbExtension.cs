using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using VolleyMS.Core.Repositories;
using VolleyMS.DataAccess.Interceptors;

namespace VolleyMS.DataAccess
{
    public static class DbExtension
    {
        public static void AddDataBase(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.AddScoped<IUnitOfWork, UnitOfWork>();
            serviceCollection.AddDbContext<VolleyMsDbContext>((sp, o) =>
            {
                var interceptor = sp.GetService<ConvertDomainEventToOutboxMessageInterceptor>();

                var dataBaseName = configuration["Database:DatabaseName"];
                var host = configuration["Database:Host"];
                var port = configuration["Database:Port"];
                var username = configuration["Database:Username"];
                var password = configuration["Database:Password"];

                o.UseNpgsql($"host={host};port={port};Database={dataBaseName};Username={username};password={password}")
                    .AddInterceptors(interceptor);

                o.EnableSensitiveDataLogging();
                o.LogTo(Console.WriteLine, LogLevel.Information);
            });
        }
    }
}

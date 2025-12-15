using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using VolleyMS.Core.Repositories;

namespace VolleyMS.DataAccess
{
    public static class DbExtension
    {
        public static void AddDataBase(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IUnitOfWork, UnitOfWork>();
            serviceCollection.AddDbContext<VolleyMsDbContext>(o =>
            {
                o.UseNpgsql("host=localhost;port=5432;Database=VolleyMsDb2;Username=postgres;password=GabeNewwel228");

                o.EnableSensitiveDataLogging();
                o.LogTo(Console.WriteLine, LogLevel.Information);
            });
        }
    }
}

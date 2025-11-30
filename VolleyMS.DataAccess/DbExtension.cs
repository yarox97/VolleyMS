using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace VolleyMS.DataAccess
{
    public static class DbExtension
    {
        public static void AddDataBase(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddDbContext<VolleyMsDbContext>(o =>
            {
                o.UseNpgsql("host=localhost;port=5432;Database=VolleyMsDb;Username=postgres;password=GabeNewwel228");
            });
        }
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VolleyMS.DataAccess
{
    public static class Extensions
    {
        public static void AddDataBase(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddDbContext<VolleyMsDbContext>(o =>
            {
                o.UseNpgsql("host=localhost port=5432 dbname=VolleyMsDb user=postgres password=1234");
            });
        }
    }
}

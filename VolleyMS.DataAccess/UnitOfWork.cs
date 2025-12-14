using VolleyMS.Core.Repositories;

namespace VolleyMS.DataAccess
{
    internal sealed class UnitOfWork : IUnitOfWork
    {
        private readonly VolleyMsDbContext _volleyMsDbContext;
        public UnitOfWork(VolleyMsDbContext volleyMsDbContext)
        {
            _volleyMsDbContext = volleyMsDbContext;
        }
        public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await _volleyMsDbContext.SaveChangesAsync(cancellationToken);
        }
    }
}

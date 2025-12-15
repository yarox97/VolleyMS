using Microsoft.EntityFrameworkCore;
using VolleyMS.Core.Common;
using VolleyMS.Core.Repositories;

namespace VolleyMS.DataAccess.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        protected readonly VolleyMsDbContext _volleyMsDbContext;
        protected readonly DbSet<T> _dbSet;

        public GenericRepository(VolleyMsDbContext volleyMsDbContext)
        {
            _volleyMsDbContext = volleyMsDbContext;
            _dbSet = volleyMsDbContext.Set<T>();
        }

        public async Task<T?> GetByIdAsync(Guid id)
        {
            return await _dbSet.FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public async Task UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _dbSet.Where(e => e.Id == id).ExecuteDeleteAsync();
        }
    }
}
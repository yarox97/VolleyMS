using VolleyMS.Core.Models;

namespace VolleyMS.Core.Repositories
{
    public interface IUserTaskRepository : IGenericRepository<UserTask>
    {
        public Task<UserTask?> GetByIdWithDetailsAsync(Guid id, CancellationToken cancellationToken);
    }
}

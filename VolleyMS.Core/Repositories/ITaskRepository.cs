using VolleyMS.Core.Shared;

namespace VolleyMS.Core.Repositories
{
    public interface ITaskRepository : IGenericRepository<Models.Task>
    {
        public Task<Models.Task?> GetWithAttachments(Guid taskId);
        public Task<Models.Task?> GetByIdWithReceiversAsync(Guid taskId);
        public Task<Models.Task?> GetByIdWithDetailsAsync(Guid taskId, CancellationToken cancellationToken);
        Task<PaginatedResult<Core.Models.Task>> GetTasksByUserIdAsync(Guid userId, int page, int pageSize, CancellationToken ct);
        Task<PaginatedResult<Core.Models.Task>> GetTasksByClubIdAsync(Guid clubId, int page, int pageSize, CancellationToken ct);
    }
}

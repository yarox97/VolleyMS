using Microsoft.EntityFrameworkCore;
using VolleyMS.Core.Models;
using VolleyMS.Core.Requests;
using VolleyMS.Core.Shared;

namespace VolleyMS.Core.Repositories
{
    public interface ITaskRepository : IGenericRepository<Models.Task>
    {
        public Task<Models.Task?> GetWithAttachments(Guid taskId);
        public Task<Models.Task?> GetByIdWithReceiversAsync(Guid taskId);
        public Task<Models.Task?> GetByIdWithDetailsAsync(Guid taskId, CancellationToken cancellationToken);
        public Task<Models.Task?> GetByIdWithDetailsAndResponses(Guid taskId, CancellationToken cancellationToken);
        Task<PaginatedResult<Core.Models.Task>> GetTasksByUserIdAsync(Guid userId, int page, int pageSize, string filter, CancellationToken ct);
        Task<PaginatedResult<Core.Models.Task>> GetTasksByClubIdAsync(Guid clubId, int page, int pageSize, CancellationToken ct);
        public Task<UserTask?> GetUserTaskByTaskIdAndUserIdAsync(Guid taskId, Guid userId, CancellationToken cancellationToken);
        public Task<UserTask?> GetUserTaskByIdAsync(Guid userTaskId, CancellationToken cancellationToken);
        public Task<(int MatchesPlayed, int TasksFinished, int TotalTasks)> GetUserTaskStatsAsync(Guid userId, CancellationToken ct);
        public Task<List<TaskMonthlyStats>> GetStatsForLastMonthsAsync(Guid userId, int monthsCount);
    }
}

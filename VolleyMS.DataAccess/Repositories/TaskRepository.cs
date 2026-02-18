using Microsoft.EntityFrameworkCore;
using VolleyMS.Core.Repositories;
using VolleyMS.Core.Shared;

namespace VolleyMS.DataAccess.Repositories
{
    public class TaskRepository : GenericRepository<Core.Models.Task>, ITaskRepository
    {
        public TaskRepository(VolleyMsDbContext volleyMsDbContext) : base(volleyMsDbContext)
        {
        }
        public async Task<Core.Models.Task?> GetWithAttachments(Guid taskId)
        {
            return await 
                _volleyMsDbContext.Tasks
                .Include(t => t.Attachments)
                    .ThenInclude(ta => ta.MediaFile)
                .FirstOrDefaultAsync(t => t.Id == taskId);
        }

        public async Task<Core.Models.Task?> GetByIdWithReceiversAsync(Guid taskId)
        {
            return await _volleyMsDbContext.Tasks
                .Include(t => t.Receivers)
                .FirstOrDefaultAsync(t => t.Id == taskId);
        }

        public async Task<Core.Models.Task?> GetByIdWithDetailsAsync(Guid taskId, CancellationToken cancellationToken)
        {
            return await _volleyMsDbContext.Tasks
                .Include(t => t.Club)
                .Include(t => t.Sender)
                .Include(t => t.Attachments)
                .Include(t => t.Receivers)
                    .ThenInclude(ut => ut.User)
                .FirstOrDefaultAsync(t => t.Id == taskId, cancellationToken);
        }

        public async Task<PaginatedResult<Core.Models.Task>> GetTasksByUserIdAsync(Guid userId, int page, int pageSize, CancellationToken ct)
        {
            var query = _volleyMsDbContext.Tasks
                .Include(t => t.Club)
                .Include(t => t.Sender)
                .Where(t => t.Receivers.Any(r => r.UserId == userId));

            var totalCount = await query.CountAsync(ct);

            var items = await query
                .OrderByDescending(t => t.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(ct);

            return new PaginatedResult<Core.Models.Task>(items, page, pageSize, totalCount);
        }

        public async Task<PaginatedResult<Core.Models.Task>> GetTasksByClubIdAsync(Guid clubId, int page, int pageSize, CancellationToken cancellationToken)
        {
            var query = _volleyMsDbContext.Tasks
                .Include(t => t.Club)
                .Include(t => t.Sender)
                .Where(t => t.ClubId == clubId);

            var totalCount = await query.CountAsync(cancellationToken);

            var items = await query
                .OrderByDescending(t => t.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return new PaginatedResult<Core.Models.Task>(items, page, pageSize, totalCount);
        }

    }
}

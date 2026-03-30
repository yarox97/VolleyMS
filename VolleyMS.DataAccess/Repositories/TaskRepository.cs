using Microsoft.EntityFrameworkCore;
using VolleyMS.Core.Models;
using VolleyMS.Core.Repositories;
using VolleyMS.Core.Requests;
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
                    .ThenInclude(ta => ta.MediaFile)
                .Include(t => t.Receivers)
                    .ThenInclude(ut => ut.User)
                .FirstOrDefaultAsync(t => t.Id == taskId, cancellationToken);
        }

        public async Task<PaginatedResult<Core.Models.Task>> GetTasksByUserIdAsync(Guid userId, int page, int pageSize, string filter, CancellationToken ct)
        {
            var query = _volleyMsDbContext.Tasks
                .AsNoTracking()
                .Include(t => t.Club)
                .Include(t => t.Sender)
                .Include(t => t.Receivers)
                    .ThenInclude(r => r.TaskResponses)
                .Where(t => t.Receivers.Any(r => r.UserId == userId));

            if (filter == "pending")
            {
                query = query.Where(t => t.Receivers.Any(r =>
                    r.UserId == userId &&
                    r.Status == TaskStatus.Uncompleted &&
                    !r.TaskResponses.Any()));
            }
            else if (filter == "in_progress")
            {
                query = query.Where(t => t.Receivers.Any(r =>
                    r.UserId == userId &&
                    (
                        (r.Status == TaskStatus.Uncompleted && r.TaskResponses.Any()) || 
                        r.Status == TaskStatus.Unconfirmed 
                    )));
            }
            else if (filter == "returned")
            {
                query = query.Where(t => t.Receivers.Any(r =>
                    r.UserId == userId &&
                    r.Status == TaskStatus.Returned)); 
            }
            else if (filter == "completed")
            {
                query = query.Where(t => t.Receivers.Any(r =>
                    r.UserId == userId &&
                    (r.Status == TaskStatus.Completed || r.Status == TaskStatus.Confirmed)));
            }
            else if (filter == "overdued")
            {
                query = query.Where(t => t.Receivers.Any(r =>
                    r.UserId == userId &&
                    r.Status == TaskStatus.Overdued));
            }

            var totalCount = await query.CountAsync(ct);

            var items = await query
            .OrderBy(t =>
                t.Receivers.Any(r => r.UserId == userId && r.Status == TaskStatus.Returned) ? 1 :
                t.Receivers.Any(r => r.UserId == userId && r.Status == TaskStatus.Overdued) ? 2 :  
                t.Receivers.Any(r => r.UserId == userId && r.Status == TaskStatus.Uncompleted) ? 3 :
                t.Receivers.Any(r => r.UserId == userId && r.Status == TaskStatus.Unconfirmed) ? 4 : 
                5) 

            .ThenByDescending(t => t.TaskPriority)

            .ThenBy(t => t.TaskSchedule.EndDate)

            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

            return new PaginatedResult<Core.Models.Task>(items, totalCount, page, pageSize);
        }

        public async Task<PaginatedResult<Core.Models.Task>> GetTasksByClubIdAsync(Guid clubId, int page, int pageSize, CancellationToken cancellationToken)
        {
            var query = _volleyMsDbContext.Tasks
                .Include(t => t.Club)
                .Include(t => t.Sender)
                .Where(t => t.ClubId == clubId);

            var totalCount = await query.CountAsync(cancellationToken);

            var items = await query
                .OrderByDescending(t => t.TaskSchedule.EndDate == null || t.TaskSchedule.EndDate >= DateTime.UtcNow)

                .ThenByDescending(t => t.TaskPriority)

                .ThenBy(t => t.TaskSchedule.EndDate)

                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return new PaginatedResult<Core.Models.Task>(items, totalCount, page, pageSize);
        }

        public async Task<UserTask?> GetUserTaskByTaskIdAndUserIdAsync(Guid taskId, Guid userId, CancellationToken cancellationToken)
        {
            return await _volleyMsDbContext.UserTasks
                .Include(ut => ut.Task)
                    .ThenInclude(ut => ut.Receivers)
                .Include(ut => ut.TaskResponses)
                    .ThenInclude(r => r.Attachments)
                        .ThenInclude(a => a.Media)
                .FirstOrDefaultAsync(ut => ut.TaskId == taskId && ut.UserId == userId, cancellationToken);
        }

        public async Task<UserTask?> GetUserTaskByIdAsync(Guid userTaskId, CancellationToken cancellationToken)
        {
            return await _volleyMsDbContext.UserTasks
            .Include(ut => ut.Task)
            .Include(ut => ut.User)
            .Include(ut => ut.TaskResponses) 
                .ThenInclude(r => r.Attachments)
                    .ThenInclude(a => a.Media)
            .FirstOrDefaultAsync(ut => ut.Id == userTaskId, cancellationToken);
        }

        public async Task<Core.Models.Task?> GetByIdWithDetailsAndResponses(Guid taskId, CancellationToken cancellationToken)
        {
            return await _volleyMsDbContext.Tasks
                .Include(t => t.Club)
                .Include(t => t.Sender)
                .Include(t => t.Attachments)
                    .ThenInclude(ta => ta.MediaFile)
                .Include(t => t.Receivers)
                    .ThenInclude(r => r.User)
                .Include(t => t.Receivers)
                    .ThenInclude(r => r.TaskResponses)
                        .ThenInclude(tr => tr.Attachments)
                            .ThenInclude(tra => tra.Media)
                .FirstOrDefaultAsync(t => t.Id == taskId, cancellationToken);
        }

        public async Task<(int MatchesPlayed, int TasksFinished, int TotalTasks)> GetUserTaskStatsAsync(Guid userId, CancellationToken ct)
        {
            var stats = await _volleyMsDbContext.UserTasks
                .Where(r => r.UserId == userId)
                .GroupBy(r => 1) 
                .Select(g => new
                {
                    TotalTasks = g.Count(),
                    TasksFinished = g.Count(r => r.Status == TaskStatus.Completed || r.Status == TaskStatus.Confirmed),
                    MatchesPlayed = g.Count(r => r.Task.TaskType == TaskType.Match && (r.Status == TaskStatus.Completed || r.Status == TaskStatus.Confirmed))
                })
                .FirstOrDefaultAsync(ct);

            if (stats == null) return (0, 0, 0);

            return (stats.MatchesPlayed, stats.TasksFinished, stats.TotalTasks);
        }

        public async Task<List<TaskMonthlyStats>> GetStatsForLastMonthsAsync(Guid userId, int monthsCount)
        {
            var now = DateTime.UtcNow;
            var startDate = new DateTime(now.Year, now.Month, 1, 0, 0, 0, DateTimeKind.Utc)
                                .AddMonths(-monthsCount + 1);

            var groupedData = await _volleyMsDbContext.UserTasks
                .Where(ut => ut.UserId == userId
                          && ut.CompletedAt != null
                          && ut.CompletedAt >= startDate
                          && (ut.Status == TaskStatus.Completed || ut.Status == TaskStatus.Confirmed))
                .GroupBy(ut => new {
                    Year = ut.CompletedAt.Value.Year,
                    Month = ut.CompletedAt.Value.Month,
                    TaskType = ut.Task.TaskType 
                })
                .Select(g => new {
                    g.Key.Year,
                    g.Key.Month,
                    g.Key.TaskType,
                    Count = g.Count()
                })
                .ToListAsync();

            return groupedData
                .Select(g => new TaskMonthlyStats(
                    g.Year,
                    g.Month,
                    g.TaskType.ToString(),
                    g.Count
                ))
                .ToList();
        }
    }
}

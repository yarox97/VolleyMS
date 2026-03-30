using Microsoft.EntityFrameworkCore;
using VolleyMS.Core.Models;
using VolleyMS.Core.Repositories;

namespace VolleyMS.DataAccess.Repositories
{
    public class UserTaskRepository : GenericRepository<UserTask>, IUserTaskRepository
    {
        public UserTaskRepository(VolleyMsDbContext volleyMsDbContext) : base(volleyMsDbContext)
        {
        }

        public async Task<UserTask?> GetByIdWithDetailsAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _volleyMsDbContext.UserTasks
            .Include(ut => ut.User)

            .Include(ut => ut.Task)
                .ThenInclude(t => t.Club)
            .Include(ut => ut.Task)
                .ThenInclude(t => t.Sender)
            .Include(ut => ut.Task)
                .ThenInclude(t => t.TaskSchedule)
            .FirstOrDefaultAsync(ut => ut.Id == id, cancellationToken);
        }
    }
}

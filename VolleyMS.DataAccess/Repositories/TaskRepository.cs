using Microsoft.EntityFrameworkCore;
using VolleyMS.Core.Repositories;

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

        public Task<Core.Models.Task?> GetAllClubTasks(Guid clubId)
        {
            throw new NotImplementedException();
        }

        public Task<Core.Models.Task?> GetAllUserTasks(Guid userId)
        {
            throw new NotImplementedException();
        }

    }
}

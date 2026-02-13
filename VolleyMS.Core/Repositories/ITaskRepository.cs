namespace VolleyMS.Core.Repositories
{
    public interface ITaskRepository : IGenericRepository<Models.Task>
    {
        public Task<Models.Task?> GetWithAttachments(Guid taskId);
        public Task<Models.Task?> GetAllUserTasks(Guid userId);
        public Task<Models.Task?> GetAllClubTasks(Guid clubId);
    }
}

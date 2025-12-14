using VolleyMS.Core.Models;

namespace VolleyMS.Core.Repositories
{
    public interface IUserRepository : IGenericRepository<User>
    {
        public Task<bool> IsLoginTaken(string userName);
        public Task<User?> GetByUserNameAsync(string userName);
    }
}
using VolleyMS.Core.Models;

namespace VolleyMS.DataAccess.Repositories
{
    public interface IUserRepository
    {
        public Task<bool> IsLoginTaken(string userName);
        public Task AddUser(User user);
    }
}
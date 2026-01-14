using VolleyMS.Core.Models;

namespace VolleyMS.Core.Repositories
{
    public interface IUserRepository : IGenericRepository<User>
    {
        public Task<bool> IsLoginTaken(string userName);
        public Task<bool> IsEmailTaken(string email);
        public Task<User?> GetByUserNameWithClubsAsync(string userName);
        public Task<User?> GetByIdWithClubsAsync(Guid userId);
    }
}
using VolleyMS.Core.Models;

namespace VolleyMS.Core.Repositories
{
    public interface IUserClubRepository : IGenericRepository<UserClub>
    {
        public Task<UserClub> GetByIds(Guid clubId, Guid userId);
    }
}

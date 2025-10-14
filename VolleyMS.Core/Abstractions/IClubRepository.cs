using VolleyMS.Core.Models;

namespace VolleyMS.DataAccess.Repositories
{
    public interface IClubRepository
    {
        public Task Create(Club club);
        //public Task Update(Club club);
        public Task Delete(Guid clubId);
        public Task AddUser(User user, string joinCode);
    }
}
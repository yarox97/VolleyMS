using VolleyMS.Core.Models;

namespace VolleyMS.DataAccess.Repositories
{
    public interface IClubRepository
    {
        public Task Create(Club club, Guid creatorId);
        //public Task Update(Club club);
        public Task Delete(Guid clubId);
        //public Task AddUser(User user, string joinCode);
        public Task<Club?> Get(string joinCode);
    }
}
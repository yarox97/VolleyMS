using Microsoft.EntityFrameworkCore;
using VolleyMS.Core.Models;
using VolleyMS.Core.Repositories;

namespace VolleyMS.DataAccess.Repositories
{
    public class UserClubRepository : GenericRepository<UserClub>, IUserClubRepository
    {
        public UserClubRepository(VolleyMsDbContext volleyMsDbContext) : base(volleyMsDbContext)
        {
        }

        public async Task<UserClub?> GetByIds(Guid clubId, Guid userId)
        {
            return await 
                _volleyMsDbContext
                .UserClubs
                .FirstOrDefaultAsync(uc => uc.ClubId == clubId && uc.UserId == userId);
        }
    }
}

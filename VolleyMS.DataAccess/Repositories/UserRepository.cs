using Microsoft.EntityFrameworkCore;
using VolleyMS.Core.Repositories;
using VolleyMS.Core.Models;

namespace VolleyMS.DataAccess.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(VolleyMsDbContext volleyMsDbContext) : base(volleyMsDbContext)
        {
        }
        public async Task<User?> GetByUserNameWithClubsAsync(string userName)
        {
            return await _volleyMsDbContext.Users
                .AsNoTracking()
                .Include(u => u.UserClubs)
                .ThenInclude(u => u.Club)
                .FirstOrDefaultAsync(u => u.UserName.ToLower() == userName.ToLower());
        }

        public async Task<User?> GetByIdWithClubsAsync(Guid userId)
        {
            return await _volleyMsDbContext.Users
                .Include(u => u.UserClubs)
                .ThenInclude(uc => uc.Club)
                .FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<bool> IsLoginTaken(string userName)
        {
            return await _volleyMsDbContext.Users
                .AnyAsync(u => u.UserName.ToLower() == userName.ToLower());                           
        }

        public async Task<bool> IsEmailTaken(string email)
        {
            return await _volleyMsDbContext.Users
                .AnyAsync(u => u.Email.ToLower() == email.ToLower());
        }

        public async Task<List<User>> GetUsersByIdsAsync(IEnumerable<Guid> userIds, CancellationToken cancellationToken = default)
        {
            if (userIds == null || !userIds.Any())
            {
                return new List<User>();
            }

            var distinctIds = userIds.Distinct().ToList();

            return await _volleyMsDbContext.Users
                .Where(user => distinctIds.Contains(user.Id))
                .ToListAsync(cancellationToken);
        }
    }
}

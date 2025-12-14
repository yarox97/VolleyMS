using Microsoft.EntityFrameworkCore;
using VolleyMS.Core.Repositories;
using VolleyMS.Core.Models;

namespace VolleyMS.DataAccess.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(VolleyMsDbContext context) : base(context)
        {
        }
        public async Task<User?> GetByUserNameAsync(string userName)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.UserName.ToLower() == userName.ToLower());
        }
        public async Task<bool> IsLoginTaken(string userName)
        {
            return await _context.Users
                .AnyAsync(u => u.UserName.ToLower() == userName.ToLower());                           
        }
    }
}

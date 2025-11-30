using Microsoft.EntityFrameworkCore;
using VolleyMS.Core.Models;
using VolleyMS.DataAccess.Mappers;

namespace VolleyMS.DataAccess.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly VolleyMsDbContext _context;
        public UserRepository(VolleyMsDbContext context)
        {
            _context = context;
        }
        public async Task<bool> IsLoginTaken(string userName)
        {
            return await _context.Users
                .AnyAsync(u => u.UserName.ToLower() == userName.ToLower());                           
        }
        public async Task AddUser(User user)
        {
            var userEntity = UserMapper.ToEntity(user);
            if(userEntity == null)
            {
                throw new ArgumentNullException(nameof(userEntity), "User entity cannot be null");
            }

            await _context.Users.AddAsync(userEntity);
            await _context.SaveChangesAsync();
        }

        public async Task<User?> Get(string userName)
        {
            var userEntity = await _context.Users.FirstOrDefaultAsync(u => u.UserName == userName);
            return userEntity is not null ? UserMapper.ToDomain(userEntity) : null;
        }

        public async Task<User?> Get(Guid Id)
        {
            var userEntity = await _context.Users.FirstOrDefaultAsync(u => u.Id == Id);
            return userEntity is not null ? UserMapper.ToDomain(userEntity) : null;
        }

        public async Task Modify(string userName, User newUser)
        {
            var userEntity = await _context.Users.FirstOrDefaultAsync(u => u.UserName == userName);
            if (userEntity != null)
            {
                userEntity.Surname = newUser.Surname;
                userEntity.Password = newUser.Password;
                userEntity.Name = newUser.Name;
                userEntity.UserType = newUser.UserType;
                userEntity.UpdatedAt = DateTime.UtcNow; // To make domain event for updateAt
                await _context.SaveChangesAsync();
                return;
            }
            throw new Exception("User you want to modify wasn't found!");
        }
    }
}

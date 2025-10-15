using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VolleyMS.Core.Models;
using VolleyMS.DataAccess.Entities;

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
            return await _context.Users.AnyAsync(u => u.UserName.ToLower() == userName.ToLower());                           
        }
        public async Task AddUser(User user)
        {
            var userModel = new UserModel
            {
                UserName = user.UserName,
                Password = user.Password,
                userType = user.UserType,
                Name = user.Name,
                Surname = user.Surname
            };
            await _context.Users.AddAsync(userModel);
            await _context.SaveChangesAsync();
        }

        public async Task<User?> GetByUserName(string userName)
        {
            var userEntity = await _context.Users.FirstOrDefaultAsync(u => u.UserName.ToLower() == userName.ToLower());

            return userEntity is not null ? User.Create(userEntity.Id, userEntity.UserName, userEntity.Password, userEntity.userType, userEntity.Name, userEntity.Surname).user : null;
        }
    }
}

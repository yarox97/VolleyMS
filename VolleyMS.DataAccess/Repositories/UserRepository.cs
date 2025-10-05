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
            var user = await _context.Users.AnyAsync(u => u.UserName == userName);
                                     
            if (user == default)
            {
                return false;
            }
            return true;
        }
        public async Task AddUser(User user)
        {
            var userModel = new UserModel
            {
                Id = user.Id,
                UserName = user.UserName,
                Password = user.Password,
                userType = user.UserType,
                Name = user.Name,
                Surname = user.Surname
            };
            await _context.SaveChangesAsync();
        }

    }
}

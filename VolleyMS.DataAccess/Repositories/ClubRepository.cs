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
    public class ClubRepository : IClubRepository
    {
        private readonly VolleyMsDbContext _context;
        public ClubRepository(VolleyMsDbContext context)
        {
            _context = context;
        }

        public async Task<bool> IfJoinCodeTaken(string joinCode)
        {
            return await _context.Clubs.AnyAsync(c => c.JoinCode == joinCode);
        }

        public async Task Create(Club club)
        {
            var clubModel = new ClubEntity
            {
                Name = club.Name,
                Description = club.Description,
                JoinCode = club.JoinCode,
                AvatarURL = club.AvatarURL,
                BackGroundURL = club.BackGroundURL
            };
            await _context.Clubs.AddAsync(clubModel);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(Guid clubId)
        {
            await _context.Clubs
                .Where(c => c.Id == clubId)
                .ExecuteDeleteAsync();

            await _context.SaveChangesAsync();
        }

        public async Task<Club?> GetClubByCode(string joinCode)
        {
            var clubEntity = await _context.Clubs.FirstOrDefaultAsync(c => c.JoinCode == joinCode);
             
            return clubEntity is not null ? Club.Create(clubEntity.Id, clubEntity.Name, clubEntity.JoinCode, clubEntity.Description, clubEntity.AvatarURL, clubEntity.BackGroundURL).club : null;
        }

        public async Task<bool> ContainsUser(Club club, User user)
        {
            return await _context.Clubs.AnyAsync(c => c.Users.Any(u => u.Id == user.Id));
        }

        public async Task AddUser(User user, string joinCode)
        {
            var userEntity = await _context.Users.FirstOrDefaultAsync(u => u.UserName == user.UserName);
            var ClubEntity = await _context.Clubs.Include(c => c.Users).FirstOrDefaultAsync(c => c.JoinCode == joinCode);

            ClubEntity.Users.Add(userEntity);
                
            await _context.SaveChangesAsync();
        }

        public async Task DeleteUser(Guid clubId, Guid userId)
        {
            await _context
                .Users
                .Where(u => u.Id == userId)
                .Where(u => u.Clubs
                .Any(c => c.Id == clubId))
                .ExecuteDeleteAsync();
        }
    }
}

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

        public async Task<Club?> GetById(Guid Id)
        {
            var clubEntity = await _context.Clubs.FirstOrDefaultAsync(c => c.Id == Id);

            return clubEntity is not null ? Club.Create(clubEntity.Id, clubEntity.Name, clubEntity.JoinCode, clubEntity.Description, clubEntity.AvatarURL, clubEntity.BackGroundURL).club : null;
        }

        public async Task<bool> ContainsUser(Club club, User user)
        {
            return await _context.UserClubs
                .AnyAsync(uc => uc.Club.Id == club.Id && uc.User.Id == user.Id);
        }

        public async Task AddUser(User user, Club club)
        {
            var UserEntity = await _context.Users.FirstOrDefaultAsync(u => u.Id == user.Id);
            var ClubEntity = await _context.Clubs.FirstOrDefaultAsync(c => c.Id == club.Id);

            if (UserEntity is not null && ClubEntity is not null)
            {
                var User_ClubEntity = new User_ClubsEntity 
                { 
                    Club = ClubEntity, 
                    User = UserEntity, 
                    ClubMemberRole = new List<ClubMemberRole>() { ClubMemberRole.Player } 
                };

                if(!await ContainsUser(club, user))
                {
                    _context.UserClubs.Add(User_ClubEntity);
                }
            }
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

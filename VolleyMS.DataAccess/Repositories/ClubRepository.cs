using Microsoft.EntityFrameworkCore;
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

        public async Task Create(Club club, Guid creatorId)
        {
            var clubModel = new ClubEntity
            {
                Id = club.Id,
                Name = club.Name,
                Description = club.Description,
                JoinCode = club.JoinCode,
                AvatarURL = club.AvatarURL,
                BackGroundURL = club.BackGroundURL,
                CreatorId = creatorId
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
            if (clubEntity == null)
            {
                throw new Exception("Error while extracting club data!");
            }
            var clubTuple = Club.Create(clubEntity.Id, clubEntity.Name, clubEntity.JoinCode, clubEntity.Description, clubEntity.AvatarURL, clubEntity.BackGroundURL);
            clubTuple.club.CreatorId = clubEntity.CreatorId;

            if (!string.IsNullOrEmpty(clubTuple.error))
            {
                throw new Exception("Error while extracting club data!");
            }

            return clubTuple.club;
        }

        public async Task<Club?> GetById(Guid Id)
        {
            var clubEntity = await _context.Clubs.FirstOrDefaultAsync(c => c.Id == Id);
            if (clubEntity == null)
            {
                throw new Exception("Error while extracting club data!");
            }
            var clubTuple = Club.Create(clubEntity.Id, clubEntity.Name, clubEntity.JoinCode, clubEntity.Description, clubEntity.AvatarURL, clubEntity.BackGroundURL);
            clubTuple.club.CreatorId = clubEntity.CreatorId;

            if (!string.IsNullOrEmpty(clubTuple.error))
            {
                throw new Exception("Error while extracting club data!");
            }

            return clubTuple.club;
        }

        public async Task<IList<User>> GetAllUsers(Guid clubId)
        {
            var userEntities = await _context.UserClubs
                .Select(uc => uc.User).Include(uc => uc.UserClubs)
                .Where(uc => uc.UserClubs.Any(c => c.Club.Id == clubId))
                .ToListAsync();

            var users = new List<User>();
            foreach (var userEntity in userEntities)
            {
                var userTuple = User.Create(userEntity.Id, userEntity.UserName, userEntity.Password, userEntity.userType, userEntity.Name, userEntity.Surname);
                if (string.IsNullOrEmpty(userTuple.error))
                {
                    users.Add(userTuple.user);
                }
            }
            return users;
        }

        public async Task<List<Guid>> GetUsersIdsByRole(Guid clubId, params ClubMemberRole[] clubMemberRoles)
        {
            var userIds = await _context.UserClubs
                .Where(uc => uc.Club.Id == clubId
                             && uc.ClubMemberRole.Any(role => clubMemberRoles.Contains(role)))
                .Select(uc => uc.User.Id)
                .ToListAsync();

            return userIds;
        }

        public async Task<bool> ContainsUser(Guid clubId, Guid userId)
        {
            return await _context.UserClubs
                .AnyAsync(uc => uc.Club.Id == clubId && uc.User.Id == userId);
        }

        public async Task AddUser(Guid userId, Guid clubId, ClubMemberRole clubMemberRole)
        {
            if (!await ContainsUser(clubId, userId))
            {
                var userClub = new User_ClubsEntity
                {
                    UserId = userId,
                    ClubId = clubId,
                    ClubMemberRole = new List<ClubMemberRole> { clubMemberRole }
                };

                _context.UserClubs.Add(userClub);

                var club = await GetById(clubId);
                club.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteUser(Guid clubId, Guid userId)
        {
            await _context.UserClubs
                .Where(uc => uc.User.Id == userId && uc.Club.Id == clubId)
                .ExecuteDeleteAsync();
        }
    }
}

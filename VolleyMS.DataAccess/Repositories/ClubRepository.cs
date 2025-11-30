using Microsoft.EntityFrameworkCore;
using VolleyMS.Core.Models;
using VolleyMS.DataAccess.Entities;
using VolleyMS.DataAccess.Mappers;

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
            var clubModel = ClubMapper.ToEntity(club);
            if(clubModel == null)
            {
                throw new ArgumentNullException(nameof(clubModel), "Club model cannot be null");
            }
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

        public async Task<Club?> Get(string joinCode)
        {
            var clubEntity = await _context.Clubs.FirstOrDefaultAsync(c => c.JoinCode == joinCode);
            var club = ClubMapper.ToDomain(clubEntity);
            return club;
        }
        public async Task<Club?> Get(Guid Id)
        {
            var clubEntity = await _context.Clubs.FirstOrDefaultAsync(c => c.Id == Id);
            var club = ClubMapper.ToDomain(clubEntity);
            return club;
        }

        public async Task<IList<User>> GetAllUsers(Guid clubId)
        {
            var userEntities = await _context.UserClubs
                .Select(uc => uc.User)
                .Where(uc => uc.UserClubs
                    .Any(c => c.Club.Id == clubId))
                .ToListAsync();

            var users = new List<User>();
            foreach (var userEntity in userEntities)
            {
                var user = UserMapper.ToDomain(userEntity);
                users.Add(user);
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
            var club = await Get(clubId) ?? throw new Exception("Club not found");
            club.UpdatedAt = DateTime.UtcNow;

            var userClub = new UserClubsEntity
            {
                UserId = userId,
                ClubId = clubId,
                ClubMemberRole = new List<ClubMemberRole> { clubMemberRole }
            };
            _context.UserClubs.Add(userClub);


            await _context.SaveChangesAsync();
        }

        public async Task DeleteUser(Guid clubId, Guid userId)
        {
            await _context.UserClubs
                .Where(uc => uc.User.Id == userId && uc.Club.Id == clubId)
                .ExecuteDeleteAsync();
            await _context.SaveChangesAsync();
        }
    }
}

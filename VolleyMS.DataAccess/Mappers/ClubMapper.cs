using VolleyMS.Core.Models;
using VolleyMS.DataAccess.Entities;

namespace VolleyMS.DataAccess.Mappers
{
    public static class ClubMapper
    {
        public static Club? ToDomain(ClubEntity? clubEntity)
        {
            if (clubEntity == null)
            {
                return null;
            }
            var club = Club.Create(
                clubEntity.Id,
                clubEntity.Name,
                clubEntity.JoinCode,
                clubEntity.Description,
                clubEntity.AvatarURL,
                clubEntity.BackGroundURL
            );
            club.CreatorId = clubEntity.CreatorId;
            club.CreatedAt = clubEntity.CreatedAt;
            club.UpdatedAt = clubEntity.UpdatedAt;
            club.DeletedAt = clubEntity.DeletedAt;
            return club;
        }

        public static ClubEntity? ToEntity(Club? club)
        {
            if (club == null)
            {
                return null;
            }
            var clubEntity = new ClubEntity
            {
                Id = club.Id,
                Name = club.Name,
                JoinCode = club.JoinCode,
                Description = club.Description,
                AvatarURL = club.AvatarURL,
                BackGroundURL = club.BackGroundURL,
                CreatorId = club.CreatorId,
                CreatedAt = club.CreatedAt,
                UpdatedAt = club.UpdatedAt,
                DeletedAt = club.DeletedAt
            };
            return clubEntity;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VolleyMS.Core.Models;
using VolleyMS.DataAccess.Entities;

namespace VolleyMS.DataAccess.Mappers
{
    public static class JoinClubMapper
    {
        public static JoinClubEntity ToEntity(JoinClub joinClub)
        {
            return new JoinClubEntity
            {
                Id = joinClub.Id,
                UserId = joinClub.UserId,
                ClubId = joinClub.ClubId,
                requestStatus = joinClub.JoinClubRequestStatus,
                CreatedAt = joinClub.CreatedAt,
                UpdatedAt = joinClub.UpdatedAt
            };
        }

        public static JoinClub ToDomain(JoinClubEntity joinClubEntity)
        {
            var joinClub = JoinClub.Create(
                joinClubEntity.requestStatus,
                joinClubEntity.UserId,
                joinClubEntity.ClubId
            );
            joinClub.CreatedAt = joinClubEntity.CreatedAt;
            joinClub.UpdatedAt = joinClubEntity.UpdatedAt;
            joinClub.CreatorId = joinClubEntity.CreatorId;

            return joinClub;
        }
    }
}

using VolleyMS.Core.Models;
using VolleyMS.DataAccess.Entities;

namespace VolleyMS.DataAccess.Mappers
{
    public static class UserMapper
    {
        public static User? ToDomain(UserEntity userEntity)
        {
            if (userEntity == null)
            {
                return null;
            }
            var user = User.Create(
                userEntity.Id,
                userEntity.UserName,
                userEntity.Password,
                userEntity.UserType,
                userEntity.Name,
                userEntity.Surname
            );
            user.CreatorId = userEntity.CreatorId;
            user.CreatedAt = userEntity.CreatedAt;
            user.UpdatedAt = userEntity.UpdatedAt;
            user.DeletedAt = userEntity.DeletedAt;

            return user;
        }
        public static UserEntity? ToEntity(User user)
        {
            if(user == null)
            {
                return null;
            }
            var userEntity = new UserEntity
            {
                Id = user.Id,
                UserName = user.UserName,
                Password = user.Password,
                UserType = user.UserType,
                Name = user.Name,
                Surname = user.Surname,
                CreatorId = user.CreatorId,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt,
                DeletedAt = user.DeletedAt
            };
            return userEntity;
        }

    }
}

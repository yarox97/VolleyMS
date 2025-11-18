using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VolleyMS.Core.Common;

namespace VolleyMS.DataAccess.Entities
{
    public class User_ClubsEntity : BaseEntity
    {
        public User_ClubsEntity()
        {
            ClubMemberRole = new List<ClubMemberRole>();
        }
        public Guid Id { get; set; }
        public UserEntity User { get; set; }
        public Guid UserId { get; set; }
        public ClubEntity Club { get; set; }
        public Guid ClubId { get; set; }
        public IList<ClubMemberRole> ClubMemberRole { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VolleyMS.Core.Common;
using VolleyMS.Core.Models;
using VolleyMS.DataAccess.Models;

namespace VolleyMS.DataAccess.Entities
{
    public class ClubEntity : BaseEntity
    {
        public ClubEntity()
        {
            Tasks = new List<TaskEntity>();
            UserClubs = new List<User_ClubsEntity>();
        }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string JoinCode { get; set; }
        public string? Description { get; set; }
        public string? AvatarURL { get; set; }
        public string? BackGroundURL { get; set;  } 
        public IList<TaskEntity> Tasks { get;  set; }
        public IList<User_ClubsEntity> UserClubs { get; set; }
    }
}

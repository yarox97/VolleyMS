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
        public string Name { get; set; } = string.Empty;
        public string JoinCode { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? AvatarURL { get; set; } = "..\\VolleyMS\\wwwroot\\Images\\DefaultAvarat.jpg"; // To make a dictionary avatar <-> path  
        public string? BackGroundURL { get; set;  } = "..\\VolleyMS\\wwwroot\\Images\\DefaultAvarat.jpg"; // Change to a real default bg pic from dict path
        public IList<TaskEntity> Tasks { get;  set; }
        public IList<User_ClubsEntity> UserClubs { get; set; }
    }
}

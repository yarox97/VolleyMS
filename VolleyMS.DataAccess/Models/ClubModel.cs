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
    public class ClubModel : BaseEntity
    {
        public ClubModel()
        {
            UserModels = new List<UserModel>();
            TaskModels = new List<TaskModel>();
        }
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string JoinCode { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? AvatarURL { get; set; } = "..\\VolleyMS\\wwwroot\\Images\\DefaultAvarat.jpg"; // To make a dictionary avatar <-> path  
        public string? BackGroundURL { get; set;  } = "..\\VolleyMS\\wwwroot\\Images\\DefaultAvarat.jpg"; // Change to a real default bg pic from dict path

        public IList<UserModel> UserModels { get; set; }
        public IList<TaskModel> TaskModels { get;  set; }
    }
}

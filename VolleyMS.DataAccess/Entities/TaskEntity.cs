using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VolleyMS.Core.Common;
using VolleyMS.DataAccess.Entities;

namespace VolleyMS.DataAccess.Models
{
    public class TaskEntity : BaseEntity
    {
        public TaskEntity()
        {
            Comments = new List<CommentEntity>();
            Sender = new UserEntity();
            Receivers = new List<UserEntity>();
            Club = new ClubEntity();
        }
        public Guid Id { get; set; }
        public TaskType TaskType { get; set; }
        public TaskStatus TaskStatus { get; set; }
        public PenaltyType PenaltyType { get; set; }
        public DateTime DueDate { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public IList<CommentEntity> Comments { get; set; }
        public Guid SenderId { get; set; }
        public UserEntity Sender { get; set; }
        public IList<UserEntity> Receivers { get; set; }
        public Guid ClubId { get; set; }
        public ClubEntity Club { get; set; }
    }
}

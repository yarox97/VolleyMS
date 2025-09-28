using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VolleyMS.Core.Common;
using VolleyMS.DataAccess.Entities;

namespace VolleyMS.DataAccess.Models
{
    public class TaskModel : AuditableFields
    {
        public TaskModel()
        {
            CommentModels = new List<CommentModel>();
            UserModel_sender = new UserModel();
            UserModel_receivers = new List<UserModel>();
            ClubModel = new ClubModel();
        }
        public Guid Id { get; set; }
        public TaskType TaskType { get; set; }
        public TaskStatus TaskStatus { get; set; }
        public PenaltyType PenaltyType { get; set; }
        public DateTime DueDate { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public IList<CommentModel> CommentModels { get; set; }
        public Guid SenderId { get; set; }
        public UserModel UserModel_sender { get; set; }
        public IList<UserModel> UserModel_receivers { get; set; }
        public Guid ClubId { get; set; }
        public ClubModel ClubModel { get; set; }
    }
}

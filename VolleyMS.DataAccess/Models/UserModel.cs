using VolleyMS.Core.Common;
using VolleyMS.Core.Models;
using VolleyMS.DataAccess.Models;

namespace VolleyMS.DataAccess.Entities

{
    public class UserModel : AuditableFields
    {
        public UserModel()
        {
            ContractModels = new List<ContractModel>();
            ClubModels = new List<ClubModel>();
            TaskModels = new List<TaskModel>();
            CommentModels = new List<CommentModel>();
        }
        public Guid Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public UserType userType { get; set; } = UserType.Player;
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;

        public IList<ContractModel> ContractModels { get; set; }
        public IList<ClubModel> ClubModels { get; set; }
        public IList<TaskModel> TaskModels { get; set; }
        public IList<CommentModel> CommentModels { get; set; }
    }
}

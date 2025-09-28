using VolleyMS.DataAccess.Entities;

namespace VolleyMS.DataAccess.Models
{
    public class CommentModel
    {
        public CommentModel()
        {
            TaskModel = new TaskModel();
            UserModel_sender = new UserModel();
        }
        public Guid Id { get; set; }
        public string Text { get; set; } = string.Empty;

        public Guid TaskId { get; set; }
        public TaskModel TaskModel;
        public Guid SenderId { get; set; }
        public UserModel UserModel_sender { get; set; }
    }
}
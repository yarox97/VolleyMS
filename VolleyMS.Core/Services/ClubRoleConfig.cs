using VolleyMS.Core.Enums;

namespace VolleyMS.Core.Services
{
    public class ClubRoleConfig
    {
        private static readonly Dictionary<ClubMemberRole, HashSet<ClubPermission>> _memberPermission = new Dictionary<ClubMemberRole, HashSet<ClubPermission>>
        {
            [ClubMemberRole.Player] = new HashSet<ClubPermission>(),

            [ClubMemberRole.Staff] = new HashSet<ClubPermission>
            {
                ClubPermission.CreateTask
            },

            [ClubMemberRole.Coach] = new HashSet<ClubPermission>
            {
                ClubPermission.CreateTask,
                ClubPermission.EditTask,
                ClubPermission.DeleteTask,

                ClubPermission.ChangeEventDetails,

                ClubPermission.AcceptJoinRequest,
                ClubPermission.ViewJoinRequests,
                ClubPermission.RejectJoinRequest
            },

            [ClubMemberRole.President] = Enum.GetValues<ClubPermission>().ToHashSet(),
            [ClubMemberRole.Creator] = Enum.GetValues<ClubPermission>().ToHashSet()
        };

        public static bool HasPermission(ClubMemberRole clubMemberRole, ClubPermission permission)
        {
            if (!_memberPermission.ContainsKey(clubMemberRole)) return false;
            return _memberPermission[clubMemberRole].Contains(permission);
        }
    }
}

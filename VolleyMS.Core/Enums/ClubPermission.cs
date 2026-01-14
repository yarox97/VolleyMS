namespace VolleyMS.Core.Enums
{
    public enum ClubPermission
    {
        // Member management
        KickMember,
        ChangeRole,
        AcceptJoinRequest,
        RejectJoinRequest,
        ViewJoinRequests,

        // Club management
        EditClubDetails,
        DeleteClub,

        // Task management
        CreateTask,
        EditTask,
        DeleteTask,

        // Contract management
        CreateContract,
        AssignContract,
        EditContract,
        DeleteContract,

        // Calendar management
        ChangeEventDetails, // change event = task datails (event is just a referecnce to task)
    }
}

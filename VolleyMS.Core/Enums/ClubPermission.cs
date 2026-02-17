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
        ViewTasks, // view all tasks in the club, not just assigned to the user

        // Contract management
        ViewContracts,
        CreateContract,
        AssignContract,
        EditContract,
        DeleteContract,

        // Calendar management
        ChangeEventDetails, // change event = task datails (event is just a referecnce to task)
    }
}

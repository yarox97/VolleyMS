public enum UserType
{
    Admin,
    Player
}

public enum TaskType
{
    Attendance,
    FileConfirmation,
    MarkAsRead
}
public enum TaskStatus
{
    Completed,
    Uncompleted,
    Returned,
    Confirmed
}

public enum PenaltyType
{
    None,
    PercentPremia,
    ValuePremia,
    PercentFine,
    ValueFine
}

public enum Currency
{
    USD,
    EUR,
    PLN,
    UAH,
    CZK
}

public enum DaysOfWeek
{
    Monday,
    Tuesday,
    Wednesday,
    Thursday,
    Friday,
    Saturday,
    Sunday
}

public enum NotificationCategory
{
    Informative,
    TaskLink,
    ClubJoinRequest,
    ClubJoinApproved,
    ClubJoinRejected,
    System,
    MessageReceived,
    EventReminder
}

public enum ClubMemberRole
{
    President,
    Coach,
    Staff,
    Player,
    Creator,
}

public enum JoinClubRequestStatus
{
    Pending,
    Approved,
    Rejected
}

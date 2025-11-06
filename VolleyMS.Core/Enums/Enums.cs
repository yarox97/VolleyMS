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

public enum NotificationType
{
    Informative,
    TaskLink,
    ClubJoinRequest,
    ClubJoinApproved,
    ClubJoinReject,
    System,
    MessageReceived,
    EventReminder
}

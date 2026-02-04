public enum UserType
{
    Admin,
    User
}

public enum 
    TaskType
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

[Flags]
public enum WeekDays
{
    None = 0,
    Monday = 1,   
    Tuesday = 2,  
    Wednesday = 4,    
    Thursday = 8,   
    Friday = 16,   
    Saturday = 32,  
    Sunday = 64,   

    // presets
    Weekdays = Monday | Tuesday | Wednesday | Thursday | Friday,
    Weekend = Saturday | Sunday,                                
    EveryDay = Weekdays | Weekend                             
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
    EventReminder,
    ContractAssigned,
    ContractEndedByUser,
    ContractEndedByClub,
    KickedFromClubByClub,
    KickedFromClubByUser
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

public enum ProofType
{
    None = 0,
    Image = 1,
    Video = 2,
    Document = 3,
    ExternalLink = 4 
}

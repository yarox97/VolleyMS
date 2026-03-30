public enum UserType
{
    Admin,
    User
}

public enum TaskType
{
    Attendance,
    GamePlan,
    Survey,
    Info,             
    DocumentSign,     
    Match              
}

public enum  TaskPriority
{
    Lowest,
    Low,
    Middle,
    High,
    Highest
}

public enum TaskStatus
{
    Completed,
    Uncompleted,
    Overdued,
    Returned,
    Unconfirmed,
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
    KickedFromClubByUser,

    TaskAssigned,
    TaskCompleted,
    TaskOverdue,
    TaskReturned,
    TaskModified,
    TaskConfirmed,
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

public enum PayslipStatus
{
    Draft = 1,      
    Approved = 2,   
    Paid = 3,       
    Cancelled = 4  
}

public enum AdjustmentType
{
    BonusValue = 1,    
    BounsPercent = 2,
    PenaltyValue = 3,
    PenaltyPercent = 4
}

public enum PlayerPosition
{
    None = 0,
    OutsideHitter = 1,  
    Setter = 2,         
    Libero = 3,         
    MiddleBlocker = 4,  
    OppositeHitter = 5  
}

public enum DominantHand
{
    None = 0,
    Right = 1,         
    Left = 2,           
    Ambidextrous = 3   
}
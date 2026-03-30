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
    Draft = 1,      // Черновик (текущий месяц, куда падают все новые штрафы/премии)
    Approved = 2,   // Утвержден (месяц закрыт, суммы зафиксированы)
    Paid = 3,       // Выплачен
    Cancelled = 4   // Отменен (в случае ошибки)
}

public enum AdjustmentType
{
    BonusValue = 1,    
    BounsPercent = 2,// Премия
    PenaltyValue = 3,
    PenaltyPercent = 4// Штраф
}

public enum PlayerPosition
{
    None = 0,
    OutsideHitter = 1,  // Доигровщик
    Setter = 2,         // Связующий
    Libero = 3,         // Либеро
    MiddleBlocker = 4,  // Центральный блокирующий
    OppositeHitter = 5  // Диагональный
}

public enum DominantHand
{
    None = 0,
    Right = 1,          // Правша
    Left = 2,           // Левша
    Ambidextrous = 3    // Амбидекстр (обе руки)
}
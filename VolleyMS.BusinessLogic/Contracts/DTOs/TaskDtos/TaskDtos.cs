
namespace VolleyMS.BusinessLogic.Contracts.DTOs.TaskDtos
{
    public record TaskDto(
        Guid Id,
        string Title,
        string Description,
        string TaskType,
        string TaskPriority,
        TaskScheduleDto Schedule,
        Guid ClubId,
        string ClubName,
        Guid SenderId,
        string SenderName,
        string? Payload,
        List<AttachmentDto> Attachments
    );

    public record TaskScheduleDto(
        DateTime StartDate,
        DateTime? EndDate,
        TimeSpan StartTime,
        TimeSpan? EndTime,
        List<DayOfWeek>? DaysOfWeek
    );

    public record AttachmentDto(
        string FileName,
        string Url,
        long Size
    );

    public record TaskDetailsDto(
        TaskDto Task,
        List<TaskReceiverDto> Receivers
    );

    public record TaskReceiverDto(
        Guid UserId,
        string FullName,
        TaskStatus TaskStatus,
        DateTime? CompletedAt
    );
}

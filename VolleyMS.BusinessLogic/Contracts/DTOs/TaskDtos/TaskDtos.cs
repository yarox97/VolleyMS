
namespace VolleyMS.BusinessLogic.Contracts.DTOs.TaskDtos
{
    public record TaskDto(
        Guid Id,
        string Title,
        string Description,
        string TaskType,
        string TaskPriority,
        string PenaltyType,
        string? PenaltyAmount,
        TaskScheduleDto Schedule,
        Guid ClubId,
        string ClubName,
        Guid SenderId,
        string SenderName,
        string? Payload,
        List<AttachmentDto> Attachments,

        string? MyStatus,
        bool HasResponses
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

    public record UserTaskResponseDto(
        Guid Id,
        string? Text,
        DateTime CreatedAt,
        List<AttachmentDto> Attachments
    );

    public record TaskReceiverDto(
        Guid UserTaskId,
        Guid UserId,
        string FullName,
        TaskStatus TaskStatus,
        DateTime? CompletedAt,
        List<UserTaskResponseDto> Responses,

        bool? HasActiveContract = false
    );
}

namespace VolleyMS.BusinessLogic.Contracts.DTOs
{
    public record PayslipDto(
        Guid Id,
        int Month,
        int Year,
        decimal BaseSalary,
        decimal TotalAmount, 
        string Status,     
        string ClubName,

        Guid? UserId = null,
        string? UserFullName = null
    );
}

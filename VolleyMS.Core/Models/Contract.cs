using VolleyMS.Core.Common;
using VolleyMS.Core.Errors;
using VolleyMS.Core.Shared;

namespace VolleyMS.Core.Models
{
    public class Contract : BaseEntity
    {
        private Contract() : base(Guid.Empty)
        {
        }

        private Contract(
            Guid id,
            decimal? monthlySalary,
            Currency currency,
            DateTime beginsFrom,
            DateTime endsBy,
            Guid userId,
            Guid clubId)
            : base(id)
        {
            MonthlySalary = monthlySalary;
            Currency = currency;
            BeginsFrom = beginsFrom;
            EndsBy = endsBy;
            UserId = userId;
            ClubId = clubId;
        }
        public decimal? MonthlySalary { get; private set; }
        public Currency Currency { get; private set; }

        public DateTime BeginsFrom { get; private set; }
        public DateTime EndsBy { get; private set; }

        public Guid UserId { get; private set; }
        public User User { get; private set; }

        // Исправил TeamId на ClubId
        public Guid ClubId { get; private set; }
        public Club Club { get; private set; }

        public static Result<Contract> Create(
            decimal? monthlySalary,
            Currency currency,
            DateTime beginsFrom,
            DateTime endsBy,
            Guid userId,
            Guid clubId)
        {
            // Валидация денег
            if (monthlySalary.HasValue && monthlySalary < 0)
            {
                return Result.Failure<Contract>(DomainErrors.Contract.NegativeSalary);
            }

            if (beginsFrom > endsBy)
            {
                return Result.Failure<Contract>(DomainErrors.Contract.InvalidDates);
            }

            if (userId == Guid.Empty || clubId == Guid.Empty)
            {
                return Result.Failure<Contract>(Error.NullValue);
            }

            // Проверка enum
            if (!Enum.IsDefined(typeof(Currency), currency))
            {
                return Result.Failure<Contract>(DomainErrors.Contract.InvalidDates);
            }

            var contract = new Contract(
                Guid.NewGuid(),
                monthlySalary,
                currency,
                beginsFrom,
                endsBy,
                userId,
                clubId
            );

            return Result.Success(contract);
        }
    }
}
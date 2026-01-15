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
            Currency? currency,
            DateTime beginsFrom,
            DateTime endsBy,
            User user,
            Club club)
            : base(id)
        {
            MonthlySalary = monthlySalary;
            Currency = currency;
            BeginsFrom = beginsFrom;
            EndsBy = endsBy;
            User = user;
            UserId = user.Id;
            Club = club;
            ClubId = club.Id;
        }
        public decimal? MonthlySalary { get; private set; }
        public Currency? Currency { get; private set; }

        public DateTime BeginsFrom { get; private set; }
        public DateTime EndsBy { get; private set; }

        public Guid UserId { get; private set; }
        public User User { get; private set; }

        public Guid ClubId { get; private set; }
        public Club Club { get; private set; }

        public bool IsActive =>
        DateTime.UtcNow >= BeginsFrom && DateTime.UtcNow <= EndsBy;

        public static Result<Contract> Create(
            decimal? monthlySalary,
            Currency? currency,
            DateTime beginsFrom,
            DateTime endsBy,
            User user,
            Club club)
        {
            if (monthlySalary.HasValue && monthlySalary < 0)
            {
                return Result.Failure<Contract>(DomainErrors.Contract.NegativeSalary);
            }

            if (beginsFrom > endsBy)
            {
                return Result.Failure<Contract>(DomainErrors.Contract.InvalidDates);
            }

            if(user == null)
            {
                return Result.Failure<Contract>(DomainErrors.User.UserNotFound);
            }

            if(club == null)
            {
                return Result.Failure<Contract>(DomainErrors.Club.ClubNotFound);
            }

            if (!Enum.IsDefined(typeof(Currency), currency))
            {
                return Result.Failure<Contract>(DomainErrors.Contract.InvalidCurrency);
            }

            var contract = new Contract(
                Guid.NewGuid(),
                monthlySalary,
                currency,
                beginsFrom,
                endsBy,
                user,
                club
            );

            return Result.Success(contract);
        }

        public bool isExpired()
        {
            return DateTime.UtcNow > EndsBy;
        }
    }
}
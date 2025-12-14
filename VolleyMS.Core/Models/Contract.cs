using VolleyMS.Core.Common;
using VolleyMS.Core.Exceptions;

namespace VolleyMS.Core.Models
{
    public class Contract : BaseEntity
    {
        private Contract() : base(Guid.Empty)
        {
        }
        private Contract(Guid id, decimal? monthlySalary, Currency currency, DateTime beginsFrom, DateTime endsBy)
            : base(id)
        {
            MontlySalary = monthlySalary;
            Currency = currency;
            BeginsFrom = beginsFrom;
            EndsBy = endsBy;
        }
        public decimal? MontlySalary { get; private set; }
        public Currency? Currency { get; private set; }
        public DateTime BeginsFrom { get; private set; } 
        public DateTime EndsBy { get; private set; }
        public Guid UserId { get; private set; }
        public User User { get; private set; }
        public Guid TeamId { get; private set; }
        public Club Club { get; private set; }

        public static Contract Create(Guid id, decimal? monthlySalary, Currency currency, DateTime beginsFrom, DateTime endsBy)
        {
            if (monthlySalary < 0)
            {
                throw new InvalidSalaryDomainException("Monthly salary cannot be negative!");
            }
            if(beginsFrom > endsBy)
            {
                throw new DateOutOfBoundsDomainException("Contract start date cannot be later than end date!");
            }

            return new Contract(id, monthlySalary, currency, beginsFrom, endsBy);
        }
    }
}
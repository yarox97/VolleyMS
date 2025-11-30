using VolleyMS.Core.Common;
using VolleyMS.Core.Exceptions;

namespace VolleyMS.Core.Models
{
    public class Contract : BaseEntity
    {
        private Contract(Guid id, decimal? monthlySalary, Currency currency, DateTime beginsFrom, DateTime endsBy)
            : base(id)
        {
            MontlySalary = monthlySalary;
            Currency = currency;
            BeginsFrom = beginsFrom;
            EndsBy = endsBy;
        }
        public decimal? MontlySalary { get; }
        public Currency? Currency { get; }
        public DateTime BeginsFrom { get; } 
        public DateTime EndsBy {get;}

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
using VolleyMS.Core.Common;

namespace VolleyMS.Core.Models
{
    public class Contract : BaseEntity
    {
        private Contract(Guid id, decimal? monthlySalary, Currency currency, DateTime beginsFrom, DateTime endsBy)
        {
            Id = id;
            MontlySalary = monthlySalary;
            Currency = currency;
            BeginsFrom = beginsFrom;
            EndsBy = endsBy;
        }
        public Guid Id { get; }
        public decimal? MontlySalary { get; }
        public Currency? Currency { get; }
        public DateTime BeginsFrom { get; } 
        public DateTime EndsBy {get;}

        public static (Contract contract, string error) Create(Guid id, decimal? monthlySalary, Currency currency, DateTime beginsFrom, DateTime endsBy)
        {
            string error = string.Empty;
            if (monthlySalary < 0)
            {
                error = "Salary can't be negative!";
            }
            if(beginsFrom > endsBy)
            {
                error = "Start of contract must be earlier than the end!";
            }

            return (new Contract(id, monthlySalary, currency, beginsFrom, endsBy), error);
        }
    }
}
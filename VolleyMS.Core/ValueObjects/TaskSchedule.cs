using VolleyMS.Core.Errors;
using VolleyMS.Core.Shared;
using VolleyMS.Core.ValueObjects;

namespace VolleyMS.Core.Models
{
    public class TaskSchedule : ValueObject
    {
        public DateTime StartDate { get; }
        public DateTime? EndDate { get; }
        public TimeSpan StartTime { get; }
        public TimeSpan EndTime { get; }

        public WeekDays Days { get; }

        private TaskSchedule(
            DateTime startDate,
            DateTime? endDate,
            WeekDays days,
            TimeSpan startTime,
            TimeSpan endTime)
        {
            StartDate = startDate;
            EndDate = endDate;
            Days = days;
            StartTime = startTime;
            EndTime = endTime;
        }

        public static Result<TaskSchedule> Create(
            DateTime startDate,
            DateTime? endDate,
            IEnumerable<DayOfWeek> daysOfWeek,
            TimeSpan startTime,
            TimeSpan endTime)
        {
            if (endDate.HasValue && startDate > endDate.Value)
                return Result.Failure<TaskSchedule>(DomainErrors.Task.InvalidDates);

            if (startTime > endTime)
                return Result.Failure<TaskSchedule>(DomainErrors.Task.InvalidTimes);

            if (endDate.HasValue && endDate.Value < DateTime.UtcNow)
                return Result.Failure<TaskSchedule>(DomainErrors.Task.InvalidDates);

            var weekDaysMask = ConvertListToFlags(daysOfWeek);

            if (weekDaysMask == WeekDays.None)
                return Result.Failure<TaskSchedule>(DomainErrors.Task.NoDaysSelected);

            return Result.Success(new TaskSchedule(startDate, endDate, weekDaysMask, startTime, endTime));
        }

        public bool IsActiveOn(DateTime date)
        {
            if (date.Date < StartDate.Date) return false;
            if (EndDate.HasValue && date.Date > EndDate.Value.Date) return false;

            return Contains(date.DayOfWeek);
        }

        public bool Contains(DayOfWeek systemDay)
        {
            var mask = MapSystemToWeekDays(systemDay);
            return (Days & mask) != 0;
        }

        public List<DayOfWeek> ToSystemDayOfWeekList()
        {
            var result = new List<DayOfWeek>();
            foreach (DayOfWeek systemDay in Enum.GetValues(typeof(DayOfWeek)))
            {
                if (Contains(systemDay))
                {
                    result.Add(systemDay);
                }
            }
            return result;
        }

        private static WeekDays ConvertListToFlags(IEnumerable<DayOfWeek> days)
        {
            if (days == null) return WeekDays.None;

            WeekDays mask = WeekDays.None;
            foreach (var day in days)
            {
                mask |= MapSystemToWeekDays(day);
            }
            return mask;
        }

        private static WeekDays MapSystemToWeekDays(DayOfWeek day)
        {
            return day switch
            {
                DayOfWeek.Monday => WeekDays.Monday,
                DayOfWeek.Tuesday => WeekDays.Tuesday,
                DayOfWeek.Wednesday => WeekDays.Wednesday,
                DayOfWeek.Thursday => WeekDays.Thursday,
                DayOfWeek.Friday => WeekDays.Friday,
                DayOfWeek.Saturday => WeekDays.Saturday,
                DayOfWeek.Sunday => WeekDays.Sunday,
                _ => WeekDays.None
            };
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return StartDate;
            yield return EndDate;
            yield return StartTime;
            yield return EndTime;
            yield return Days;
        }
    }
}
using System;
using System.Collections.Generic;

namespace Playground.Extensions
{
    public static class DateTimeExtensions
    {
        public static IEnumerable<DateTime> DaysToAndIncluding(this DateTime startDate, DateTime endDate)
        {
            while((endDate - startDate).TotalDays > 0)
            {
                yield return startDate.Subtract(startDate.TimeOfDay);
                startDate = startDate.AddDays(1);
            }
        }
    }
}

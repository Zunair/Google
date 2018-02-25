using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Google.Calendar
{
    public static partial class Enums
    {
        /// <summary>
        /// Extends DayOfWeek enum
        /// </summary>
        public enum Day
        {
            //
            // Summary:
            //     Indicates Day After Tomorrow.
            DayAfterTomorrow = -3,
            //
            // Summary:
            //     Indicates Tomorrow.
            Tomorrow = -2,
            //
            // Summary:
            //     Indicates Today.
            Today = -1,
            //
            // Summary:
            //     Indicates Sunday.
            Sunday = 0,
            //
            // Summary:
            //     Indicates Monday.
            Monday = 1,
            //
            // Summary:
            //     Indicates Tuesday.
            Tuesday = 2,
            //
            // Summary:
            //     Indicates Wednesday.
            Wednesday = 3,
            //
            // Summary:
            //     Indicates Thursday.
            Thursday = 4,
            //
            // Summary:
            //     Indicates Friday.
            Friday = 5,
            //
            // Summary:
            //     Indicates Saturday.
            Saturday = 6
        }
    }

    

    internal static class DayCalc
    {
        public static DateTime GetDateTime(Enums.Day day = Enums.Day.Today)
        {
            DateTime retVal = DateTime.Now;

            if (day == Enums.Day.Tomorrow)
            {
                retVal = retVal.AddDays(1);
                retVal = SetTime(retVal);
            }
            else if (day == Enums.Day.DayAfterTomorrow)
            {
                retVal = retVal.AddDays(2);
                retVal = SetTime(retVal);
            }
            else if (day != Enums.Day.Today)
            {
                // -1, -2, -3 are handeled already so we can use enum Day as enum DayOfTheWeek now
                retVal = GetNextWeekday((DayOfWeek)Enum.Parse(typeof(DayOfWeek), day.ToString()));
            }
            
            return retVal;
        }

        /// <summary>
        /// Get next specified day with time
        /// </summary>
        /// <param name="day">Next day to get</param>
        /// <returns>Day and time as 00:00:00</returns>
        public static DateTime GetNextWeekday(DayOfWeek day, bool fromToday = false)
        {
            DateTime retVal;
            DateTime now = fromToday ? DateTime.Now : DateTime.Now.AddDays(1);
            
            // The (... + 7) % 7 ensures we end up with a value in the range [0, 6]
            int daysToAdd = ((int)day - (int)now.DayOfWeek + 7) % 7;

            retVal = now.AddDays(daysToAdd);
            retVal = SetTime(retVal);

            return retVal;
        }


        public enum TimeOfDay
        {
            EndOfDay,
            BeginingOfDay
        }
        public static DateTime SetTime(DateTime dateTime, TimeOfDay timeOfDay = TimeOfDay.BeginingOfDay)
        {
            return timeOfDay == TimeOfDay.BeginingOfDay ?
                new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 0, 0, 0) :
                new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 23, 59, 59);
        }
    }

}

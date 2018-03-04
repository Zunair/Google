using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Google
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
                retVal = GetTime(retVal);
            }
            else if (day == Enums.Day.DayAfterTomorrow)
            {
                retVal = retVal.AddDays(2);
                retVal = GetTime(retVal);
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
            retVal = GetTime(retVal);

            return retVal;
        }


        public enum TimeOfDay
        {
            BeginingOfDay,
            EndOfDay,

            BeginingOfWeek,
            EndOfWeek,

            BeginingOfMonth,
            EndOfMonth,

            BeginingOfYear,
            EndOfYear
        }

        /// <summary>
        /// Calculates time based on given parameters
        ///  Example 1: If given dateTime is Now() and timeOfDay is EndOfDay then it will return Today's Date with 23hr 59min 59sec
        ///  Example 2: If given dateTime is Tomorow and timeOfDay is BeginingOfDay then it will return Tomorrows's Date with 0hr 0min 0sec
        ///  Example 3: If given dateTime is Now() and timeOfDay is EndOfWeek then it will return last day of current weeks's Date with 23hr 59min 59sec
        /// </summary>
        /// <param name="dateTime">Begining date time used to calcuate days, weeks, months, years</param>
        /// <param name="period">Ending period</param>
        /// <returns>Calculated end date for period</returns>
        public static DateTime GetTime(DateTime dateTime, TimeOfDay period = TimeOfDay.BeginingOfDay, DayOfWeek firstday = DayOfWeek.Monday)
        {
            DateTime retVal = new DateTime();
            
            switch (period)
            {
                case TimeOfDay.BeginingOfYear:
                    retVal = new DateTime(dateTime.Year, 1, 1, 0, 0, 0);
                    break;

                case TimeOfDay.EndOfYear:
                    retVal = new DateTime(dateTime.Year, 12, 31, 23, 59, 59);
                    break;

                case TimeOfDay.BeginingOfMonth:
                    retVal = new DateTime(dateTime.Year, 1, 1, 0, 0, 0);
                    break;

                case TimeOfDay.EndOfMonth:
                    retVal = new DateTime(dateTime.Year, 12, DateTime.DaysInMonth(dateTime.Year, dateTime.Month), 23, 59, 59);
                    break;

                case TimeOfDay.BeginingOfWeek:
                    retVal = dateTime.AddDays((-1 * (Int32)dateTime.DayOfWeek) + (GetDayOffset(firstday)));
                    retVal = GetTime(retVal, TimeOfDay.BeginingOfDay);
                    break;

                case TimeOfDay.EndOfWeek:
                    retVal = dateTime.AddDays((-1 * (Int32)dateTime.DayOfWeek) + (6 + (GetDayOffset(firstday))));
                    retVal = GetTime(retVal, TimeOfDay.EndOfDay);
                    break;

                case TimeOfDay.EndOfDay:
                    retVal = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 23, 59, 59);
                    break;

                case TimeOfDay.BeginingOfDay:
                    retVal = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 0, 0, 0);
                    break;
            }

            return retVal;
        }

        /// <summary>
        /// Gets a number to offset the day to change first day of the week
        /// </summary>
        /// <param name="firstDay">First day of the week</param>
        /// <returns></returns>
        private static int GetDayOffset(DayOfWeek firstDay)
        {
            int retVal = 0;

            if (firstDay != DayOfWeek.Sunday && DateTime.Now.DayOfWeek == DayOfWeek.Sunday)
                retVal = 6;
            else
                retVal = DateTime.Now.DayOfWeek - firstDay;

            retVal = GetFirstDayOfWeek - retVal;

            return retVal;
        }

        /// <summary>
        /// Returns first day of the week based on current system settings
        /// </summary>
        private static Int32 GetFirstDayOfWeek
        {
            get
            {
                return ((Int32)DateTime.Now.AddDays(-1 * (Int32)DateTime.Now.DayOfWeek).DayOfWeek + 6);
            }
        }
    }

}

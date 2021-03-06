﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Google
{
    public static class PeriodCalc
    {
        /// <summary>
        /// Gets datetime based on specified day enum
        /// </summary>
        /// <param name="period">Extended Day Enum</param>
        /// <returns></returns>
        public static DateTime GetDateTime(Enums.Period period, DayOfWeek startOfWeekDay, DateTime? beginingDateTime = null)
        {
            DateTime retVal = DateTime.Now;
            if (beginingDateTime != null) retVal = (DateTime)beginingDateTime;

            if (period == Enums.Period.Tomorrow)
            {
                retVal = retVal.AddDays(1);
                retVal = GetTime(retVal, firstday: startOfWeekDay);
            }
            else if (period == Enums.Period.DayAfterTomorrow)
            {
                retVal = retVal.AddDays(2);
                retVal = GetTime(retVal, firstday: startOfWeekDay);
            }
            else if (period == Enums.Period.NextWeek)
            {
                retVal = retVal.AddDays(7);
                retVal = GetTime(retVal, firstday: startOfWeekDay);
            }
            else if (period == Enums.Period.NextMonth)
            {
                retVal = retVal.AddMonths(1);
                retVal = GetTime(retVal, firstday: startOfWeekDay);
            }
            else if (period == Enums.Period.NextYear)
            {
                retVal = retVal.AddYears(1);
                retVal = GetTime(retVal, firstday: startOfWeekDay);
            }
            else if (period != Enums.Period.Today)
            {
                // -5 to -1 are handled already so we can use enum Day as enum DayOfTheWeek now
                retVal = GetNextWeekday((DayOfWeek)Enum.Parse(typeof(DayOfWeek), period.ToString()), beginingDateTime: retVal);
            }

            return retVal;
        }

        /// <summary>
        /// Get next specified day with time
        /// </summary>
        /// <param name="day">Next day to get</param>
        /// <returns>Day and time as 00:00:00</returns>
        public static DateTime GetNextWeekday(DayOfWeek day, bool fromToday = false, DateTime? beginingDateTime = null)
        {

            DateTime retVal;
            DateTime now = DateTime.Now;
            if (beginingDateTime != null) now = (DateTime)beginingDateTime; // beginingDateTime is used for unit testing

            now = fromToday ? now : now.AddDays(1);

            // The (... + 7) % 7 ensures we end up with a value in the range [0, 6]
            int daysToAdd = ((int)day - (int)now.DayOfWeek + 7) % 7;

            retVal = now.AddDays(daysToAdd);
            retVal = GetTime(retVal);

            return retVal;
        }

        /// <summary>
        /// Options to specify time of the period
        /// </summary>
        public enum TimeOfPeriod
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
        ///  Example 4: If given dateTime is Now() and timeOfDay is EndOfMonth then it will return last day of current month's Date with 23hr 59min 59sec
        /// </summary>
        /// <param name="dateTime">Begining date time used to calcuate days, weeks, months, years</param>
        /// <param name="period">Ending period</param>
        /// <returns>Calculated end date for period</returns>
        public static DateTime GetTime(DateTime dateTime, TimeOfPeriod period = TimeOfPeriod.BeginingOfDay, DayOfWeek firstday = DayOfWeek.Monday, DateTime? beginingDateTime = null)
        {
            DateTime retVal = new DateTime();
            DateTime now = DateTime.Now;
            if (beginingDateTime != null) now = (DateTime)beginingDateTime;

            switch (period)
            {
                case TimeOfPeriod.BeginingOfYear:
                    retVal = new DateTime(dateTime.Year, 1, 1, 0, 0, 0);
                    break;

                case TimeOfPeriod.EndOfYear:
                    retVal = new DateTime(dateTime.Year, 12, 31, 23, 59, 59);
                    break;

                case TimeOfPeriod.BeginingOfMonth:
                    retVal = new DateTime(dateTime.Year, 1, 1, 0, 0, 0);
                    break;

                case TimeOfPeriod.EndOfMonth:
                    retVal = new DateTime(dateTime.Year, 12, DateTime.DaysInMonth(dateTime.Year, dateTime.Month), 23, 59, 59);
                    break;

                case TimeOfPeriod.BeginingOfWeek:
                    retVal = dateTime.AddDays((-1 * (Int32)dateTime.DayOfWeek) + (GetDayOffset(firstday)));
                    retVal = GetTime(retVal, TimeOfPeriod.BeginingOfDay);
                    break;

                case TimeOfPeriod.EndOfWeek:
                    retVal = dateTime.AddDays((-1 * (Int32)dateTime.DayOfWeek) + (6 + (GetDayOffset(firstday))));
                    retVal = GetTime(retVal, TimeOfPeriod.EndOfDay);
                    break;

                case TimeOfPeriod.EndOfDay:
                    retVal = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 23, 59, 59);
                    break;

                case TimeOfPeriod.BeginingOfDay:
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
        public static int GetDayOffset(DayOfWeek firstDay, DateTime? beginingDateTime = null)
        {
            int retVal = 0;
            if (beginingDateTime == null) beginingDateTime = DateTime.Now;

            DateTime now = (DateTime)beginingDateTime;

            if (firstDay != DayOfWeek.Sunday && now.DayOfWeek == DayOfWeek.Sunday)
                retVal = 6;
            else
                retVal = now.DayOfWeek - firstDay;

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

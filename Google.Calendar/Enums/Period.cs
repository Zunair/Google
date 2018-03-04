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
        public enum Period
        {
            //
            // Summary:
            //     Indicates Next Year.
            NextYear = -6,
            //
            // Summary:
            //     Indicates Next Month.
            NextMonth = -5,
            //
            // Summary:
            //     Indicates Next Week.
            NextWeek = -4,
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
    
}

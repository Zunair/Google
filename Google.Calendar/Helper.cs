using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Google
{
    public class Helper
    {
        internal static string AddOrdinal(int num)
        {
            if (num <= 0) return num.ToString();

            switch (num % 100)
            {
                case 11:
                case 12:
                case 13:
                    return num + "th";
            }

            switch (num % 10)
            {
                case 1:
                    return num + "st";
                case 2:
                    return num + "nd";
                case 3:
                    return num + "rd";
                default:
                    return num + "th";
            }

        }

        internal static void NullToEmpty(ref string strToFix)
        {
            if (String.IsNullOrEmpty(strToFix)) strToFix = string.Empty;
        }

        internal static string FormatTime(DateTime dt)
        {
            CultureInfo ci = CultureInfo.InvariantCulture;
            return dt.ToString(ci.DateTimeFormat.ShortTimePattern);
        }

        internal static void Break()
        {
            if (System.Diagnostics.Debugger.IsAttached) System.Diagnostics.Debugger.Break();
        }
    }
}

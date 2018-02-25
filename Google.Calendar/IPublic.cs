using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Google.Calendar
{
    /// <summary>
    /// Functions used in external Apps, for example LINKS.
    /// </summary>
    public static partial class I
    {

        /// <summary>
        /// This function is called from LINKS when exe is loaded as a plugin.
        /// </summary>
        public static void OnLoad()
        {
            Authenticate();
            service = CreateService();
        }


        // TODO: Return properly formated output for speech
        /// <summary>
        /// This function can be called from LINKS as [Google.Calendar.I.GetEvents("10","Today","day")]
        /// </summary>
        /// <param name="maxEventResults">Count of events to get</param>
        /// <param name="startDay">Begining day of first event. Full day names.</param>
        /// <param name="period">Day, Week, Month, Year</param>
        /// <returns></returns>
        public static string GetEvents(string maxEventResults, string startDay, string period = "day")
        {
            string retVal = string.Empty;

            try
            {

                Enums.Day day = (Enums.Day)Enum.Parse(typeof(Enums.Day), startDay);
                Enums.DateTimePeriod timePeriod = (Enums.DateTimePeriod)Enum.Parse(typeof(Enums.DateTimePeriod), period);

                calendarEvents = GetEvents(int.Parse(maxEventResults), day, timePeriod);

                Console.WriteLine("Upcoming events:");
                if (calendarEvents.Items != null && calendarEvents.Items.Count > 0)
                {
                    foreach (var eventItem in calendarEvents.Items)
                    {
                        string when = eventItem.Start.DateTime.ToString();
                        if (String.IsNullOrEmpty(when))
                        {
                            when = eventItem.Start.Date;
                        }
                        Console.WriteLine("{0} ({1})", eventItem.Summary, when);
                    }
                }
                else
                {
                    Console.WriteLine("No upcoming events found.");
                }
            }
            catch (Exception ex)
            {
                retVal = "Error in google calendar get event function. " + ex.Message;
            }
            return retVal;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Google
{
    /// <summary>
    /// Functions used in external Apps, for example LINKS.
    /// </summary>
    public static partial class Calendar
    {
        static bool isLoaded = false;

        /// <summary>
        /// This function is called on LINKS startup when exe is loaded as a plugin.
        /// </summary>
        public static void OnLoad()
        {
            Helper.Break();

            Authenticate();
            service = CreateService();
            isLoaded = true;
        }
        
        /// <summary>
        /// This function can be called from LINKS as [Google.Calendar.I.GetEvents("10","Today","day")]
        /// </summary>
        /// <param name="maxEventResults">Count of events to get</param>
        /// <param name="startDay">Begining day of first event. Full day names.</param>
        /// <param name="period">Day, Week, Month, Year</param>
        /// <returns></returns>
        public static string GetEvents(string maxEventResults, string startDay, string period = "day")
        {
            Helper.Break();

            if (!isLoaded) OnLoad();

            string retVal = string.Empty;

            // [Get_SirOrMadam], you have {0} event(s) scheduled for {1}.
            // {4} event, {5} will be held at {2}, (in {3}).

            // {0}: eventCount
            // {1}: startDay
            // {2}: eventTime
            // {3}: eventLocation
            // {4}: eventNumber (with ordinal)
            // {5}: eventDescriptionOrTitle
            // {6}: eventSummary

            // Sample
            //  Sir, you have 2 events scheduled for today.
            //  1st event, Fake Party will be held at 1pm, at Brady's House.
            //  2nd event...


            try
            {

                Enums.Day day = (Enums.Day)Enum.Parse(typeof(Enums.Day), startDay);
                Enums.DateTimePeriod timePeriod = (Enums.DateTimePeriod)Enum.Parse(typeof(Enums.DateTimePeriod), period);

                calendarEvents = GetEvents(int.Parse(maxEventResults), day, timePeriod);

                startDay = startDay.Replace("DayAfterTomorrow", "Day After Tomorrow");

                if (calendarEvents.Items != null && calendarEvents.Items.Count > 0)
                {
                    int eventCount = calendarEvents.Items.Count;
                    

                    for (int eventNumber = 0; eventNumber < eventCount; eventNumber++)
                    {
                        Google.Apis.Calendar.v3.Data.Event eventItem = calendarEvents.Items[eventNumber];

                        //eventCount                                            // {0}
                        //startDay                                              // {1}
                        string eventTime = eventItem.Start.DateTime.ToString(); // {2}
                        string eventLocation = eventItem.Location;              // {3}
                        //eventNumber                                           // {4}
                        string eventDescription = eventItem.Description;        // {5}
                        string eventSummary = eventItem.Summary;                // {6}
                        
                        if (period.ToUpper() == "DAY")
                        {
                            eventTime = Helper.FormatTime(DateTime.Parse(eventTime));
                        }

                        if (String.IsNullOrEmpty(eventTime))
                        {
                            eventTime = eventItem.Start.Date;
                        }
                        
                        string[] paramsData = new string[] { eventCount.ToString(),
                                                                startDay,
                                                                eventTime,
                                                                eventLocation,
                                                                Helper.AddOrdinal(eventNumber + 1),
                                                                eventDescription,
                                                                eventSummary};

                        if (Properties.Settings.Default.GoogleCalendarDefaultSpeech == string.Empty)
                        {
                            Properties.Settings.Default.GoogleCalendarDefaultSpeech = "[Get_SirOrMadam], you have {0} " +
                                                (int.Parse(paramsData[0]) == 1 ? "event" : "events") +
                                                " scheduled for {1}. " +
                                                "{4} event, {5} will be held at {2}, " +
                                                (paramsData[3] == string.Empty ? string.Empty : "at {3}.");
                            Properties.Settings.Default.Save();
                        }
                            retVal = string.Format(Properties.Settings.Default.GoogleCalendarDefaultSpeech, paramsData);

                        Console.WriteLine(retVal.Replace(".",".\n"));
                    }
                }
                else
                {
                    retVal = string.Format("No upcoming events found for {0}.", startDay);
                    Console.WriteLine(retVal);
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

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
        /// Gets specific event's description
        /// </summary>
        /// <param name="eventNumber">Event number that shows up when GetEvents is called</param>
        /// <returns>Event's description</returns>
        public static string GetEventDescription(string eventNumber)
        {
            Google.Apis.Calendar.v3.Data.Event eventItem = calendarEvents.Items[int.Parse(eventNumber)-1];

            return eventItem.Description;
        }

        /// <summary>
        /// This function can be called from LINKS as [Google.Calendar.I.GetEvents("10","Today","Day","Monday")]
        /// </summary>
        /// <param name="maxEventResults">Count of events to get</param>
        /// <param name="timePeriod">Begining of first event. Full day names, Tomorrow, DayAfterTomorror, NextWeek, NextMonth, NextYear</param>
        /// <param name="timeSpan">Day, Week, Month, Year</param>
        /// <param name="startOfWeek">Start of week's full day name</param>
        /// <returns>Parsed events for speech</returns>
        public static string GetEvents(string maxEventResults, string timePeriod, string timeSpan = "Day", string startOfWeek = "Monday")
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

                Enums.Period _timePeriod = Helper.StringToEnum<Enums.Period>(timePeriod); // (Enums.Period)Enum.Parse(typeof(Enums.Period), timePeriod, true);
                Enums.TimeSpan _timeSpan = Helper.StringToEnum<Enums.TimeSpan>(timePeriod); // (Enums.TimeSpan)Enum.Parse(typeof(Enums.TimeSpan), timeSpan, true);
                DayOfWeek _startOfWeek = Helper.StringToEnum<DayOfWeek>(timePeriod); // (DayOfWeek)Enum.Parse(typeof(DayOfWeek), startOfWeek, true); 
                
                calendarEvents = GetEvents(int.Parse(maxEventResults), _timePeriod, _timeSpan);

                timePeriod = timePeriod.Replace("DayAfterTomorrow", "Day After Tomorrow");
                timePeriod = timePeriod.Replace("Next", "Next ");

                if (calendarEvents.Items != null && calendarEvents.Items.Count > 0)
                {
                    int eventCount = calendarEvents.Items.Count;

                    // TODO: If period is a day get all events of that day - allow to used multiple days seperated by commas
                    // If period is a week, get daily events with day names
                    // If period is month, get filtered events based on keywords seperated by comma with month/day names
                    // If period year, get filtered events based on keywords seperated by comma year/month/day

                    for (int eventNumber = 0; eventNumber < eventCount; eventNumber++)
                    {
                        Google.Apis.Calendar.v3.Data.Event eventItem = calendarEvents.Items[eventNumber];

                        //eventCount                                            // {0}
                        //startDay                                              // {1}
                        string eventTime = DateTime.Parse(eventItem.Start.DateTime.ToString()).ToShortTimeString(); // {2}
                        string eventLocation = eventItem.Location;              // {3}
                        //eventNumber                                           // {4}
                        string eventDescription = eventItem.Description;        // {5}
                        string eventSummary = eventItem.Summary;                // {6}
                        
                        eventTime = Helper.FormatTime(DateTime.Parse(eventTime));
                    
                        if (String.IsNullOrEmpty(eventTime))
                        {
                            eventTime = eventItem.Start.Date;
                        }
                        
                        string[] paramsData = new string[] { eventCount.ToString(),
                                                                timePeriod,
                                                                eventTime,
                                                                eventLocation,
                                                                Helper.AddOrdinal(eventNumber + 1),
                                                                eventDescription,
                                                                eventSummary};

                        //if (Properties.Settings.Default.GoogleCalendarDefaultSpeech == string.Empty)
                        //{
                            Properties.Settings.Default.GoogleCalendarDefaultSpeech = "[Get_SirOrMadam], you have {0} " +
                                                (int.Parse(paramsData[0]) == 1 ? "event" : "events") + " scheduled for {1}. ";
                        Properties.Settings.Default.GoogleCalendarDefaultSpeechEvent =
                                            "{4} event, {6} will be held at {2}. " +
                                            (string.IsNullOrEmpty(paramsData[3]) ? string.Empty : "at {3}.");
                                                //"Summary: {6}." +
                                                //" Description: {5}.";
                            //Properties.Settings.Default.Save();
                        //}

                        retVal += string.IsNullOrEmpty(retVal) ? Properties.Settings.Default.GoogleCalendarDefaultSpeech + Properties.Settings.Default.GoogleCalendarDefaultSpeechEvent : Properties.Settings.Default.GoogleCalendarDefaultSpeechEvent;
                        retVal = string.Format(retVal, paramsData);

                    }
                    Console.WriteLine(retVal.Replace(".",".\n"));
                }
                else
                {
                    retVal = string.Format("No upcoming events found for {0}.", timePeriod);
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

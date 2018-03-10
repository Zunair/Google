using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Json;

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

            if (Properties.Settings.Default.GoogleCalendarDefaultSpeechReset)
            {
                Properties.Settings.Default.GoogleCalendarDefaultSpeech1 = "[Get_SirOrMadam], you have {0} ";
                Properties.Settings.Default.GoogleCalendarDefaultSpeech2 = "event";
                Properties.Settings.Default.GoogleCalendarDefaultSpeech3 = "events";
                Properties.Settings.Default.GoogleCalendarDefaultSpeech4 = " scheduled for {1}. ";
                Properties.Settings.Default.GoogleCalendarDefaultSpeech5 = " {4} event, {6} will be held at {2}. ";
                Properties.Settings.Default.GoogleCalendarDefaultSpeech6 = "at {3}.";
                Properties.Settings.Default.GoogleCalendarDefaultSpeech7 = "Day After Tommorow";
                Properties.Settings.Default.GoogleCalendarDefaultSpeech8 = "Tomorrow";
                Properties.Settings.Default.GoogleCalendarDefaultSpeech9 = "Today";
                Properties.Settings.Default.GoogleCalendarDefaultSpeech10 = "Sunday";
                Properties.Settings.Default.GoogleCalendarDefaultSpeech11 = "Monday";
                Properties.Settings.Default.GoogleCalendarDefaultSpeech12 = "Tuesday";
                Properties.Settings.Default.GoogleCalendarDefaultSpeech13 = "Wednesday";
                Properties.Settings.Default.GoogleCalendarDefaultSpeech14 = "Thursday";
                Properties.Settings.Default.GoogleCalendarDefaultSpeech15 = "Friday";
                Properties.Settings.Default.GoogleCalendarDefaultSpeech16 = "Saturday";
                Properties.Settings.Default.GoogleCalendarDefaultSpeech17 = "the Week";
                Properties.Settings.Default.GoogleCalendarDefaultSpeech18 = "Month";
                Properties.Settings.Default.GoogleCalendarDefaultSpeech19 = "Year";
                Properties.Settings.Default.GoogleCalendarDefaultSpeech20 = "Next {0}";
                Properties.Settings.Default.GoogleCalendarDefaultSpeech21 = "No upcoming events found for {0}.";
                Properties.Settings.Default.GoogleCalendarDefaultSpeech22 = "Error in google calendar get event function. ";
                Properties.Settings.Default.GoogleCalendarDefaultSpeechReset = false;
                Properties.Settings.Default.Save();
            }
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
                Enums.Period _timePeriod = Helper.StringToEnum<Enums.Period>(timePeriod);
                Enums.TimeSpan _timeSpan = Helper.StringToEnum<Enums.TimeSpan>(timeSpan);
                DayOfWeek _startOfWeek = Helper.StringToEnum<DayOfWeek>(startOfWeek);
                
                calendarEvents = GetEvents(int.Parse(maxEventResults), _timePeriod, _timeSpan, _startOfWeek);

                timePeriod = timePeriod.Replace("DayAfterTomorrow", Properties.Settings.Default.GoogleCalendarDefaultSpeech7);
                timePeriod = timePeriod.Replace("Tomorrow", Properties.Settings.Default.GoogleCalendarDefaultSpeech8);
                timePeriod = timePeriod.Replace("Today", Properties.Settings.Default.GoogleCalendarDefaultSpeech9);
                timePeriod = timePeriod.Replace("Sunday", Properties.Settings.Default.GoogleCalendarDefaultSpeech10);
                timePeriod = timePeriod.Replace("Monday", Properties.Settings.Default.GoogleCalendarDefaultSpeech11);
                timePeriod = timePeriod.Replace("Tuesday", Properties.Settings.Default.GoogleCalendarDefaultSpeech12);
                timePeriod = timePeriod.Replace("Wednesday", Properties.Settings.Default.GoogleCalendarDefaultSpeech13);
                timePeriod = timePeriod.Replace("Thursday", Properties.Settings.Default.GoogleCalendarDefaultSpeech14);
                timePeriod = timePeriod.Replace("Friday", Properties.Settings.Default.GoogleCalendarDefaultSpeech15);
                timePeriod = timePeriod.Replace("Saturday", Properties.Settings.Default.GoogleCalendarDefaultSpeech16);
                timePeriod = timePeriod.Replace("Week", Properties.Settings.Default.GoogleCalendarDefaultSpeech17);
                timePeriod = timePeriod.Replace("Month", Properties.Settings.Default.GoogleCalendarDefaultSpeech18);
                timePeriod = timePeriod.Replace("Year", Properties.Settings.Default.GoogleCalendarDefaultSpeech19);
                timePeriod = timePeriod.Replace("Next", string.Format(Properties.Settings.Default.GoogleCalendarDefaultSpeech20, timePeriod.Replace("Next", "")));

                if (calendarEvents.Items != null && calendarEvents.Items.Count > 0)
                {
                    int eventCount = calendarEvents.Items.Count;

                    // TODO: If period is a day get all events of that day - allow to use multiple days seperated by commas
                    // If period is a week, get daily events with day names
                    // If period is month, get filtered events based on keywords seperated by comma with month/day names
                    // If period year, get filtered events based on keywords seperated by comma year/month/day

                    string speech = Properties.Settings.Default.GoogleCalendarDefaultSpeech1 +
                                            (eventCount == 1 ? Properties.Settings.Default.GoogleCalendarDefaultSpeech2 : Properties.Settings.Default.GoogleCalendarDefaultSpeech3) + Properties.Settings.Default.GoogleCalendarDefaultSpeech4;


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
                        
                        string eSpeech  = Properties.Settings.Default.GoogleCalendarDefaultSpeech5 +
                                            (string.IsNullOrEmpty(paramsData[3]) ? string.Empty : Properties.Settings.Default.GoogleCalendarDefaultSpeech6);
                                                //"Summary: {6}." +
                                                //" Description: {5}.";
                            //Properties.Settings.Default.Save();
                        //}

                        retVal += string.IsNullOrEmpty(retVal) ? speech + eSpeech : eSpeech;
                        retVal = string.Format(retVal, paramsData);

                    }
                    Console.WriteLine(retVal.Replace(".",".\n"));
                }
                else
                {
                    retVal = string.Format(Properties.Settings.Default.GoogleCalendarDefaultSpeech21, timePeriod);
                    Console.WriteLine(retVal);
                }
            }
            catch (Exception ex)
            {
                retVal = Properties.Settings.Default.GoogleCalendarDefaultSpeech22 + ex.Message;
            }
            return retVal;
        }
        
    }
}

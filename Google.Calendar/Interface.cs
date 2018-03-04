using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static Google.PeriodCalc;

namespace Google
{
    /// Make sure to create a folder called Protected and put your client_secret.json in there.
 
    public static partial class Calendar
    {
        static string[] Scopes = { CalendarService.Scope.CalendarReadonly };
        static string ApplicationName = "LINKS - Google Calendar";

        static CalendarService service;
        static UserCredential credential;
        static Events calendarEvents;

        /// <summary>
        /// Authenticate User
        /// </summary>
        private static void Authenticate()
        {
            // Loads Protected\client_secret.json file
            //  this file is not on Git, either get your own or get it from your admin dev.
            using (var stream = new MemoryStream(Properties.Resources.client_secret))
            {
                string credPath = System.Environment.GetFolderPath(
                    System.Environment.SpecialFolder.UserProfile);
                credPath = Path.Combine(credPath, ".credentials", "links-google-calendar.json");

                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
                //Console.WriteLine("Credential file saved to: " + credPath);
            }
        }

        /// <summary>
        /// Create Google Calendar Service
        /// </summary>
        /// <returns>Google Calendar Service</returns>
        private static CalendarService CreateService()
        {
            // Create Google Calendar API service.
            return new CalendarService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="maxEventResults"></param>
        /// <param name="period"></param>
        /// <param name="timeSpan"></param>
        /// <param name="startOfWeekDay"></param>
        /// <returns></returns>
        private static Events GetEvents(int maxEventResults, Enums.Period period, 
                                        Enums.TimeSpan timeSpan = Enums.TimeSpan.Day, 
                                        DayOfWeek startOfWeekDay = DayOfWeek.Monday)
        {
            Events retVal = null;
            
            // Define parameters of request.
            //var cl = service.CalendarList.List().Execute().Items;
            //EventsResource.ListRequest request = service.Events.List("en.sa#holiday@group.v.calendar.google.com");
            EventsResource.ListRequest request = service.Events.List("primary");

            TimeOfPeriod timeOfDay = TimeOfPeriod.EndOfDay;
            switch (timeSpan)
            {
                case Enums.TimeSpan.Week:
                    timeOfDay = TimeOfPeriod.EndOfWeek;
                    break;

                case Enums.TimeSpan.Month:
                    timeOfDay = TimeOfPeriod.EndOfMonth;
                    break;

                case Enums.TimeSpan.Year:
                    timeOfDay = TimeOfPeriod.EndOfYear;
                    break;
            }

            DateTime dt = PeriodCalc.GetDateTime(period, startOfWeekDay);
            request.TimeMin = dt;
            request.TimeMax = PeriodCalc.GetTime(dt, timeOfDay, startOfWeekDay);
            request.ShowDeleted = false;
            request.SingleEvents = true;
            request.MaxResults = maxEventResults;
            request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;


            // Get events.
            retVal = request.Execute();

            
            return retVal;
        }

    }
}

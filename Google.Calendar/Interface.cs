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
using static Google.DayCalc;

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

        static void Authenticate()
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

        static CalendarService CreateService()
        {
            // Create Google Calendar API service.
            return new CalendarService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });
        }
        
        private static Events GetEvents(int maxEventResults, Enums.Day day, Enums.DateTimePeriod period = Enums.DateTimePeriod.Day, DayOfWeek startOfWeekDay = DayOfWeek.Monday)
        {
            Events retVal = null;
            
            // Define parameters of request.
            //var cl = service.CalendarList.List().Execute().Items;
            //EventsResource.ListRequest request = service.Events.List("en.sa#holiday@group.v.calendar.google.com");
            EventsResource.ListRequest request = service.Events.List("primary");

            TimeOfDay timeOfDay = TimeOfDay.EndOfDay;
            switch (period)
            {
                case Enums.DateTimePeriod.Week:
                    timeOfDay = TimeOfDay.EndOfWeek;
                    break;

                case Enums.DateTimePeriod.Month:
                    timeOfDay = TimeOfDay.EndOfMonth;
                    break;

                case Enums.DateTimePeriod.Year:
                    timeOfDay = TimeOfDay.EndOfYear;
                    break;
            }

            DateTime dt = DayCalc.GetDateTime(day);
            request.TimeMin = dt;
            request.TimeMax = DayCalc.GetTime(dt, timeOfDay, startOfWeekDay); // TODO: Calculate time period
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

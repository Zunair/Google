﻿using Google.Apis.Auth.OAuth2;
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

namespace Google.Calendar
{
    public static partial class I
    {
        static string[] Scopes = { CalendarService.Scope.CalendarReadonly };
        static string ApplicationName = "LINKS - Google Calendar";

        static CalendarService service;
        static UserCredential credential;
        static Events calendarEvents;

        static void Authenticate()
        {
            using (var stream =
                new FileStream(Path.Combine("Protected", "client_secret.json"), FileMode.Open, FileAccess.Read))
            {
                string credPath = System.Environment.GetFolderPath(
                    System.Environment.SpecialFolder.UserProfile);
                credPath = Path.Combine(credPath, ".credentials/links-google-calendar.json");

                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
                Console.WriteLine("Credential file saved to: " + credPath);
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
        
        static Events GetEvents(int maxEventResults, Enums.Day day, Enums.DateTimePeriod period)
        {
            Events retVal = null;
            
            // Define parameters of request.
            //var cl = service.CalendarList.List().Execute().Items;
            //EventsResource.ListRequest request = service.Events.List("en.sa#holiday@group.v.calendar.google.com");
            EventsResource.ListRequest request = service.Events.List("primary");


            DateTime dt = DayCalc.GetDateTime(day);
            request.TimeMin = dt;
            request.TimeMax = DayCalc.SetTime(dt, DayCalc.TimeOfDay.EndOfDay); // TODO: Calculate time period
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
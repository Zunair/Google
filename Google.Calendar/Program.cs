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

namespace Google.Calendar
{

    class Program
    {
        // If modifying these scopes, delete your previously saved credentials
        // at ~/.credentials/calendar-dotnet-quickstart.json
        static string[] Scopes = { CalendarService.Scope.CalendarReadonly };
        static string ApplicationName = "LINKS - Google Calendar";


        static void Test()
        {
            Console.WriteLine();
            Console.WriteLine(DayCalc.GetDateTime(Enums.Day.Today));
            Console.WriteLine(DayCalc.GetDateTime(Enums.Day.Tomorrow));
            Console.WriteLine(DayCalc.GetDateTime(Enums.Day.DayAfterTomorrow));
            Console.WriteLine(DayCalc.GetDateTime(Enums.Day.Sunday));
            Console.WriteLine(DayCalc.GetDateTime(Enums.Day.Monday));
            Console.WriteLine(DayCalc.GetDateTime(Enums.Day.Tuesday));
            Console.WriteLine(DayCalc.GetDateTime(Enums.Day.Wednesday));
            Console.WriteLine(DayCalc.GetDateTime(Enums.Day.Thursday));
            Console.WriteLine(DayCalc.GetDateTime(Enums.Day.Friday));
            Console.WriteLine(DayCalc.GetDateTime(Enums.Day.Saturday));
            Console.WriteLine();

            I.OnLoad();
            I.GetEvents("10", "Today");
        }

        static void Main(string[] args)
        {
            Test();
            
            Console.Read();

        }
    }
}
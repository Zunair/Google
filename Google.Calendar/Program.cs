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

namespace Google
{

    class Program
    {
        static void Test()
        {
            string appName = "Google Calendar Plugin For LINKS";

            Console.Title = appName;
            Console.WriteLine(appName + " v" + typeof(Program).Assembly.GetName().Version);
            Console.WriteLine("\n\n");
            Console.WriteLine("Call from LINKS:");
            Console.WriteLine("  [Google.Calendar.I.GetEvents(\"MaxEventResultCount\",\"Today,Tomorrow,DayAfterTommorow,Sunday,Monday...\",\"Day,Week,Month,Year (not used atm, value Day is default)\")]");
            Console.WriteLine();
            Console.WriteLine("Sample:");
            Console.WriteLine("  [Google.Calendar.GetEvents(\"10\",\"Today\",\"Day\")]");
            Console.WriteLine("\n\n");
            Console.WriteLine("Testing Sample...");

            Calendar.OnLoad();
            Calendar.GetEvents("10", "Today", "Week");
        }

        static void Main(string[] args)
        {
            Test();

            Console.WriteLine("\nPress any key to exit.");
            Console.Read();

        }
    }
}
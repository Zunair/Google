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
        static void Test()
        {
            I.OnLoad();
            I.GetEvents("10", "Today", "Week");
        }

        static void Main(string[] args)
        {
            Test();
            
            Console.Read();

        }
    }
}
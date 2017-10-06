using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace HotelSearchingListingBookingEngine.Core
{
    public class Logger
    {
        private static string _logFile = Path.GetFullPath("logs.txt");

        public static void StoreLog(string log)
        {
            File.AppendAllText(_logFile, _getLogTime() + " " + log);
        }

        private static string _getLogTime()
        {
            return DateTime.UtcNow.AddHours(5.5).ToString();
        }
    }
}

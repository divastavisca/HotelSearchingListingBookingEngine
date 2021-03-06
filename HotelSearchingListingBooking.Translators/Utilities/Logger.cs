﻿using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace HotelSearchingListingBooking.Translators.Utilities
{
    public class Logger
    {
        private static string _logFile = Path.GetFullPath("logs.txt");

        public static void StoreLog(string log)
        {
            try
            {
                File.AppendAllText(_logFile, getLogText(log, false, null));
            }
            catch (Exception excep)
            {

            }
        }

        public static void LogException(string log,string stackTrace)
        {
            try
            {
                File.AppendAllText(_logFile, getLogText(log, true, stackTrace));
            }
            catch(Exception excep)
            {

            }
        }

        private static string getLogText(string log,bool isException,string stackTrace)
        {
            StringBuilder logText = new StringBuilder();
            logText.Append("-------------------------");
            logText.Append("Log ID:");
            logText.Append(Guid.NewGuid().ToString());
            logText.AppendLine();
            logText.Append("Timestamp: ");
            logText.Append(_getLogTime());
            logText.AppendLine();
            logText.Append("Log Description: ");
            logText.Append(log);
            if(isException)
            {
                logText.AppendLine();
                logText.Append("Stack Trace: ");
                logText.Append(stackTrace);
            }
            logText.AppendLine();
            logText.Append("-------------------------");
            return logText.ToString();
        }

        private static string _getLogTime()
        {
            return DateTime.UtcNow.AddHours(5.5).ToString();
        }
    }
}

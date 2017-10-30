using System;
using System.Collections.Generic;
using System.Text;
using SystemContracts.CustomExceptions;

namespace HotelSearchingListingBooking.Caches
{
    public class CacheManager
    {
        private static Dictionary<DateTime, string> _sessionLogs;
        private static List<Type> _optimizableCaches;
        private static int _maxCount = 2;
        private static int _timeOutMinutes = 2;

        static CacheManager()
        {
            _sessionLogs = new Dictionary<DateTime, string>();
            _optimizableCaches = new List<Type>()
            {
                typeof(ItineraryCache),
                typeof(SearchCriterionCache),
                typeof(SelectedItineraryCache),
                typeof(SelectedItineraryRoomsCache),
                typeof(PricingRequestCache),
                typeof(TripProductCache)
            };
        }

        public static void RegisterSession(string sessionId)
        {
            try
            {
                if(_sessionLogs.Count >= _maxCount)
                {
                    optimizeAllCaches();
                }
                _sessionLogs.Add(DateTime.Now, sessionId);
            }
            catch(Exception baseException)
            {
                throw new CacheManagerException()
                {
                    Source = "registering into manager"
                };
            }
        }

        public static void UpdateSession(string callerSessionId)
        {
            try
            {
                DateTime key = DateTime.MaxValue;
                foreach (KeyValuePair<DateTime, string> log in _sessionLogs)
                {
                    if (log.Value == callerSessionId)
                    {
                        key = log.Key;
                        break;
                    }
                }
                if (key != DateTime.MaxValue)
                {
                    _sessionLogs.Remove(key);
                    RegisterSession(callerSessionId);
                }
            }
            catch(Exception baseException)
            {
                throw new CacheManagerException()
                {
                    Source = "Updating session"
                };
            }
        }

        private static void optimizeAllCaches()
        {
            try
            {
                List<DateTime> _removedLogs = new List<DateTime>();
                foreach (KeyValuePair<DateTime, string> _uniqueSession in _sessionLogs)
                {
                    if (_removedLogs.Count!=0 && _removedLogs.Count == _sessionLogs.Count - _maxCount)
                        break;
                    if (isTimedOut(_uniqueSession.Key))
                    {
                        foreach (Type optimizableCache in _optimizableCaches)
                        {
                            optimizableCache.GetMethod("Remove").Invoke(null, new object[] { _uniqueSession.Value });
                        }
                        _removedLogs.Add(_uniqueSession.Key);
                    }
                }
                foreach(DateTime logKey in _removedLogs)
                {
                    _sessionLogs.Remove(logKey);
                }
            }
            catch(Exception baseException)
            {
                throw new CacheManagerException()
                {
                    Source = "optimizing caches"
                };
            }
        }

        private static bool isTimedOut(DateTime timeInstance)
        {
            if (DateTime.Now.Hour == timeInstance.Hour)
                return DateTime.Now.Minute - timeInstance.Minute >= _timeOutMinutes;
            else return true;
        }
    }
}

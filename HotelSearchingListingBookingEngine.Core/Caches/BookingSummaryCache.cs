using System;
using System.Collections.Generic;
using System.Text;
using SystemContracts.InternalContracts;

namespace HotelSearchingListingBookingEngine.Core.Caches
{
    public class BookingSummaryCache
    {
        private static Dictionary<string, BookingSummary> _cache;

        static BookingSummaryCache()
        {
            _cache = new Dictionary<string, BookingSummary>();
        }

        public static bool IsPresent(string sessionId)
        {
            return _cache.ContainsKey(sessionId);
        }

        public static void AddToCache(string sessionId, BookingSummary searchResponse)
        {
            _cache.Add(sessionId, searchResponse);
        }

        public static void Remove(string sessionId)
        {
            if (IsPresent(sessionId))
                _cache.Remove(sessionId);
        }

        public static BookingSummary GetSummary(string sessionId)
        {
            return IsPresent(sessionId) ? _cache[sessionId] : null;
        }
    }
}

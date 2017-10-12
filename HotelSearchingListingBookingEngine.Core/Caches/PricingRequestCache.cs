using System;
using System.Collections.Generic;
using System.Text;

namespace HotelSearchingListingBookingEngine.Core.Caches
{
    public class PricingRequestCache
    {
        private static Dictionary<string, string> _cache;

        static PricingRequestCache()
        {
            _cache = new Dictionary<string, string>();
        }

        public static bool IsPresent(string sessionId)
        {
            return _cache.ContainsKey(sessionId);
        }

        public static void AddToCache(string sessionId, string requestedRoomId)
        {
            _cache.Add(sessionId, requestedRoomId);
        }

        public static void Remove(string sessionId)
        {
            if (IsPresent(sessionId))
                _cache.Remove(sessionId);
        }

        public static string GetItineraries(string sessionId)
        {
            return IsPresent(sessionId) ? _cache[sessionId] : null;
        }
    }
}

using System;
using System.Collections.Generic;
using ExternalServices.PricingPolicyEngine;
using System.Text;

namespace HotelSearchingListingBookingEngine.Core.Caches
{
    public class TripProductCache
    {
        private static Dictionary<string, TripProduct> _cache;

        static TripProductCache()
        {
            _cache = new Dictionary<string, TripProduct>();
        }

        public static bool IsPresent(string sessionId)
        {
            return _cache.ContainsKey(sessionId);
        }

        public static void AddToCache(string sessionId, TripProduct searchResponse)
        {
            _cache.Add(sessionId, searchResponse);
        }

        public static void Remove(string sessionId)
        {
            if (IsPresent(sessionId))
                _cache.Remove(sessionId);
        }

        public static TripProduct GetItineraries(string sessionId)
        {
            return IsPresent(sessionId) ? _cache[sessionId] : null;
        }
    }
}

using System;
using System.Collections.Generic;
using HotelSearchingListingBooking.ExternalServices.PricingPolicyEngine;
using System.Text;

namespace HotelSearchingListingBooking.Caches
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

        public static void AddToCache(string sessionId, TripProduct tripProduct)
        {
            _cache.Add(sessionId, tripProduct);
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

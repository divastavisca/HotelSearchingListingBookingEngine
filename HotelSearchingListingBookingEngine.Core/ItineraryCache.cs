using System;
using System.Collections.Generic;
using System.Text;
using ExternalServices.HotelSearchEngineConnecter;

namespace HotelSearchingListingBookingEngine.Core
{
    public class ItineraryCache
    {
        private static Dictionary<string, HotelItinerary> _cache;

        public static bool IsPresent(string sessionId)
        {
            return _cache.ContainsKey(sessionId);
        }

        public static void AddToCache(string sessionId, HotelItinerary searchResponse)
        {
            _cache.Add(sessionId, searchResponse);
        }

        public static void Remove(string sessionId)
        {
            if (IsPresent(sessionId))
                _cache.Remove(sessionId);
        }

        public static HotelItinerary GetResponse(string sessionId)
        {
            return IsPresent(sessionId) ? _cache[sessionId] : null;
        }
    }
}

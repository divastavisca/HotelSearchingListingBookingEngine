using System;
using System.Collections.Generic;
using System.Text;
using HotelSearchingListingBooking.ExternalServices.HotelSearchEngine;

namespace HotelSearchingListingBooking.Caches
{
    public class SelectedItineraryCache
    {
        private static Dictionary<string, HotelItinerary> _cache;

        static SelectedItineraryCache()
        {
            _cache = new Dictionary<string, HotelItinerary>();
        }

        public static bool IsPresent(string sessionId)
        {
            return _cache.ContainsKey(sessionId);
        }

        public static void AddToCache(string sessionId, HotelItinerary selectedItinerary)
        {
            _cache.Add(sessionId, selectedItinerary);
        }

        public static void Remove(string sessionId)
        {
            if (IsPresent(sessionId))
                _cache.Remove(sessionId);
        }

        public static HotelItinerary GetSelecetedItinerary(string sessionId)
        {
            return IsPresent(sessionId) ? _cache[sessionId] : null;
        }
    }
}

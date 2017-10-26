using System;
using System.Collections.Generic;
using System.Text;
using ExternalServices.HotelSearchEngine;

namespace HotelSearchingListingBookingEngine.Core.Caches
{
    public class SelectedItineraryRoomsCache
    {
        private static Dictionary<string, Room[]> _cache;

        static SelectedItineraryRoomsCache()
        {
            _cache = new Dictionary<string, ExternalServices.HotelSearchEngine.Room[]>();
        }

        public static bool IsPresent(string sessionId)
        {
            return _cache.ContainsKey(sessionId);
        }

        public static void AddToCache(string sessionId, ExternalServices.HotelSearchEngine.Room[] selectedItinerary)
        {
            _cache.Add(sessionId, selectedItinerary);
        }

        public static void Remove(string sessionId)
        {
            if (IsPresent(sessionId))
                _cache.Remove(sessionId);
        }

        public static Room[] GetAllRooms(string sessionId)
        {
            return IsPresent(sessionId) ? _cache[sessionId] : null;
        }
    }
}

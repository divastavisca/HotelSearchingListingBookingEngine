using System;
using System.Collections.Generic;
using System.Text;
using HotelSearchingListingBooking.ExternalServices.HotelSearchEngine;

namespace HotelSearchingListingBooking.Caches
{
    public class SelectedItineraryRoomsCache
    {
        private static Dictionary<string, Room[]> _cache;

        static SelectedItineraryRoomsCache()
        {
            _cache = new Dictionary<string, Room[]>();
        }

        public static bool IsPresent(string sessionId)
        {
            return _cache.ContainsKey(sessionId);
        }

        public static void AddToCache(string sessionId, Room[] itineraryRooms)
        {
            _cache.Add(sessionId, itineraryRooms);
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

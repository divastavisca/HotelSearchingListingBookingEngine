using System;
using System.Collections.Generic;
using System.Text;
using ExternalServices.HotelSearchEngine;

namespace HotelSearchingListingBookingEngine.Core.Caches
{
    public class SearchCriterionCache
    {
        private static Dictionary<string, HotelSearchCriterion> _cache;

        static SearchCriterionCache()
        {
            _cache = new Dictionary<string, HotelSearchCriterion>();
        }

        public static bool IsPresent(string sessionId)
        {
            return _cache.ContainsKey(sessionId);
        }

        public static void AddToCache(string sessionId, HotelSearchCriterion searchCriterion)
        {
            _cache.Add(sessionId, searchCriterion);
        }

        public static void Remove(string sessionId)
        {
            if (IsPresent(sessionId))
                _cache.Remove(sessionId);
        }

        public static HotelSearchCriterion GetSearchCriterion(string sessionId)
        {
            return IsPresent(sessionId) ? _cache[sessionId] : null;
        }
    }
}

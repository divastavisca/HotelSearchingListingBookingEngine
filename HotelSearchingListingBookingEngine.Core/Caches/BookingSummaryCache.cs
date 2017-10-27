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

        public static bool IsPresent(string tripId)
        {
            return _cache.ContainsKey(tripId);
        }

        public static void AddToCache(string tripId, BookingSummary bookingSummary)
        {
            _cache.Add(tripId, bookingSummary);
        }

        public static void Remove(string tripId)
        {
            if (IsPresent(tripId))
                _cache.Remove(tripId);
        }

        public static BookingSummary GetSummary(string tripId)
        {
            return IsPresent(tripId) ? _cache[tripId] : null;
        }
    }
}

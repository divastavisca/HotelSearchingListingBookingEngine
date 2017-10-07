using System;
using System.Collections.Generic;
using System.Text;
using SystemContracts.ConsumerContracts;
using ExternalServices.HotelSearchEngine;
using SystemContracts.Attributes.HotelAttributes;

namespace HotelSearchingListingBookingEngine.Core.Parsers
{
    public class MultiAvailHotelSearchRSParser
    {
        public MultiAvailHotelSearchRS Parse(HotelSearchRS hotelSearchRS)
        {
            MultiAvailHotelSearchRS multiAvailHotelSearchRS = new MultiAvailHotelSearchRS()
            {
                CallerSessionId = hotelSearchRS.SessionId,
                ResultsCount = hotelSearchRS.Itineraries.Length
            };
            multiAvailHotelSearchRS.Itineraries = parseItineraries(hotelSearchRS.Itineraries);
            return multiAvailHotelSearchRS;
        }

        private ItinerarySummary[] parseItineraries(HotelItinerary[] itineraries)
        {
            List<ItinerarySummary> fetchedItineraries = new List<ItinerarySummary>();
            foreach (HotelItinerary hotelItinerary in itineraries)
            {
                ItinerarySummary uniqueItinerary;
                if (tryParseItinerary(hotelItinerary,out uniqueItinerary))
                    fetchedItineraries.Add(uniqueItinerary);
            }
            return fetchedItineraries.Count > 0 ? fetchedItineraries.ToArray() : null;
        }

        private bool tryParseItinerary(HotelItinerary hotelItinerary, out ItinerarySummary uniqueItinerary)
        {
            throw new NotImplementedException();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ExternalServices.HotelSearchEngine;
using SystemContracts.ConsumerContracts;
using SystemContracts.Attributes.HotelAttributes;

namespace HotelSearchingListingBookingEngine.Core.Parsers
{
    public class SingleAvailRoomSearchRSParser
    {
        public SingleAvailRoomSearchRS Parse(HotelRoomAvailRS hotelRoomSearchRS)
        {
            var userSearchResults = ItineraryCache.GetItineraries(hotelRoomSearchRS.SessionId);
            foreach(HotelItinerary itinerary in userSearchResults)
            {
              //  if(itinerary.HotelProperty.SupplierHotelId == hote)
            }
            SingleAvailRoomSearchRS parserRS = new SingleAvailRoomSearchRS()
            {
                Itinerary = new Itinerary()
                {
                  //  ItinerarySummary = (new MultiAvailHotelSearchRSParser()).TryParseItinerary()
                }
            };
            return null;
        }
    }
}

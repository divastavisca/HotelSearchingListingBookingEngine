using System;
using System.Collections.Generic;
using System.Text;
using SystemContracts.ConsumerContracts;
using ExternalServices.HotelSearchEngine;
using SystemContracts.Attributes.HotelAttributes;
using Newtonsoft.Json;
using SystemContracts.Attributes;

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
            uniqueItinerary = new ItinerarySummary();
            try
            {
                uniqueItinerary.Name = hotelItinerary.HotelProperty.Name;
                uniqueItinerary.Address = new HotelAddress()
                {
                    AddressLine1 = hotelItinerary.HotelProperty.Address.AddressLine1,
                    AddressLine2 = hotelItinerary.HotelProperty.Address.AddressLine2,
                    City = hotelItinerary.HotelProperty.Address.City.Name,
                    State = hotelItinerary.HotelProperty.Address.City.State,
                    Country = hotelItinerary.HotelProperty.Address.City.Country,
                    ZipCode = hotelItinerary.HotelProperty.Address.ZipCode
                };
                uniqueItinerary.GeoCode = JsonConvert.DeserializeObject<GeoCoordinates>(JsonConvert.SerializeObject(hotelItinerary.HotelProperty.GeoCode));
                List<string> uniqueAmenities = new List<string>();
                foreach (Amenity hotelAmenity in hotelItinerary.HotelProperty.Amenities)
                {
                    uniqueAmenities.Add(hotelAmenity.Name);
                }
                uniqueItinerary.Amenities = uniqueAmenities.ToArray();

            }
            catch(Exception baseException)
            {

            }
            return true;
        }
    }
}

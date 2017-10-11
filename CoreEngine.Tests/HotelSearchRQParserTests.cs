using Microsoft.VisualStudio.TestTools.UnitTesting;
using HotelSearchingListingBookingEngine.Core.Parsers;
using HotelSearchingListingBookingEngine.Core;
using SystemContracts.ConsumerContracts;
using System;
using HotelSearchingListingBookingEngine.Core.ServiceEngines;
using System.Threading;
using System.Threading.Tasks;
using ExternalServices.HotelSearchEngine;
using HotelSearchingListingBookingEngine.Core.Caches;

namespace CoreEngine.Tests
{
    [TestClass]
    public class HotelSearchRQParserTests
    {
        HotelSearchRQParser parser;
        MultiAvailHotelSearchRQ request;
        MultiAvailHotelSearchEngine engine;

        public HotelSearchRQParserTests()
        {
            parser = new HotelSearchRQParser();
            request = new MultiAvailHotelSearchRQ()
            {
                AdultsCount = 2,
                CheckInDate = DateTime.Parse("2017-10-15"),
                CheckOutDate = DateTime.Parse("2017-10-16"),
                ChildrenAge = new System.Collections.Generic.List<int>() {12,12,12},
                ChildrensCount = 3,
                SearchLocation = new SystemContracts.Attributes.Destination()
                {
                    GeoCode = new SystemContracts.Attributes.GeoCoordinates()
                    {
                        Latitude = 27.173891f,
                        Longitude = 78.042068f
                    },
                    Name = "Taj Mahal",
                    Type = "GeoCode"
                }
            };
            engine = new MultiAvailHotelSearchEngine();
        }

        [TestMethod]
        public async Task Valid_Search_Request_Parsing()
        {
            var response = await engine.SearchAsync(request);
            var itinerary = ItineraryCache.GetItineraries(((MultiAvailHotelSearchRS)response).CallerSessionId);
            var hotelroomavail = new HotelRoomAvailRQ();
            hotelroomavail.HotelSearchCriterion = SearchCriterionCache.GetSearchCriterion(((MultiAvailHotelSearchRS)response).CallerSessionId);
            hotelroomavail.Itinerary = ItineraryCache.GetItineraries(((MultiAvailHotelSearchRS)response).CallerSessionId)[0];
            hotelroomavail.SessionId = ((MultiAvailHotelSearchRS)response).CallerSessionId;
            hotelroomavail.ResultRequested = ResponseType.Complete;
            HotelEngineClient client = new HotelEngineClient();
            var res = await client.HotelRoomAvailAsync(hotelroomavail);
        }
    }
}

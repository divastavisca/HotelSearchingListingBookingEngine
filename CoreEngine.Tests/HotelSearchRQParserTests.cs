using Microsoft.VisualStudio.TestTools.UnitTesting;
using HotelSearchingListingBookingEngine.Core.Parsers;
using SystemContracts.ConsumerContracts;
using System;
using HotelSearchingListingBookingEngine.Core.ServiceEngines;
using System.Threading;
using System.Threading.Tasks;

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
                AdultsCount = 1,
                CheckInDate = DateTime.Parse("10-10-2017"),
                CheckOutDate = DateTime.Parse("12-10-2017"),
                ChildrenAges = new int[2] { 12, 12 },
                ChildrensCount = 2,
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
            var response2 = await engine.SearchAsync(request);
            Thread.Sleep(10000000);
        }
    }
}

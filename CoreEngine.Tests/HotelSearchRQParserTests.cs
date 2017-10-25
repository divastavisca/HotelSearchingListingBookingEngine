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
        HotelProductBookRQ req;

        public HotelSearchRQParserTests()
        {
            parser = new HotelSearchRQParser();
            request = new MultiAvailHotelSearchRQ()
            {
                AdultsCount = 1,
                CheckInDate = DateTime.Parse("2017-11-15"),
                CheckOutDate = DateTime.Parse("2017-11-16"),
                ChildrenAge = new System.Collections.Generic.List<int>() { 12 },
                ChildrenCount = 1,
                SearchLocation = new SystemContracts.Attributes.Destination()
                {
                    GeoCode = new SystemContracts.Attributes.GeoCoordinates()
                    {
                        Latitude = 36.09965f,
                        Longitude = -115.165222f
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
            int i = 0;
                foreach (HotelItinerary iti in itinerary)
                {
                    if (iti.HotelFareSource.Name.StartsWith("TouricoTGSTest"))
                    {
                        i = 0;
                        hotelroomavail.Itinerary = iti;
                        foreach (Room ro in iti.Rooms)
                        {
                            if (ro.HotelFareSource.Name.StartsWith("TouricoTGSTest"))
                                break;
                            i++;
                        }
                    }
                }
                // hotelroomavail.Itinerary = ItineraryCache.GetItineraries(((MultiAvailHotelSearchRS)response).CallerSessionId)[192];
                hotelroomavail.SessionId = ((MultiAvailHotelSearchRS)response).CallerSessionId;
                hotelroomavail.ResultRequested = ResponseType.Complete;
                HotelEngineClient client = new HotelEngineClient();
                var res = await client.HotelRoomAvailAsync(hotelroomavail);

            SelectedItineraryCache.AddToCache(res.SessionId, res.Itinerary);
            i = 0;
            foreach (Room ro in res.Itinerary.Rooms)
            {
                if (ro.HotelFareSource.Name.StartsWith("TouricoTGSTest"))
                    break;
                i++;
            }
            RoomPricingRQ pricingRQ = new RoomPricingRQ();
            pricingRQ.CallerSessionId = res.SessionId;
            pricingRQ.RoomId = res.Itinerary.Rooms[i].RoomId.ToString();
            SystemContracts.ServiceContracts.IEngineServiceRS roomPricingRS = await (new HotelRoomPricingRequestEngine()).RequestAsync(pricingRQ);
            //PricingRequestCache.AddToCache(pricingRQ.CallerSessionId, pricingRQ.RoomId);
            //ExternalServices.PricingPolicyEngine.TripProductPriceRQ tripProductPriceRQ = (new TripProductPriceRQParser()).Parse(pricingRQ);
            //var result = await (new ExternalServices.PricingPolicyEngine.TripsEngineClient()).PriceTripProductAsync(tripProductPriceRQ);
         //   TripProductCache.AddToCache(result.SessionId, result.TripProduct);
            var ghu = ItineraryCache.GetItineraries(hotelroomavail.SessionId);
            //foreach (HotelItinerary iti in ghu)
            //{
            //    hotelroomavail = new HotelRoomAvailRQ();
            //    hotelroomavail.HotelSearchCriterion = SearchCriterionCache.GetSearchCriterion(((MultiAvailHotelSearchRS)response).CallerSessionId);
            //    hotelroomavail.Itinerary = iti;
            //    hotelroomavail.SessionId = ((MultiAvailHotelSearchRS)response).CallerSessionId;
            //    hotelroomavail.ResultRequested = ResponseType.Complete;
            //    res = await client.HotelRoomAvailAsync(hotelroomavail);
            //    rq.HotelSearchCriterion = SearchCriterionCache.GetSearchCriterion(res.SessionId);
            //    rq.Itinerary = res.Itinerary;
            //    rq.SessionId = res.SessionId;
            //    rq.ResultRequested = ResponseType.Complete;
            //    rq.AdditionalInfo = rq.HotelSearchCriterion.Pos.AdditionalInfo;
            //    var res1 = await client.HotelRoomPriceAsync(rq);

            //}
            req = new HotelProductBookRQ()
            {
                CallerSessionId = ((MultiAvailHotelSearchRS)response).CallerSessionId,
                Guests = new SystemContracts.Attributes.Guest[1]
                {
                    new SystemContracts.Attributes.Guest()
                    {
                        Name = new SystemContracts.Attributes.Name()
                        {
                            FirstName = "Divas",
                            MiddleName = "Kumar",
                            LastName = "Agarwal"
                        },
                        DateOfBirth = DateTime.Parse("08-10-1995"),
                        Age = 22,
                        Email = "dagarwal@tavisca.com",
                        Gender = 'M',
                        Type = "Adult"
                    }
                },
                PaymentDetails = new SystemContracts.Attributes.PaymentDetails()
                {
                    Amount = itinerary[0].Fare.TotalFare.Amount,
                    Currency = itinerary[0].Fare.TotalFare.Currency,
                    BillingAddress = new SystemContracts.Attributes.UserBillingAddress()
                    {
                        AddressLine1 = "Line 1",
                        AddressLine2 = "Line 2",
                        AddressContext = "Address",
                        City = "Agra",
                        State = "UP",
                        Country = "IN",
                        ZipCode = "423423",
                        PhoneNumber = "1234567890"
                    },
                    CreditCardDetails = new SystemContracts.Attributes.CreditCardDetails()
                    {
                        NameOnCard = "Divas",
                        CardNumber = "4444333322221111",
                        Cvv = "123",
                        Code = "VI",
                        CardName = "VISA",
                        Expiry = DateTime.Now.AddYears(2),
                        IsThreeDAuth = true
                    }
                }
            };
            var RS = (new TripFolderBookRQParser()).Parse(req);
            var mainRs = await (new ExternalServices.PricingPolicyEngine.TripsEngineClient()).BookTripFolderAsync(RS);
            var staginginfo = (new StagingRSParser()).Parse(mainRs);
            var cRq = (new CompleteBookingRQParser()).Parse(staginginfo);
            var finalRS = await (new ExternalServices.PricingPolicyEngine.TripsEngineClient()).CompleteBookingAsync(cRq);
        }
    }
}

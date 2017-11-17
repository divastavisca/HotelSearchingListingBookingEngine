using Microsoft.VisualStudio.TestTools.UnitTesting;
using HotelSearchingListingBooking.Translators;
using HotelSearchingListingBookingEngine.Core;
using SystemContracts.ConsumerContracts;
using System;
using HotelSearchingListingBookingEngine.Core.ServiceEngines;
using System.Threading;
using System.Threading.Tasks;
using HotelSearchingListingBooking.ExternalServices.HotelSearchEngine;
using HotelSearchingListingBooking.Caches;

namespace CoreEngine.Tests
{
    [TestClass]
    public class HotelFlowTest
    {
        HotelSearchRQTranslator parser;
        MultiAvailHotelSearchRQ request;
        MultiAvailHotelSearchEngine engine;
        HotelProductBookRQ req;

        public HotelFlowTest()
        {
            parser = new HotelSearchRQTranslator();
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
            hotelroomavail.SessionId = ((MultiAvailHotelSearchRS)response).CallerSessionId;
            hotelroomavail.ResultRequested = ResponseType.Complete;
            HotelEngineClient client = new HotelEngineClient();
            var res = await client.HotelRoomAvailAsync(hotelroomavail);
            SelectedItineraryCache.AddToCache(res.SessionId, res.Itinerary);
            i = 0;
            foreach (Room ro in res.Itinerary.Rooms)
            {
                if (ro.HotelFareSource.Name.StartsWith("TouricoTGSTest")|| ro.HotelFareSource.Name.StartsWith("HotelBeds"))
                    break;
                i++;
            }
            RoomPricingRQ pricingRQ = new RoomPricingRQ();
            SelectedItineraryRoomsCache.AddToCache(res.SessionId, res.Itinerary.Rooms);
            pricingRQ.CallerSessionId = res.SessionId;
            pricingRQ.RoomId = res.Itinerary.Rooms[i].RoomId.ToString();
            SystemContracts.ServiceContracts.IEngineServiceRS roomPricingRS = await (new HotelRoomPricingRequestEngine()).RequestAsync(pricingRQ);
            var ghu = ItineraryCache.GetItineraries(hotelroomavail.SessionId);
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
                            MiddleName = null,
                            LastName = "Agarwal"
                        },
                        DateOfBirth = DateTime.Parse("1995-08-10"),
                        Age = 22,
                        Email = "dagarwal@tavisca.com",
                        Gender = 'M',
                        Type = "Adult"
                    }
                },
                PaymentDetails = new SystemContracts.Attributes.PaymentDetails()
                {
                    Price = itinerary[0].Fare.TotalFare.Amount.ToString()+itinerary[0].Fare.TotalFare.Currency,
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
            SystemContracts.ServiceContracts.IEngineServiceRS rsRes = await (new BookingServiceCoordinator()).RequestAsync(req);
        }
    }
}

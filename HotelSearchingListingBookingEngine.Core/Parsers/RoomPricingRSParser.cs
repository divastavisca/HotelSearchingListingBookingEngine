using System;
using System.Collections.Generic;
using System.Text;
using ExternalServices.HotelSearchEngine;
using SystemContracts.ConsumerContracts;
using HotelSearchingListingBookingEngine.Core.CustomExceptions;
using HotelSearchingListingBookingEngine.Core.Caches;

namespace HotelSearchingListingBookingEngine.Core.Parsers
{
    public class RoomPricingRSParser
    {
        public RoomPricingRS Parse(ExternalServices.PricingPolicyEngine.TripProductPriceRS hotelRoomPriceRS)
        {
            try
            {
                RoomPricingRS parsedRS = new RoomPricingRS();
                parsedRS.CallerSessionId = hotelRoomPriceRS.SessionId;
                if (hotelRoomPriceRS.TripProduct == null)
                    parsedRS.IsUpdated = false;
                else parsedRS.IsUpdated = true;
                if (PricingRequestCache.IsPresent(hotelRoomPriceRS.SessionId) == false)
                    throw new InvalidObjectRequestException()
                    {
                        Source = typeof(HotelItinerary).Name
                    };
                parsedRS.Currency = ((ExternalServices.PricingPolicyEngine.HotelTripProduct)hotelRoomPriceRS.TripProduct).HotelItinerary.Rooms[0].DisplayRoomRate.TotalFare.Currency;
                parsedRS.RoomPrice = ((ExternalServices.PricingPolicyEngine.HotelTripProduct)hotelRoomPriceRS.TripProduct).HotelItinerary.Rooms[0].DisplayRoomRate.TotalFare.Amount;
                if (TripProductCache.IsPresent(hotelRoomPriceRS.SessionId))
                    TripProductCache.Remove(hotelRoomPriceRS.SessionId);
                TripProductCache.AddToCache(hotelRoomPriceRS.SessionId, hotelRoomPriceRS.TripProduct);
                return parsedRS;
                throw new InvalidObjectRequestException()
                {
                    Source = typeof(ExternalServices.PricingPolicyEngine.Room).Name
                };
            }
            catch (InvalidObjectRequestException invalidObjectRequestException)
            {
                Logger.LogException(invalidObjectRequestException.ToString(), invalidObjectRequestException.StackTrace);
                throw new ServiceResponseParserException()
                {
                    Source = invalidObjectRequestException.Source
                };
            }
            catch (Exception baseException)
            {
                Logger.LogException(baseException.ToString(), baseException.StackTrace);
                throw new ServiceResponseParserException()
                {
                    Source = baseException.Source
                };
            }
        }
    }
}

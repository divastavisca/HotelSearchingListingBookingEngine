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
        public RoomPricingRS Parse(HotelRoomPriceRS hotelRoomPriceRS)
        {
            try
            {
                RoomPricingRS parsedRS = new RoomPricingRS();
                parsedRS.CallerSessionId = hotelRoomPriceRS.SessionId;
                if (hotelRoomPriceRS.Itinerary == null)
                    parsedRS.IsUpdated = false;
                else parsedRS.IsUpdated = true;
                if (PricingRequestCache.IsPresent(hotelRoomPriceRS.SessionId) == false)
                    throw new InvalidObjectRequestException()
                    {
                        Source = typeof(HotelItinerary).Name
                    };
                var roomId = PricingRequestCache.GetSelectedRoomId(hotelRoomPriceRS.SessionId);
                foreach(Room hotelRoom in hotelRoomPriceRS.Itinerary.Rooms)
                {
                    if(hotelRoom.RoomId.ToString() == roomId)
                    {
                        parsedRS.Currency = hotelRoom.DisplayRoomRate.TotalFare.Currency;
                        parsedRS.RoomPrice = hotelRoom.DisplayRoomRate.TotalFare.Amount;
                        return parsedRS;
                    }
                }
                throw new InvalidObjectRequestException()
                {
                    Source = typeof(Room).Name
                };
            }
            catch(InvalidObjectRequestException invalidObjectRequestException)
            {
                Logger.LogException(invalidObjectRequestException.ToString(), invalidObjectRequestException.StackTrace);
                throw new ServiceResponseParserException()
                {
                    Source = invalidObjectRequestException.Source
                };
            }
            catch(Exception baseException)
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

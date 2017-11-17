using System;
using System.Collections.Generic;
using System.Text;
using HotelSearchingListingBooking.ExternalServices.HotelSearchEngine;
using SystemContracts.ConsumerContracts;
using SystemContracts.CustomExceptions;
using HotelSearchingListingBooking.Caches;
using HotelSearchingListingBooking.Translators.Utilities;

namespace HotelSearchingListingBooking.Translators
{
    public class RoomPricingRSTranslator
    {
        public RoomPricingRS Translate(ExternalServices.PricingPolicyEngine.TripProductPriceRS hotelRoomPriceRS)
        {
            try
            {
                RoomPricingRS _translatedRS = new RoomPricingRS();
                _translatedRS.CallerSessionId = hotelRoomPriceRS.SessionId;
                if (hotelRoomPriceRS.TripProduct == null)
                    _translatedRS.IsUpdated = false;
                else _translatedRS.IsUpdated = true;
                if (PricingRequestCache.IsPresent(hotelRoomPriceRS.SessionId) == false)
                    throw new InvalidObjectRequestException()
                    {
                        Source = typeof(HotelItinerary).Name
                    };
                _translatedRS.Currency = ((ExternalServices.PricingPolicyEngine.HotelTripProduct)hotelRoomPriceRS.TripProduct).HotelItinerary.Rooms[0].DisplayRoomRate.TotalFare.Currency;
                _translatedRS.RoomPrice = ((ExternalServices.PricingPolicyEngine.HotelTripProduct)hotelRoomPriceRS.TripProduct).HotelItinerary.Rooms[0].DisplayRoomRate.TotalFare.Amount;
                if (TripProductCache.IsPresent(hotelRoomPriceRS.SessionId))
                    TripProductCache.Remove(hotelRoomPriceRS.SessionId);
                TripProductCache.AddToCache(hotelRoomPriceRS.SessionId, hotelRoomPriceRS.TripProduct);
                return _translatedRS;
                throw new InvalidObjectRequestException()
                {
                    Source = typeof(ExternalServices.PricingPolicyEngine.Room).Name
                };
            }
            catch (InvalidObjectRequestException invalidObjectRequestException)
            {
                Logger.LogException(invalidObjectRequestException.ToString(), invalidObjectRequestException.StackTrace);
                throw new ServiceResponseTranslatorException()
                {
                    Source = invalidObjectRequestException.Source
                };
            }
            catch (Exception baseException)
            {
                Logger.LogException(baseException.ToString(), baseException.StackTrace);
                throw new ServiceResponseTranslatorException()
                {
                    Source = baseException.Source
                };
            }
        }
    }
}

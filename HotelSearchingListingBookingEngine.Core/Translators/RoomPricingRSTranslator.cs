using System;
using System.Collections.Generic;
using System.Text;
using ExternalServices.HotelSearchEngine;
using SystemContracts.ConsumerContracts;
using HotelSearchingListingBookingEngine.Core.CustomExceptions;
using HotelSearchingListingBookingEngine.Core.Caches;

namespace HotelSearchingListingBookingEngine.Core.Translators
{
    public class RoomPricingRSTranslator
    {
        public RoomPricingRS Translate(ExternalServices.PricingPolicyEngine.TripProductPriceRS hotelRoomPriceRS)
        {
            try
            {
                RoomPricingRS translatedRS = new RoomPricingRS();
                translatedRS.CallerSessionId = hotelRoomPriceRS.SessionId;
                if (hotelRoomPriceRS.TripProduct == null)
                    translatedRS.IsUpdated = false;
                else translatedRS.IsUpdated = true;
                if (PricingRequestCache.IsPresent(hotelRoomPriceRS.SessionId) == false)
                    throw new InvalidObjectRequestException()
                    {
                        Source = typeof(HotelItinerary).Name
                    };
                translatedRS.Currency = ((ExternalServices.PricingPolicyEngine.HotelTripProduct)hotelRoomPriceRS.TripProduct).HotelItinerary.Rooms[0].DisplayRoomRate.TotalFare.Currency;
                translatedRS.RoomPrice = ((ExternalServices.PricingPolicyEngine.HotelTripProduct)hotelRoomPriceRS.TripProduct).HotelItinerary.Rooms[0].DisplayRoomRate.TotalFare.Amount;
                if (TripProductCache.IsPresent(hotelRoomPriceRS.SessionId))
                    TripProductCache.Remove(hotelRoomPriceRS.SessionId);
                TripProductCache.AddToCache(hotelRoomPriceRS.SessionId, hotelRoomPriceRS.TripProduct);
                return translatedRS;
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

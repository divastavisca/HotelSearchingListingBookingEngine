using System;
using System.Collections.Generic;
using System.Text;
using ExternalServices.HotelSearchEngine;
using SystemContracts.ConsumerContracts;
using HotelSearchingListingBookingEngine.Core.CustomExceptions;
using HotelSearchingListingBookingEngine.Core.Caches;


namespace HotelSearchingListingBookingEngine.Core.Parsers
{
    public class HotelRoomPriceRQParser
    {
        public HotelRoomPriceRQ Parse(RoomPricingRQ hotelRoomPricingRQ)
        {
            try
            {
                var userSelectedItinerary = SelectedItineraryCache.GetSelecetedItinerary(hotelRoomPricingRQ.CallerSessionId);
                if (userSelectedItinerary == null)
                    throw new InvalidObjectRequestException()
                    {
                        Source = userSelectedItinerary.GetType().Name
                    };
                HotelRoomPriceRQ parsedRQ = new HotelRoomPriceRQ();
                parsedRQ.HotelSearchCriterion = SearchCriterionCache.GetSearchCriterion(hotelRoomPricingRQ.CallerSessionId);
                if (parsedRQ.HotelSearchCriterion == null)
                    throw new InvalidObjectRequestException()
                    {
                        Source = parsedRQ.HotelSearchCriterion.GetType().Name
                    };
                parsedRQ.SessionId = hotelRoomPricingRQ.CallerSessionId;
                parsedRQ.Itinerary = userSelectedItinerary;
                parsedRQ.ResultRequested = ResponseType.Complete;
                if (PricingRequestCache.IsPresent(parsedRQ.SessionId))
                    PricingRequestCache.Remove(parsedRQ.SessionId);
                PricingRequestCache.AddToCache(parsedRQ.SessionId, hotelRoomPricingRQ.RoomId);
                return parsedRQ;
            }
            catch(InvalidObjectRequestException invalidObjectRequestException)
            {
                Logger.LogException(invalidObjectRequestException.ToString(), invalidObjectRequestException.StackTrace);
                throw new ServiceRequestParserException()
                {
                    Source = invalidObjectRequestException.Source
                };
            }
            catch(NullReferenceException nullRefException)
            {
                Logger.LogException(nullRefException.ToString(), nullRefException.StackTrace);
                throw new ServiceRequestParserException()
                {
                    Source = nullRefException.Source
                };
            }
            catch(Exception baseException)
            {
                Logger.LogException(baseException.ToString(), baseException.StackTrace);
                throw new ServiceRequestParserException()
                {
                    Source = baseException.Source
                };
            }
        }
    }
}

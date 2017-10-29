using System;
using System.Collections.Generic;
using System.Text;
using ExternalServices.HotelSearchEngine;
using SystemContracts.ConsumerContracts;
using HotelSearchingListingBookingEngine.Core.CustomExceptions;
using HotelSearchingListingBookingEngine.Core.Caches;

namespace HotelSearchingListingBookingEngine.Core.Translators
{
    public class HotelRoomAvailRQTranslator
    { 
        public HotelRoomAvailRQ Translate(SingleAvailRoomSearchRQ singleAvailRoomSearchRQ)
        {
            try
            {
                HotelRoomAvailRQ translatedRQ = new HotelRoomAvailRQ()
                {
                    SessionId = singleAvailRoomSearchRQ.CallerSessionId,
                    ResultRequested = ResponseType.Complete,
                    HotelSearchCriterion = SearchCriterionCache.GetSearchCriterion(singleAvailRoomSearchRQ.CallerSessionId)
                };
                translatedRQ.Itinerary = getRequiredItinerary(singleAvailRoomSearchRQ.CallerSessionId, singleAvailRoomSearchRQ.ItineraryId);
                if (translatedRQ.Itinerary == null)
                    throw new InvalidObjectRequestException()
                    {
                        Source = translatedRQ.Itinerary.GetType().Name
                    };
                return translatedRQ;
            }
            catch(InvalidObjectRequestException invalidObjectRequestException)
            {
                Logger.LogException(invalidObjectRequestException.ToString(), invalidObjectRequestException.StackTrace);
                throw new ServiceRequestTranslatorException()
                {
                    Source = invalidObjectRequestException.Source
                };
            }
            catch(NullReferenceException nullRefExcep)
            {
                Logger.LogException(nullRefExcep.ToString(), nullRefExcep.StackTrace);
                throw new ServiceRequestTranslatorException()
                {
                    Source = nullRefExcep.Source
                };
            }
            catch(Exception baseException)
            {
                Logger.LogException(baseException.ToString(), baseException.StackTrace);
                throw new ServiceRequestTranslatorException()
                {
                    Source = baseException.Source
                };
            }
        }

        private HotelItinerary getRequiredItinerary(string callerSessionId,string requiredItineraryId)
        {
            try
            {
                var storedResults = ItineraryCache.GetItineraries(callerSessionId);
                foreach (HotelItinerary hotelItinerary in storedResults)
                {
                    if (hotelItinerary.HotelProperty.SupplierHotelId == requiredItineraryId)
                        return hotelItinerary;
                }
                return null;
            }
            catch (NullReferenceException nullRefExcep)
            {
                Logger.LogException(nullRefExcep.ToString(), nullRefExcep.StackTrace);
                throw new Exception();
            }
            catch (Exception baseException)
            {
                Logger.LogException(baseException.ToString(), baseException.StackTrace);
                throw new Exception();
            }
        }
    }
}

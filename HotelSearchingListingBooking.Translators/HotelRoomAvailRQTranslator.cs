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
    public class HotelRoomAvailRQTranslator
    { 
        public HotelRoomAvailRQ Translate(SingleAvailRoomSearchRQ singleAvailRoomSearchRQ)
        {
            try
            {
                HotelRoomAvailRQ _translatedRQ = new HotelRoomAvailRQ()
                {
                    SessionId = singleAvailRoomSearchRQ.CallerSessionId,
                    ResultRequested = ResponseType.Complete,
                    HotelSearchCriterion = SearchCriterionCache.GetSearchCriterion(singleAvailRoomSearchRQ.CallerSessionId)
                };
                _translatedRQ.Itinerary = getRequiredItinerary(singleAvailRoomSearchRQ.CallerSessionId, singleAvailRoomSearchRQ.ItineraryId);
                if (_translatedRQ.Itinerary == null)
                    throw new InvalidObjectRequestException()
                    {
                        Source = _translatedRQ.Itinerary.GetType().Name
                    };
                return _translatedRQ;
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

using System;
using System.Collections.Generic;
using System.Text;
using ExternalServices.HotelSearchEngine;
using SystemContracts.ConsumerContracts;

namespace HotelSearchingListingBookingEngine.Core.Parsers
{
    public class HotelRoomAvailRQParser
    {
        public HotelRoomAvailRQ Parse(SingleAvailRoomSearchRQ singleAvailRoomSearchRQ)
        {
            try
            {
                HotelRoomAvailRQ parsedRQ = new HotelRoomAvailRQ()
                {
                    SessionId = singleAvailRoomSearchRQ.CallerSessionId,
                    ResultRequested = ResponseType.Complete,
                    HotelSearchCriterion = SearchCriterionCache.GetSearchCriterion(singleAvailRoomSearchRQ.CallerSessionId)
                };
                parsedRQ.Itinerary = getRequiredItinerary(singleAvailRoomSearchRQ.CallerSessionId, singleAvailRoomSearchRQ.ItineraryId);
                if (parsedRQ.Itinerary == null)
                    throw new Exception("Cannot identify required itinerary");
                return parsedRQ;
            }
            catch(NullReferenceException nullRefExcep)
            {
                Logger.LogException(nullRefExcep.ToString(), nullRefExcep.StackTrace);
                return null;
            }
            catch(Exception baseException)
            {
                Logger.LogException(baseException.ToString(), baseException.StackTrace);
                return null;
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
                return null;
            }
            catch (Exception baseException)
            {
                Logger.LogException(baseException.ToString(), baseException.StackTrace);
                return null;
            }
        }
    }
}

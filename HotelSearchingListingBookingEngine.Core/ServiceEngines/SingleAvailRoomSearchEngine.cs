using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SystemContracts.ServiceContracts;
using HotelSearchingListingBookingEngine.Core.Translators;
using SystemContracts.ConsumerContracts;
using ExternalServices.HotelSearchEngine;
using HotelSearchingListingBookingEngine.Core.CustomExceptions;
using HotelSearchingListingBookingEngine.Core.Caches;

namespace HotelSearchingListingBookingEngine.Core.ServiceEngines
{
    public class SingleAvailRoomSearchEngine : ISearchServiceEngine
    {
        public async Task<IEngineServiceRS> SearchAsync(IEngineServiceRQ serviceRequest)
        {
            try
            {
                var singleAvailRoomSearchRQ = (SingleAvailRoomSearchRQ)serviceRequest;
                if (ItineraryCache.IsPresent(singleAvailRoomSearchRQ.CallerSessionId))
                {
                    HotelRoomAvailRQ parsedSingleAvailRQ = (new HotelRoomAvailRQTranslator()).Translate(singleAvailRoomSearchRQ);
                    if (parsedSingleAvailRQ == null)
                        throw new ParseException()
                        {
                            Source = parsedSingleAvailRQ.GetType().Name
                        };
                    HotelRoomAvailRS hotelRoomSearchRS = await (new HotelEngineClient()).HotelRoomAvailAsync(parsedSingleAvailRQ);
                    if (hotelRoomSearchRS == null)
                        throw new ObjectFetchException()
                        {
                            Source = hotelRoomSearchRS.GetType().Name
                        };
                    if (hotelRoomSearchRS.Itinerary == null)
                        throw new InvalidObjectRequestException()
                        {
                            Source = hotelRoomSearchRS.Itinerary.GetType().Name
                        };
                    if (SelectedItineraryCache.IsPresent(hotelRoomSearchRS.SessionId))
                    {
                        SelectedItineraryCache.Remove(hotelRoomSearchRS.SessionId);
                        if (SelectedItineraryRoomsCache.IsPresent(hotelRoomSearchRS.SessionId))
                        {
                            SelectedItineraryRoomsCache.Remove(hotelRoomSearchRS.SessionId);
                            if (PricingRequestCache.IsPresent(hotelRoomSearchRS.SessionId))
                            {
                                PricingRequestCache.Remove(hotelRoomSearchRS.SessionId);
                                if (TripProductCache.IsPresent(hotelRoomSearchRS.SessionId))
                                    TripProductCache.Remove(hotelRoomSearchRS.SessionId);
                            }
                        }
                    }
                    SelectedItineraryCache.AddToCache(hotelRoomSearchRS.SessionId, hotelRoomSearchRS.Itinerary);
                    SelectedItineraryRoomsCache.AddToCache(hotelRoomSearchRS.SessionId, hotelRoomSearchRS.Itinerary.Rooms);
                    SingleAvailRoomSearchRS engineSearchRS = (new SingleAvailRoomSearchRSTranslator()).Translate(hotelRoomSearchRS);
                    if (engineSearchRS == null)
                        throw new ParseException()
                        {
                            Source = engineSearchRS.GetType().Name
                        };
                    return engineSearchRS;
                }
                else return null;
            }
            catch(ObjectFetchException objectFetchException)
            {
                Logger.LogException(objectFetchException.ToString(), objectFetchException.StackTrace);
                throw new SearchEngineException()
                {
                    Source = objectFetchException.Source
                };
            }
            catch(InvalidObjectRequestException invalidObjectRequestException)
            {
                if(SelectedItineraryCache.IsPresent(((SingleAvailRoomSearchRQ)serviceRequest).CallerSessionId))
                {
                    SelectedItineraryCache.Remove(((SingleAvailRoomSearchRQ)serviceRequest).CallerSessionId);
                }
                Logger.LogException(invalidObjectRequestException.ToString(), invalidObjectRequestException.StackTrace);
                throw new SearchEngineException()
                {
                    Source = invalidObjectRequestException.Source
                };
            }
            catch(ParseException parseException)
            {
                Logger.LogException(parseException.ToString(), parseException.StackTrace);
                throw new SearchEngineException()
                {
                    Source = parseException.Source
                };
            }
            catch(NullReferenceException nullRefExcep)
            {
                Logger.LogException(nullRefExcep.ToString(), nullRefExcep.StackTrace);
                throw new SearchEngineException()
                {
                    Source = nullRefExcep.Source
                };
            }
            catch(Exception baseExcep)
            {
                Logger.LogException(baseExcep.ToString(), baseExcep.StackTrace);
                throw new SearchEngineException()
                {
                    Source = baseExcep.Source
                };
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SystemContracts.ServiceContracts;
using HotelSearchingListingBookingEngine.Core.Parsers;
using SystemContracts.ConsumerContracts;
using ExternalServices.HotelSearchEngine;
using HotelSearchingListingBookingEngine.Core.CustomExceptions;
using HotelSearchingListingBookingEngine.Core.Caches;

namespace HotelSearchingListingBookingEngine.Core.ServiceEngines
{
    public class SingleAvailRoomSearchEngine : ISearchEngine
    {
        public async Task<IEngineServiceRS> SearchAsync(IEngineServiceRQ serviceRequest)
        {
            try
            {
                var singleAvailRoomSearchRQ = (SingleAvailRoomSearchRQ)serviceRequest;
                if (ItineraryCache.IsPresent(singleAvailRoomSearchRQ.CallerSessionId))
                {
                    HotelRoomAvailRQ parsedSingleAvailRQ = (new HotelRoomAvailRQParser()).Parse(singleAvailRoomSearchRQ);
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
                    SingleAvailRoomSearchRS engineSearchRS = (new SingleAvailRoomSearchRSParser()).Parse(hotelRoomSearchRS);
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

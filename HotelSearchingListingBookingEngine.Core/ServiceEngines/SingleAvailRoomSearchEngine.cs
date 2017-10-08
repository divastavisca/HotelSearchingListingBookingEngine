using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SystemContracts.ServiceContracts;
using HotelSearchingListingBookingEngine.Core.Parsers;
using SystemContracts.ConsumerContracts;
using ExternalServices.HotelSearchEngine;

namespace HotelSearchingListingBookingEngine.Core.ServiceEngines
{
    public class SingleAvailRoomSearchEngine : ISearchEngine
    {
        public Task<IEngineServiceRS> SearchAsync(IEngineServiceRQ serviceRequest)
        {
            try
            {
                var singleAvailRoomSearchRQ = (SingleAvailRoomSearchRQ)serviceRequest;
                if (ItineraryCache.IsPresent(singleAvailRoomSearchRQ.CallerSessionId))
                {
                    HotelRoomAvailRQ parsedSingleAvailRQ = (new HotelRoomAvailRQParser()).Parse(singleAvailRoomSearchRQ);
                    if (parsedSingleAvailRQ == null)
                        throw new Exception("Unable to parse single avail request");
                    Task<HotelRoomAvailRS> hotelRoomSearchRS = (new HotelEngineClient()).HotelRoomAvailAsync(parsedSingleAvailRQ);
                    if (hotelRoomSearchRS == null)
                        throw new Exception("Unable to fetch room data");

                }
                else return null;
            }
            catch(NullReferenceException nullRefExcep)
            {
                Logger.LogException(nullRefExcep.ToString(), nullRefExcep.StackTrace);
                return null;
            }
            catch(Exception baseExcep)
            {
                Logger.LogException(baseExcep.ToString(), baseExcep.StackTrace);
                return null;
            }
        }
    }
}

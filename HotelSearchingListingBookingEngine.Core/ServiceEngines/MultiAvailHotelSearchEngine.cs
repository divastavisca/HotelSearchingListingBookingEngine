using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SystemContracts.ConsumerContracts;
using SystemContracts.ServiceContracts;
using ExternalServices.HotelSearchEngine;
using HotelSearchingListingBookingEngine.Core.Parsers;
using HotelSearchingListingBookingEngine.Core;

namespace HotelSearchingListingBookingEngine.Core.ServiceEngines
{
    public class MultiAvailHotelSearchEngine
    {
        public async Task<IEngineServiceRS> SearchAsync(IEngineServiceRQ searchRQ)
        {
            try
            {
                HotelSearchRQ hotelSearchRQ = (new HotelSearchRQParser()).Parse((MultiAvailHotelSearchRQ)searchRQ);
                HotelSearchRS hotelSearchRS = await (new HotelEngineClient()).HotelAvailAsync(hotelSearchRQ);
                if (ItineraryCache.IsPresent(hotelSearchRS.SessionId) == false)
                    ItineraryCache.AddToCache(hotelSearchRS.SessionId, hotelSearchRS.Itineraries);
                else
                {
                    ItineraryCache.Remove(hotelSearchRS.SessionId);
                    ItineraryCache.AddToCache(hotelSearchRS.SessionId, hotelSearchRS.Itineraries);
                }
                return (new MultiAvailHotelSearchRSParser()).Parse(hotelSearchRS);
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
    }
}

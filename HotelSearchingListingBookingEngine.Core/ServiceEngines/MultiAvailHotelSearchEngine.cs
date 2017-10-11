using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SystemContracts.ConsumerContracts;
using SystemContracts.ServiceContracts;
using ExternalServices.HotelSearchEngine;
using HotelSearchingListingBookingEngine.Core.Parsers;
using HotelSearchingListingBookingEngine.Core;
using HotelSearchingListingBookingEngine.Core.CustomExceptions;
using HotelSearchingListingBookingEngine.Core.Caches;

namespace HotelSearchingListingBookingEngine.Core.ServiceEngines
{
    public class MultiAvailHotelSearchEngine : ISearchEngine
    {
        public async Task<IEngineServiceRS> SearchAsync(IEngineServiceRQ searchRQ)
        {
            try
            {
                HotelSearchRQ hotelSearchRQ = (new HotelSearchRQParser()).Parse((MultiAvailHotelSearchRQ)searchRQ);
                HotelSearchRS hotelSearchRS = await (new HotelEngineClient()).HotelAvailAsync(hotelSearchRQ);
                if (hotelSearchRS.Itineraries == null || hotelSearchRS.Itineraries.Length == 0)
                    throw new NoResultsFoundException();
                if (ItineraryCache.IsPresent(hotelSearchRS.SessionId) == false)
                {
                    ItineraryCache.AddToCache(hotelSearchRS.SessionId, hotelSearchRS.Itineraries);
                    SearchCriterionCache.AddToCache(hotelSearchRS.SessionId, hotelSearchRQ.HotelSearchCriterion);
                }
                else
                {
                    ItineraryCache.Remove(hotelSearchRS.SessionId);
                    ItineraryCache.AddToCache(hotelSearchRS.SessionId, hotelSearchRS.Itineraries);
                    SearchCriterionCache.Remove(hotelSearchRS.SessionId);
                    SearchCriterionCache.AddToCache(hotelSearchRS.SessionId, hotelSearchRQ.HotelSearchCriterion);
                }
                return (new MultiAvailHotelSearchRSParser()).Parse(hotelSearchRS);
            }
            catch(ServiceRequestParserException requestParserException)
            {
                Logger.LogException(requestParserException.ToString(), requestParserException.StackTrace);
                throw new SearchEngineException()
                {
                    Source = requestParserException.Source
                };
            }
            catch (NullReferenceException nullRefExcep)
            {
                Logger.LogException(nullRefExcep.ToString(), nullRefExcep.StackTrace);
                throw new SearchEngineException()
                {
                    Source = nullRefExcep.Source
                };
            }
            catch (NoResultsFoundException noResultsFoundExcetion)
            {
                Logger.StoreLog(noResultsFoundExcetion.ToString());
                throw new SearchEngineException()
                {
                    Source = noResultsFoundExcetion.Source
                };
            }
            catch (Exception baseException)
            {
                Logger.LogException(baseException.ToString(), baseException.StackTrace);
                throw new SearchEngineException()
                {
                    Source = baseException.Source
                };
            }
        }
    }
}

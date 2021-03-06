﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SystemContracts.ConsumerContracts;
using SystemContracts.ServiceContracts;
using HotelSearchingListingBooking.ExternalServices.HotelSearchEngine;
using HotelSearchingListingBooking.Translators;
using HotelSearchingListingBookingEngine.Core;
using SystemContracts.CustomExceptions;
using HotelSearchingListingBooking.Caches;
using HotelSearchingListingBookingEngine.Core.Utilities;

namespace HotelSearchingListingBookingEngine.Core.ServiceEngines
{
    public class MultiAvailHotelSearchEngine : ISearchServiceEngine
    {
        public async Task<IEngineServiceRS> SearchAsync(IEngineServiceRQ searchRQ)
        {
            try
            {
                HotelSearchRQ hotelSearchRQ = (new HotelSearchRQTranslator()).Translate((MultiAvailHotelSearchRQ)searchRQ);
                CacheManager.RegisterSession(hotelSearchRQ.SessionId);
                HotelSearchRS hotelSearchRS = await (new HotelEngineClient()).HotelAvailAsync(hotelSearchRQ);
                if (hotelSearchRS.Itineraries == null || hotelSearchRS.Itineraries.Length == 0)
                    throw new NoResultsFoundException();
                updateCaches(hotelSearchRS.SessionId, hotelSearchRQ.HotelSearchCriterion, hotelSearchRS.Itineraries);
                return (new MultiAvailHotelSearchRSTranslator()).Translate(hotelSearchRS);
            }
            catch (CacheManagerException cacheManagerException)
            {
                Logger.LogException(cacheManagerException.ToString(), cacheManagerException.StackTrace);
                throw new SearchEngineException()
                {
                    Source = cacheManagerException.Source
                };
            }
            catch (ServiceRequestTranslatorException requestParserException)
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

        private void updateCaches(string sessionId, HotelSearchCriterion hotelSearchCriterion, HotelItinerary[] itineraries)
        {
            if (ItineraryCache.IsPresent(sessionId))
            {
                ItineraryCache.Remove(sessionId);
                SearchCriterionCache.Remove(sessionId);
                if (SelectedItineraryCache.IsPresent(sessionId))
                {
                    SelectedItineraryCache.Remove(sessionId);
                    if (SelectedItineraryRoomsCache.IsPresent(sessionId))
                        SelectedItineraryRoomsCache.Remove(sessionId);
                    if (PricingRequestCache.IsPresent(sessionId))
                    {
                        PricingRequestCache.Remove(sessionId);
                        if (TripProductCache.IsPresent(sessionId))
                            TripProductCache.Remove(sessionId);
                    }
                }
            }
            else
            {
                ItineraryCache.AddToCache(sessionId, itineraries);
                SearchCriterionCache.AddToCache(sessionId, hotelSearchCriterion);
            }
        }
    }
}

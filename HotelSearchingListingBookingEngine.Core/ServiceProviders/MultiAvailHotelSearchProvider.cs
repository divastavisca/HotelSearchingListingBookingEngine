using System;
using System.Collections.Generic;
using System.Text;
using SystemContracts.ServiceContracts;
using SystemContracts.Attributes.HotelAttributes;
using SystemContracts.Attributes;
using ExternalServices.HotelSearchEngine;
using HotelSearchingListingBookingEngine.Core.ServiceEngines;
using System.Threading.Tasks;
using HotelSearchingListingBookingEngine.Core.CustomExceptions;

namespace HotelSearchingListingBookingEngine.Core.ServiceProviders
{
    public class MultiAvailHotelSearchProvider : IEngineServiceProvider
    {
        public async Task<IEngineServiceRS> GetServiceRSAsync(IEngineServiceRQ serviceRQ)
        {
            try
            {
                return await (new MultiAvailHotelSearchEngine()).SearchAsync(serviceRQ);
            }
            catch(SearchEngineException searchEngineException)
            {
                Logger.LogException(searchEngineException.ToString(), searchEngineException.StackTrace);
                throw new ServiceProviderException()
                {
                    Source = searchEngineException.Source
                };
            }
            catch(Exception baseException)
            {
                Logger.LogException(baseException.ToString(), baseException.StackTrace);
                throw new ServiceProviderException()
                {
                    Source = baseException.Source
                };
            }
        }
    }
}

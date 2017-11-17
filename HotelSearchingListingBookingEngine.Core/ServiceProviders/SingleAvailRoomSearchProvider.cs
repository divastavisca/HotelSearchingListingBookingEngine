using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SystemContracts.ServiceContracts;
using HotelSearchingListingBookingEngine.Core.ServiceEngines;
using HotelSearchingListingBookingEngine.Core.Utilities;
using SystemContracts.CustomExceptions;

namespace HotelSearchingListingBookingEngine.Core.ServiceProviders
{
    public class SingleAvailRoomSearchProvider : IEngineServiceProvider
    {
        public async Task<IEngineServiceRS> GetServiceRSAsync(IEngineServiceRQ serviceRQ)
        {
            try
            {
                return await (new SingleAvailRoomSearchEngine()).SearchAsync(serviceRQ);
            }
            catch (SearchEngineException searchEngineException)
            {
                Logger.LogException(searchEngineException.ToString(), searchEngineException.StackTrace);
                throw new ServiceProviderException()
                {
                    Source = searchEngineException.Source
                };
            }
            catch (Exception baseException)
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

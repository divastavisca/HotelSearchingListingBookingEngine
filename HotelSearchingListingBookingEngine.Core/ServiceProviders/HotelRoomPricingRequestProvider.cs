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
    public class HotelRoomPricingRequestProvider : IEngineServiceProvider
    {
        public async Task<IEngineServiceRS> GetServiceRS(IEngineServiceRQ serviceRQ)
        {
            try
            {
                return await (new HotelRoomPricingRequestEngine()).RequestAsync(serviceRQ);
            }
            catch (PricingRequestEngineException pricingRequestEngineException)
            {
                Logger.LogException(pricingRequestEngineException.ToString(), pricingRequestEngineException.StackTrace);
                throw new ServiceProviderException()
                {
                    Source = pricingRequestEngineException.Source
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

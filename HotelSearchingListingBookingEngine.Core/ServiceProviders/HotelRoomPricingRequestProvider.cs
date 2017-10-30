using System;
using System.Collections.Generic;
using System.Text;
using SystemContracts.ServiceContracts;
using SystemContracts.Attributes.HotelAttributes;
using SystemContracts.Attributes;
using HotelSearchingListingBooking.ExternalServices.HotelSearchEngine;
using HotelSearchingListingBookingEngine.Core.ServiceEngines;
using HotelSearchingListingBookingEngine.Core.Utilities;
using System.Threading.Tasks;
using SystemContracts.CustomExceptions;


namespace HotelSearchingListingBookingEngine.Core.ServiceProviders
{
    public class HotelRoomPricingRequestProvider : IEngineServiceProvider
    {
        public async Task<IEngineServiceRS> GetServiceRSAsync(IEngineServiceRQ serviceRQ)
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

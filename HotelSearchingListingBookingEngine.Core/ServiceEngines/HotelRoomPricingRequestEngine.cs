using System;
using System.Collections.Generic;
using System.Text;
using SystemContracts.ConsumerContracts;
using SystemContracts.ServiceContracts;
using ExternalServices.HotelSearchEngine;
using HotelSearchingListingBookingEngine.Core.Parsers;
using HotelSearchingListingBookingEngine.Core;
using HotelSearchingListingBookingEngine.Core.CustomExceptions;
using HotelSearchingListingBookingEngine.Core.Caches;
using System.Threading.Tasks;

namespace HotelSearchingListingBookingEngine.Core.ServiceEngines
{
    public class HotelRoomPricingRequestEngine : IRequestServiceEngine
    {
        public Task<IEngineServiceRS> RequestAsync(IEngineServiceRQ engineServiceRQ)
        {
            try
            {
                
            }
            catch(ServiceRequestParserException serviceRequestParserException)
            {
                Logger.LogException(serviceRequestParserException.ToString(), serviceRequestParserException.StackTrace);
                throw new PricingRequestEngineException()
                {
                    Source = serviceRequestParserException.Source
                };
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ExternalServices.PricingPolicyEngine;
using SystemContracts.ServiceContracts;
using HotelSearchingListingBookingEngine.Core.CustomExceptions;
using HotelSearchingListingBookingEngine.Core.InternalEngineHandshakes;
using HotelSearchingListingBookingEngine.Core.Translators;

namespace HotelSearchingListingBookingEngine.Core.InternalServiceEngines
{
    public class HotelProductBookingEngine : IInternalServiceEngine
    {
        public async Task<IEngineServiceRS> ProcessAsync(IEngineServiceRQ engineServiceRQ)
        {
            try
            {
                CompleteBookingRQ completeBookingRQ = (new CompleteBookingRQTranslator()).Translate((ProductStagingInfo)engineServiceRQ);
                CompleteBookingRS completeBookingRS = await (new TripsEngineClient()).CompleteBookingAsync(completeBookingRQ);
                return (new ProductBookingRSTranslator()).Translate(completeBookingRS);
            }
            catch (ServiceRequestParserException serviceRequestParserException)
            {
                Logger.LogException(serviceRequestParserException.ToString(), serviceRequestParserException.StackTrace);
                throw new SupportingEngineException()
                {
                    Source = serviceRequestParserException.Source
                };
            }
            catch (ServiceResponseParserException serviceResponseParserException)
            {
                Logger.LogException(serviceResponseParserException.ToString(), serviceResponseParserException.StackTrace);
                throw new SupportingEngineException()
                {
                    Source = serviceResponseParserException.Source
                };
            }
            catch(Exception baseException)
            {
                Logger.LogException(baseException.ToString(), baseException.StackTrace);
                throw new SupportingEngineException()
                {
                    Source = baseException.Source
                };
            }
        }
    }
}

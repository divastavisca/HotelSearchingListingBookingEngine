using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using HotelSearchingListingBooking.ExternalServices.PricingPolicyEngine;
using SystemContracts.ServiceContracts;
using SystemContracts.CustomExceptions;
using HotelSearchingListingBookingEngine.Core.InternalEngineHandshakes;
using HotelSearchingListingBookingEngine.Core.Utilities;
using HotelSearchingListingBooking.Translators;

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
            catch (ServiceRequestTranslatorException serviceRequestParserException)
            {
                Logger.LogException(serviceRequestParserException.ToString(), serviceRequestParserException.StackTrace);
                throw new SupportingEngineException()
                {
                    Source = serviceRequestParserException.Source
                };
            }
            catch (ServiceResponseTranslatorException serviceResponseParserException)
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

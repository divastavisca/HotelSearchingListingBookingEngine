using System;
using System.Collections.Generic;
using System.Text;
using ExternalServices.PricingPolicyEngine;
using HotelSearchingListingBookingEngine.Core.Parsers;
using System.Threading.Tasks;
using SystemContracts.ServiceContracts;
using HotelSearchingListingBookingEngine.Core.CustomExceptions;
using SystemContracts.ConsumerContracts;
using HotelSearchingListingBookingEngine.Core.InternalEngineHandshakes;

namespace HotelSearchingListingBookingEngine.Core.InternalServiceEngines
{
    public class HotelProductStagingEngine : IInternalServiceEngine
    {
        public async Task<IEngineServiceRS> ProcessAsync(IEngineServiceRQ productStagingRQ)
        {
            try
            {
                TripFolderBookRQ tripFolderBookRQ = (new TripFolderBookRQParser()).Parse((HotelProductBookRQ)productStagingRQ);
                TripFolderBookRS tripFolderBookRS =  await (new TripsEngineClient()).BookTripFolderAsync(tripFolderBookRQ);
                return (new StagingRSParser()).Parse(tripFolderBookRS);
            }
            catch(ServiceRequestParserException serviceRequestParserException)
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
            catch (Exception baseException)
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

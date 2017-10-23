using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SystemContracts.ServiceContracts;
using ExternalServices.PricingPolicyEngine;
using HotelSearchingListingBookingEngine.Core.InternalEngineHandshakes;
using HotelSearchingListingBookingEngine.Core.InternalServiceEngines;
using HotelSearchingListingBookingEngine.Core.CustomExceptions;

namespace HotelSearchingListingBookingEngine.Core.ServiceEngines
{
    public class BookingServiceCoordinator : IRequestServiceEngine
    {
        private readonly string _statgingSupportEngineAlias = "Staging";
        private readonly string _bookingSupportEngineAlias = "Booking";

        public async Task<IEngineServiceRS> RequestAsync(IEngineServiceRQ engineServiceRQ)
        {
            try
            {
                IInternalServiceEngine stagingEngine = InternalServiceEnginesFactory.GetSupportEngine(_statgingSupportEngineAlias);
                ProductStagingInfo productStagingInfo = (ProductStagingInfo)(await stagingEngine.ProcessAsync(engineServiceRQ));
                IInternalServiceEngine bookingEngine = InternalServiceEnginesFactory.GetSupportEngine(_bookingSupportEngineAlias);
                return await bookingEngine.ProcessAsync(productStagingInfo);
            }
            catch(FactoryException factoryException)
            {
                Logger.LogException(factoryException.ToString(), factoryException.StackTrace);
                throw new BookingCoordinatorEngineException()
                {
                    Source = factoryException.Source
                };
            }
            catch(SupportingEngineException supportingEngineException)
            {
                Logger.LogException(supportingEngineException.ToString(), supportingEngineException.StackTrace);
                throw new BookingCoordinatorEngineException()
                {
                    Source = supportingEngineException.Source
                };
            }
            catch (Exception baseException)
            {
                Logger.LogException(baseException.ToString(), baseException.StackTrace);
                throw new BookingCoordinatorEngineException()
                {
                    Source = baseException.Source
                };
            }
        }
    }
}

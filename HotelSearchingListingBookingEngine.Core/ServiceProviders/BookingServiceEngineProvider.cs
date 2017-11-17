using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SystemContracts.ServiceContracts;
using SystemContracts.CustomExceptions;
using HotelSearchingListingBookingEngine.Core.Utilities;
using HotelSearchingListingBookingEngine.Core.ServiceEngines;

namespace HotelSearchingListingBookingEngine.Core.ServiceProviders
{
    public class BookingServiceEngineProvider : IEngineServiceProvider
    {
        public async Task<IEngineServiceRS> GetServiceRSAsync(IEngineServiceRQ serviceRQ)
        {
            try
            {
                return await (new BookingServiceCoordinator()).RequestAsync(serviceRQ);
            }
            catch(BookingCoordinatorEngineException bookingCoordinatorEngineException)
            {
                Logger.LogException(bookingCoordinatorEngineException.ToString(), bookingCoordinatorEngineException.StackTrace);
                throw new ServiceProviderException()
                {
                    Source = bookingCoordinatorEngineException.Source
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

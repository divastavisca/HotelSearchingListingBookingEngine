using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SystemContracts.ServiceContracts;

namespace HotelSearchingListingBookingEngine.Core.ServiceProviders
{
    public class SingleAvailRoomSearchProvider : IEngineServiceProvider
    {
        public async Task<IEngineServiceRS> GetServiceRS(IEngineServiceRQ serviceRQ)
        {
            return await (new SingleAvailRoomSearchProvider()).GetServiceRS(serviceRQ);
        }
    }
}

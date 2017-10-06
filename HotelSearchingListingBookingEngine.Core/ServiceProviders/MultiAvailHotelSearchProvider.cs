using System;
using System.Collections.Generic;
using System.Text;
using SystemContracts.ServiceContracts;
using SystemContracts.Attributes.HotelAttributes;
using SystemContracts.Attributes;
using ExternalServices.HotelSearchEngineConnecter;
using HotelSearchingListingBookingEngine.Core.ServiceEngines;
using System.Threading.Tasks;

namespace HotelSearchingListingBookingEngine.Core.ServiceProviders
{
    public class MultiAvailHotelSearchProvider : IEngineServiceProvider
    {
        public async Task<IEngineServiceRS> GetServiceRS(IEngineServiceRQ serviceRQ)
        { 
             return await (new MultiAvailHotelSearchEngine()).SearchAsync(serviceRQ);
        }
    }
}

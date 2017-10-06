using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SystemContracts.ConsumerContracts;
using SystemContracts.ServiceContracts;
using ExternalServices.HotelSearchEngineConnecter;
using HotelSearchingListingBookingEngine.Core.Parsers;

namespace HotelSearchingListingBookingEngine.Core.ServiceEngines
{
    public class MultiAvailHotelSearchEngine
    {
        public async Task<IEngineServiceRS> SearchAsync(IEngineServiceRQ searchRQ)
        {
            HotelSearchRQ hotelSearchRQ = (new HotelSearchRQParser()).Parse((MultiAvailHotelSearchRQ)searchRQ);
            HotelSearchRS hotelSearchRS = await (new HotelEngineClient()).HotelAvailAsync(hotelSearchRQ);
            return (new MultiAvailHotelSearchRSParser()).Parse(hotelSearchRS);
        }
    }
}

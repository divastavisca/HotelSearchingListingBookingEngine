using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SystemContracts.ConsumerContracts;
using SystemContracts.ServiceContracts;
using HotelSearchingListingBookingEngine.Core.ServiceProviders;

namespace HotelSearchingListingBooking.API.Controllers
{
    [Route("padharojanab")]
    public class ValuesController : Controller
    {
        [Route("value")]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]MultiAvailHotelSearchRQ value)
        {
            IEngineServiceProvider engineServiceProvider = new MultiAvailHotelSearchProvider();
            var response = await engineServiceProvider.GetServiceRS(value);
            return Ok(response);
        }
    }
}

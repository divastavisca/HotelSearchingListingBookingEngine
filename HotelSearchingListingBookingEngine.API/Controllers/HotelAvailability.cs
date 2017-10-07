using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HotelSearchingListingBookingEngine.Core.ServiceProviders;
using SystemContracts.ConsumerContracts;
using SystemContracts.ServiceContracts;

namespace HotelServicesConnecters.API.Controllers
{
    [Route("api/gethotels")]
    public class HotelAvailabilityController : Controller
    {
        public void Get()
        {

        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Postq([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}

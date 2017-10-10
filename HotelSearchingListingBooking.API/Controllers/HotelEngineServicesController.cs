using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Reflection;
using HotelSearchingListingBooking.API.Models;
using HotelSearchingListingBookingEngine.Core;
using SystemContracts.ServiceContracts;

namespace HotelSearchingListingBooking.API.Controllers
{
    [Route("padharojanab")]
    public class HotelEngineServicesController : Controller
    {        
        [HttpPost("value")]
        public async Task<IActionResult> APIServiceRequestAction([FromBody]ServiceRequest serviceRequest)
        {
            try
            {
                var requestedServiceType = ServiceRequestResolver.GetServiceType(serviceRequest);
                if (requestedServiceType == null)
                    throw new NullReferenceException("Cannot resolve service request");
                IEngineServiceRQ engineServiceRequest = (IEngineServiceRQ)JsonConvert.DeserializeObject(serviceRequest.JsonRequest, requestedServiceType);
                if (engineServiceRequest == null)
                    throw new JsonException("Unable to parse json string");
                IEngineServiceProvider engineServiceProvider = APIServiceFactory.GetServiceProvider(requestedServiceType);
                if (engineServiceProvider == null)
                    throw new Exception("Unable to generate service provider");
                IEngineServiceRS engineServiceRS = await engineServiceProvider.GetServiceRS(engineServiceRequest);
                if (engineServiceRS == null)
                    throw new Exception("Unable to generate response");
                return Ok(engineServiceRS);
            }
            catch (JsonException jsonException)
            {
                Logger.LogException(jsonException.ToString(), jsonException.StackTrace);
                return BadRequest();
            }
            catch (NullReferenceException nullRefExcep)
            {
                Logger.LogException(nullRefExcep.ToString(), nullRefExcep.StackTrace);
                return BadRequest();
            }
            catch (Exception baseException)
            {
                Logger.LogException(baseException.ToString(), baseException.StackTrace);
                return BadRequest();
            }
        }
               
    }
}

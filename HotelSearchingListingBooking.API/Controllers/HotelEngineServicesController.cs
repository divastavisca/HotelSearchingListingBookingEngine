 using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using HotelSearchingListingBooking.API.Models;
using HotelSearchingListingBookingEngine.Core;
using SystemContracts.ServiceContracts;
using HotelSearchingListingBookingEngine.Core.CustomExceptions;

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
                if(requestedServiceType==null)
                    throw new InvalidServiceRequestException();
                IEngineServiceRQ engineServiceRequest = (IEngineServiceRQ)JsonConvert.DeserializeObject(serviceRequest.JsonRequest, requestedServiceType);
                if (engineServiceRequest == null)
                    throw new ParseException();
                IEngineServiceProvider engineServiceProvider = APIServiceFactory.GetServiceProvider(requestedServiceType);
                if (engineServiceProvider == null)
                    throw new ServiceProviderGenerationException();
                IEngineServiceRS engineServiceRS = await engineServiceProvider.GetServiceRSAsync(engineServiceRequest);
                if (engineServiceRS == null)
                    throw new ResponseGenerationException();
                return Ok(engineServiceRS);
            }
            catch(ServiceProviderException serviceProviderException)
            {
                Logger.LogException(serviceProviderException.ToString(), serviceProviderException.StackTrace);
                return NotFound();
            }
            catch (ResponseGenerationException responseGenerationException)
            {
                Logger.LogException(responseGenerationException.ToString(), responseGenerationException.StackTrace);
                return NotFound();
            }
            catch (InvalidServiceRequestException invalidServiceRequested)
            {
                Logger.LogException(invalidServiceRequested.ToString(), invalidServiceRequested.StackTrace);
                return BadRequest();
            }
            catch(ParseException parsingException)
            {
                Logger.LogException(parsingException.ToString(), parsingException.StackTrace);
                return BadRequest();
            }
            catch (ServiceProviderGenerationException serviceProviderGenerationException)
            {
                Logger.LogException(serviceProviderGenerationException.ToString(), serviceProviderGenerationException.StackTrace);
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

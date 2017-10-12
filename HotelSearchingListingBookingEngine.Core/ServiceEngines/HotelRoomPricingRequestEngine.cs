using System;
using System.Collections.Generic;
using System.Text;
using SystemContracts.ConsumerContracts;
using SystemContracts.ServiceContracts;
using ExternalServices.HotelSearchEngine;
using HotelSearchingListingBookingEngine.Core.Parsers;
using HotelSearchingListingBookingEngine.Core;
using HotelSearchingListingBookingEngine.Core.CustomExceptions;
using HotelSearchingListingBookingEngine.Core.Caches;
using System.Threading.Tasks;

namespace HotelSearchingListingBookingEngine.Core.ServiceEngines
{
    public class HotelRoomPricingRequestEngine : IRequestServiceEngine
    {
        public async Task<IEngineServiceRS> RequestAsync(IEngineServiceRQ engineServiceRQ)
        {
            try
            {
                HotelRoomPriceRQ hotelRoomPriceRQ = (new HotelRoomPriceRQParser()).Parse((RoomPricingRQ)engineServiceRQ);
                HotelRoomPriceRS hotelRoomPriceRS = await (new HotelEngineClient()).HotelRoomPriceAsync(hotelRoomPriceRQ);
                if (hotelRoomPriceRS.Itinerary == null)
                    throw new NoResultsFoundException()
                    {
                        Source = hotelRoomPriceRS.Itinerary.GetType().Name
                    };
                return (new RoomPricingRSParser()).Parse(hotelRoomPriceRS);
            }

            catch(ServiceRequestParserException serviceRequestParserException)
            {
                Logger.LogException(serviceRequestParserException.ToString(), serviceRequestParserException.StackTrace);
                throw new PricingRequestEngineException()
                {
                    Source = serviceRequestParserException.Source
                };
            }
            catch (ServiceResponseParserException serviceResponseParserException)
            {
                Logger.LogException(serviceResponseParserException.ToString(), serviceResponseParserException.StackTrace);
                throw new PricingRequestEngineException()
                {
                    Source = serviceResponseParserException.Source
                };
            }
            catch (NoResultsFoundException noResultsFoundException)
            {
                Logger.LogException(noResultsFoundException.ToString(), noResultsFoundException.StackTrace);
                throw new PricingRequestEngineException()
                {
                    Source = noResultsFoundException.Source
                };
            }
            catch(Exception baseException)
            {
                Logger.LogException(baseException.ToString(), baseException.StackTrace);
                throw new PricingRequestEngineException()
                {
                    Source = baseException.Source
                };
            }
        }
    }
}

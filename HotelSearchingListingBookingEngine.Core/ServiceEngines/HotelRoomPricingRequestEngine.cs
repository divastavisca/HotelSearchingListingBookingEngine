using System;
using System.Collections.Generic;
using System.Text;
using SystemContracts.ConsumerContracts;
using SystemContracts.ServiceContracts;
using ExternalServices.PricingPolicyEngine;
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
                TripProductPriceRQ hotelRoomPriceRQ = (new TripProductPriceRQParser()).Parse((RoomPricingRQ)engineServiceRQ);
                TripProductPriceRS hotelRoomPriceRS = await (new TripsEngineClient()).PriceTripProductAsync(hotelRoomPriceRQ);
                if (hotelRoomPriceRS.TripProduct == null)
                    throw new NoResultsFoundException()
                    {
                        Source = hotelRoomPriceRS.TripProduct.GetType().Name
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

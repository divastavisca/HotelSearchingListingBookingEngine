using System;
using System.Collections.Generic;
using System.Text;
using SystemContracts.ConsumerContracts;
using SystemContracts.ServiceContracts;
using HotelSearchingListingBooking.ExternalServices.PricingPolicyEngine;
using HotelSearchingListingBooking.Translators;
using HotelSearchingListingBookingEngine.Core;
using SystemContracts.CustomExceptions;
using HotelSearchingListingBooking.Caches;
using System.Threading.Tasks;
using HotelSearchingListingBookingEngine.Core.Utilities;

namespace HotelSearchingListingBookingEngine.Core.ServiceEngines
{
    public class HotelRoomPricingRequestEngine : IRequestServiceEngine
    {
        public async Task<IEngineServiceRS> RequestAsync(IEngineServiceRQ engineServiceRQ)
        {
            try
            {
                TripProductPriceRQ hotelRoomPriceRQ = (new TripProductPriceRQTranslator()).Translate((RoomPricingRQ)engineServiceRQ);
                TripProductPriceRS hotelRoomPriceRS = await (new TripsEngineClient()).PriceTripProductAsync(hotelRoomPriceRQ);
                if (hotelRoomPriceRS.TripProduct == null)
                    throw new NoResultsFoundException()
                    {
                        Source = hotelRoomPriceRS.TripProduct.GetType().Name
                    };
                return (new RoomPricingRSTranslator()).Translate(hotelRoomPriceRS);
            }
            catch (ServiceRequestTranslatorException serviceRequestParserException)
            {
                Logger.LogException(serviceRequestParserException.ToString(), serviceRequestParserException.StackTrace);
                throw new PricingRequestEngineException()
                {
                    Source = serviceRequestParserException.Source
                };
            }
            catch (ServiceResponseTranslatorException serviceResponseParserException)
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

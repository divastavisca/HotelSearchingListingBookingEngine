using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SystemContracts.ServiceContracts;
using ExternalServices.PricingPolicyEngine;
using HotelSearchingListingBookingEngine.Core.InternalEngineHandshakes;
using HotelSearchingListingBookingEngine.Core.InternalServiceEngines;
using HotelSearchingListingBookingEngine.Core.CustomExceptions;
using HotelSearchingListingBookingEngine.Core.Caches;
using SystemContracts.ConsumerContracts;
using SystemContracts.InternalContracts;

namespace HotelSearchingListingBookingEngine.Core.ServiceEngines
{
    public class BookingServiceCoordinator : IRequestServiceEngine
    {
        private readonly string _statgingSupportEngineAlias = "Staging";
        private readonly string _bookingSupportEngineAlias = "Booking";

        public async Task<IEngineServiceRS> RequestAsync(IEngineServiceRQ engineServiceRQ)
        {
            try
            {
                IInternalServiceEngine stagingEngine = InternalServiceEnginesFactory.GetSupportEngine(_statgingSupportEngineAlias);
                ProductStagingInfo productStagingInfo = (ProductStagingInfo)(await stagingEngine.ProcessAsync(engineServiceRQ));
                if (productStagingInfo.TripFolderId != null)
                {
                    storeSummary(productStagingInfo.TripFolderId.ToString(),((HotelProductBookRQ)engineServiceRQ).Guests,productStagingInfo.Product.HotelItinerary.StayPeriod,productStagingInfo.Product.HotelItinerary.Rooms[0],productStagingInfo.Product.HotelItinerary.HotelProperty.Name);
                    IInternalServiceEngine bookingEngine = InternalServiceEnginesFactory.GetSupportEngine(_bookingSupportEngineAlias);
                    return await bookingEngine.ProcessAsync(productStagingInfo);
                }
                else throw new InvalidObjectRequestException()
                {
                    Source = typeof(TripFolder).Name
                };
            }
            catch(FactoryException factoryException)
            {
                Logger.LogException(factoryException.ToString(), factoryException.StackTrace);
                throw new BookingCoordinatorEngineException()
                {
                    Source = factoryException.Source
                };
            }
            catch (InvalidObjectRequestException invalidObjectRequestException)
            {
                Logger.LogException(invalidObjectRequestException.ToString(), invalidObjectRequestException.StackTrace);
                throw new BookingCoordinatorEngineException()
                {
                    Source = invalidObjectRequestException.Source
                };
            }
            catch (SupportingEngineException supportingEngineException)
            {
                Logger.LogException(supportingEngineException.ToString(), supportingEngineException.StackTrace);
                throw new BookingCoordinatorEngineException()
                {
                    Source = supportingEngineException.Source
                };
            }
            catch (Exception baseException)
            {
                Logger.LogException(baseException.ToString(), baseException.StackTrace);
                throw new BookingCoordinatorEngineException()
                {
                    Source = baseException.Source
                };
            }
            finally
            {
                if (BookingSummaryCache.IsPresent(((HotelProductBookRQ)engineServiceRQ).CallerSessionId))
                {
                    BookingSummaryCache.Remove(((HotelProductBookRQ)engineServiceRQ).CallerSessionId);
                }
            }
        }

        private void storeSummary(string tripFolderId,SystemContracts.Attributes.Guest[] guests,DateTimeSpan timePeriod,Room selectedRoom,string hotelName)
        {
            if(BookingSummaryCache.IsPresent(tripFolderId))
            {
                BookingSummaryCache.Remove(tripFolderId);
            }
            BookingSummaryCache.AddToCache
            (
                tripFolderId,
                new BookingSummary()
                {
                    HotelName = hotelName,
                    Guests = guests,
                    CheckInDate = timePeriod.Start,
                    CheckOutDate = timePeriod.End,
                    RoomName = selectedRoom.RoomName, 
                    TotalAmount = selectedRoom.DisplayRoomRate.TotalFare.Amount,
                    Currency = selectedRoom.DisplayRoomRate.TotalFare.Currency
                }
            );
        }
    }
}

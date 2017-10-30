using System;
using System.Collections.Generic;
using System.Text;
using SystemContracts.ConsumerContracts;
using HotelSearchingListingBooking.ExternalServices.PricingPolicyEngine;
using SystemContracts.CustomExceptions;
using HotelSearchingListingBooking.Caches;
using SystemContracts.InternalContracts;
using HotelSearchingListingBooking.Translators.Utilities;

namespace HotelSearchingListingBooking.Translators
{
    public class ProductBookingRSTranslator
    {
        public ProductBookingRS Translate(CompleteBookingRS completeBookingRS)
        {
            try
            {
                ProductBookingRS _productBookingRS = new ProductBookingRS();
                _productBookingRS.IsCompleted = checkCompletion(completeBookingRS);
                if (_productBookingRS.IsCompleted)
                {
                    if (completeBookingRS.TripFolder != null)
                    {
                        _productBookingRS.ConfirmationId = completeBookingRS.TripFolder.ConfirmationNumber;
                        _productBookingRS.TransactionId = completeBookingRS.TripFolder.Products[0].PassengerSegments[0].SupplierConfirmationNumber;
                    }
                    _productBookingRS.BookingSummary = BookingSummaryCache.GetSummary(completeBookingRS.TripFolder.Id.ToString());
                    if (_productBookingRS.BookingSummary == null)
                        throw new InvalidObjectRequestException()
                        {
                            Source = typeof(BookingSummary).Name
                        };
                    //CLEAR CACHES
                    BookingSummaryCache.Remove(completeBookingRS.TripFolder.Id.ToString());
                    TripProductCache.Remove(completeBookingRS.TripFolder.Id.ToString());
                    PricingRequestCache.Remove(completeBookingRS.TripFolder.Id.ToString());
                }
                else throw new BookingFailedException()
                {
                    Source = typeof(CompleteBookingRS).Name
                };
                return _productBookingRS;
            }
            catch (InvalidObjectRequestException invalidObjectRequestException)
            {
                Logger.LogException(invalidObjectRequestException.ToString(), invalidObjectRequestException.StackTrace);
                throw new ServiceResponseTranslatorException()
                {
                    Source = invalidObjectRequestException.Source
                };
            }
            catch (BookingFailedException bookingFailedException)
            {
                Logger.LogException(bookingFailedException.ToString(), bookingFailedException.StackTrace);
                throw new ServiceResponseTranslatorException()
                {
                    Source = bookingFailedException.Source
                };
            }
            catch (NullReferenceException nullReferenceException)
            {
                Logger.LogException(nullReferenceException.ToString(), nullReferenceException.StackTrace);
                throw new ServiceResponseTranslatorException()
                {
                    Source = nullReferenceException.Source
                };
            }
            catch(Exception baseException)
            {
                Logger.LogException(baseException.ToString(), baseException.StackTrace);
                throw new ServiceResponseTranslatorException()
                {
                    Source = baseException.Source
                };
            }
        }

        private bool checkCompletion(CompleteBookingRS completeBookingRS)
        {
            return completeBookingRS.ServiceStatus.Status == ServiceStatusType.Success;
        }
    }
}

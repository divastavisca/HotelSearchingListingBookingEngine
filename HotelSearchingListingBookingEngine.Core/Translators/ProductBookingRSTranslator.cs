﻿using System;
using System.Collections.Generic;
using System.Text;
using SystemContracts.ConsumerContracts;
using ExternalServices.PricingPolicyEngine;
using HotelSearchingListingBookingEngine.Core.CustomExceptions;
using HotelSearchingListingBookingEngine.Core.Caches;
using SystemContracts.InternalContracts;

namespace HotelSearchingListingBookingEngine.Core.Translators
{
    public class ProductBookingRSTranslator
    {
        public ProductBookingRS Translate(CompleteBookingRS completeBookingRS)
        {
            try
            {
                ProductBookingRS productBookingRS = new ProductBookingRS();
                productBookingRS.IsCompleted = checkCompletion(completeBookingRS);
                if (productBookingRS.IsCompleted)
                {
                    if (completeBookingRS.TripFolder != null)
                        productBookingRS.TransactionId = completeBookingRS.TripFolder.ConfirmationNumber;
                    productBookingRS.BookingSummary = BookingSummaryCache.GetSummary(completeBookingRS.TripFolder.Id.ToString());
                    if (productBookingRS.BookingSummary == null)
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
                return productBookingRS;
            }
            catch (InvalidObjectRequestException invalidObjectRequestException)
            {
                Logger.LogException(invalidObjectRequestException.ToString(), invalidObjectRequestException.StackTrace);
                throw new ServiceResponseParserException()
                {
                    Source = invalidObjectRequestException.Source
                };
            }
            catch (BookingFailedException bookingFailedException)
            {
                Logger.LogException(bookingFailedException.ToString(), bookingFailedException.StackTrace);
                throw new ServiceResponseParserException()
                {
                    Source = bookingFailedException.Source
                };
            }
            catch (NullReferenceException nullReferenceException)
            {
                Logger.LogException(nullReferenceException.ToString(), nullReferenceException.StackTrace);
                throw new ServiceResponseParserException()
                {
                    Source = nullReferenceException.Source
                };
            }
            catch(Exception baseException)
            {
                Logger.LogException(baseException.ToString(), baseException.StackTrace);
                throw new ServiceResponseParserException()
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

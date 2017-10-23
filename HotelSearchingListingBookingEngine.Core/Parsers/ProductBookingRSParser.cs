using System;
using System.Collections.Generic;
using System.Text;
using SystemContracts.ConsumerContracts;
using ExternalServices.PricingPolicyEngine;
using HotelSearchingListingBookingEngine.Core.CustomExceptions;

namespace HotelSearchingListingBookingEngine.Core.Parsers
{
    public class ProductBookingRSParser
    {
        public ProductBookingRS Parse(CompleteBookingRS completeBookingRS)
        {
            try
            {
                ProductBookingRS productBookingRS = new ProductBookingRS();
                productBookingRS.IsCompleted = checkCompletion(completeBookingRS);
                if(productBookingRS.IsCompleted)
                {
                    if (completeBookingRS.TripFolder != null)
                        productBookingRS.TransactionId = completeBookingRS.TripFolder.ConfirmationNumber;
                    else productBookingRS.TransactionId = Guid.NewGuid().ToString();
                }
                return productBookingRS;
            }
            catch(NullReferenceException nullReferenceException)
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
            return completeBookingRS.ErrorInfoList == null;
        }
    }
}

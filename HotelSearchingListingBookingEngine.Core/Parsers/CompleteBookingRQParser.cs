using System;
using System.Collections.Generic;
using System.Text;
using ExternalServices.PricingPolicyEngine;
using HotelSearchingListingBookingEngine.Core.InternalEngineHandshakes;
using HotelSearchingListingBookingEngine.Core.CustomExceptions;

namespace HotelSearchingListingBookingEngine.Core.Parsers
{
    public class CompleteBookingRQParser
    {
        public CompleteBookingRQ Parse(ProductStagingInfo productStagingInfo)
        {
            try
            {
                throw new NotImplementedException();
            }
            catch(Exception baseException)
            {
                Logger.LogException(baseException.ToString(), baseException.StackTrace);
                throw new ServiceRequestParserException()
                {
                    Source = baseException.Source
                };
            }
        }
    }
}

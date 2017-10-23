using System;
using System.Collections.Generic;
using System.Text;
using HotelSearchingListingBookingEngine.Core.InternalEngineHandshakes;
using ExternalServices.PricingPolicyEngine;
using HotelSearchingListingBookingEngine.Core.CustomExceptions;

namespace HotelSearchingListingBookingEngine.Core.Parsers
{
    public class StagingRSParser
    {
        public ProductStagingInfo Parse(TripFolderBookRS tripFolderBookRS)
        {
            try
            {
                throw new NotImplementedException();
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
    }
}

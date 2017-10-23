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
                throw new NotImplementedException();
            }
            catch(Exception baseException)
            {
                throw new ServiceResponseParserException()
                {
                    Source = baseException.Source
                };
            }
        }
    }
}

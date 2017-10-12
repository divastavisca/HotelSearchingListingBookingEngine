﻿using System;
using System.Collections.Generic;
using System.Text;

namespace HotelSearchingListingBookingEngine.Core.CustomExceptions
{
    public class PricingRequestEngineException : Exception
    {
        public override string ToString()
        {
            return (new StringBuilder()).Append("Error in processing Price Request at ")
                                        .Append(Source)
                                        .ToString();
        }
    }
}

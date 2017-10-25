﻿using System;
using System.Collections.Generic;
using System.Text;

namespace HotelSearchingListingBookingEngine.Core.CustomExceptions
{
    public class SupportingEngineException : Exception
    {
        public override string ToString()
        {
            return (new StringBuilder()).Append("Error in providing support for ")
                                        .Append(Source)
                                        .ToString();
        }
    }
}

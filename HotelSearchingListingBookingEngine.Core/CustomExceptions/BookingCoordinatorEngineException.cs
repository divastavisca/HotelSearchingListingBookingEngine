﻿using System;
using System.Collections.Generic;
using System.Text;

namespace HotelSearchingListingBookingEngine.Core.CustomExceptions
{
    public class BookingCoordinatorEngineException : Exception
    {
        public override string ToString()
        {
            return (new StringBuilder()).Append("Error in booking at ")
                                        .Append(Source)
                                        .ToString();
        }
    }
}

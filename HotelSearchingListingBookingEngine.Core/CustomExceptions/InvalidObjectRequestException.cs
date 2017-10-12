using System;
using System.Collections.Generic;
using System.Text;

namespace HotelSearchingListingBookingEngine.Core.CustomExceptions
{
    public class InvalidObjectRequestException : Exception
    {
        public override string ToString()
        {
            return (new StringBuilder()).Append("Error in finding object of type ")
                                        .Append(Source)
                                        .ToString();
        }
    }
}

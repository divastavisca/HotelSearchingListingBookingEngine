using System;
using System.Collections.Generic;
using System.Text;

namespace HotelSearchingListingBookingEngine.Core.CustomExceptions
{
    public class ResponseParserException : Exception
    {
        public override string ToString()
        {
            return (new StringBuilder()).Append("Error in parsing response at ")
                                        .Append(Source)
                                        .ToString();
        }
    }
}

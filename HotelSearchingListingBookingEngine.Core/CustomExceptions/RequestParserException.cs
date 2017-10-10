using System;
using System.Collections.Generic;
using System.Text;

namespace HotelSearchingListingBookingEngine.Core.CustomExceptions
{
    public class ServiceRequestParserException : Exception
    {
        public override string ToString()
        {
            return (new StringBuilder()).Append("Error in parsing Request")
                                        .AppendLine()
                                        .Append("Exception at ")
                                        .Append(Source)
                                        .ToString();
        }
    }
}

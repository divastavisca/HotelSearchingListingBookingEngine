using System;
using System.Collections.Generic;
using System.Text;

namespace HotelSearchingListingBookingEngine.Core.CustomExceptions
{
    public class ServiceResponseTranslatorException : Exception
    {
        public override string ToString()
        {
            return (new StringBuilder()).Append("Error in translating response at ")
                                        .Append(Source)
                                        .ToString();
        }
    }
}

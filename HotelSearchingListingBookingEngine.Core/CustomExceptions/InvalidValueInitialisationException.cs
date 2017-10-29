using System;
using System.Collections.Generic;
using System.Text;

namespace HotelSearchingListingBookingEngine.Core.CustomExceptions
{
    public class InvalidValueInitializationException : Exception
    {
        public override string ToString()
        {
            return (new StringBuilder()).Append("Invalid value inizialized in ")
                                        .Append(Source)
                                        .ToString();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace HotelSearchingListingBookingEngine.Core.CustomExceptions
{
    public class CacheOverflowException : Exception
    {
        public override string ToString()
        {
            return (new StringBuilder()).Append("Error in cleaning caches")
                                        .ToString();
        }
    }
}

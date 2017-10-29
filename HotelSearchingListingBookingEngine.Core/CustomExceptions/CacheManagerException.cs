using System;
using System.Collections.Generic;
using System.Text;

namespace HotelSearchingListingBookingEngine.Core.CustomExceptions
{
    public class CacheManagerException : Exception
    {
        public override string ToString()
        {
            return (new StringBuilder()).Append("Error in ")
                                        .Append(Source)
                                        .ToString();
        }

    }
}

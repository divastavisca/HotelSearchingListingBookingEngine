using System;
using System.Collections.Generic;
using System.Text;

namespace HotelSearchingListingBookingEngine.Core.CustomExceptions
{
    public class ObjectFetchException : Exception
    {
        public override string ToString()
        {
            return (new StringBuilder()).Append("Unable to fetch object ")
                                        .Append(Source)
                                        .ToString();
        }
    }
}

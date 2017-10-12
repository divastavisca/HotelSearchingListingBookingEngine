using System;
using System.Collections.Generic;
using System.Text;

namespace HotelSearchingListingBookingEngine.Core.CustomExceptions
{
    public class FilePathResolverError : Exception
    {
        public override string ToString()
        {
            return (new StringBuilder()).Append("Unable to resolve file path ")
                                        .Append(Source)
                                        .ToString();
        }
    }
}

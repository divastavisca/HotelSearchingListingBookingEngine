using System;
using System.Collections.Generic;
using System.Text;

namespace HotelSearchingListingBookingEngine.Core.CustomExceptions
{
    public class BookingFailedException : Exception
    {
        public override string ToString()
        {
            return (new StringBuilder()).Append("Booking failed ")
                                        .Append(Source)
                                        .ToString();
        }
    }
}

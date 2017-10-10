using System;
using System.Collections.Generic;
using System.Text;

namespace HotelSearchingListingBookingEngine.Core.CustomExceptions
{
    public class ParseException : Exception
    {
        public override string ToString()
        {
            return "Error in parsing";
        }
    }
}

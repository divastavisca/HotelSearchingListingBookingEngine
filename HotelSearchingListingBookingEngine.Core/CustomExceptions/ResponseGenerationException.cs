using System;
using System.Collections.Generic;
using System.Text;

namespace HotelSearchingListingBookingEngine.Core.CustomExceptions
{
    public class ResponseGenerationException : Exception
    {
        public override string ToString()
        {
            return "Error in generating response";
        }
    }
}

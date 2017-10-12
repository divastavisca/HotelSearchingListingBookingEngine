using System;
using System.Collections.Generic;
using System.Text;

namespace HotelSearchingListingBookingEngine.Core.CustomExceptions
{
    public class ObjectInitializationException : Exception
    {
        

        public override string ToString()
        {
            return (new StringBuilder()).Append("Error in initilizing ")
                                        .Append(Source)
                                        .ToString();
        }
    }
}

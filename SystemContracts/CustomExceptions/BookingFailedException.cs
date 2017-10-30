using System;
using System.Collections.Generic;
using System.Text;

namespace SystemContracts.CustomExceptions
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

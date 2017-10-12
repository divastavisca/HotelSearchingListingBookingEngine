using System;
using System.Collections.Generic;
using System.Text;

namespace HotelSearchingListingBookingEngine.Core.CustomExceptions
{
    public class NoResultsFoundException : Exception
    {
        public override string ToString()
        {
            return "No Results were discovered or some exception occured at supplier end";
        }
    }
}

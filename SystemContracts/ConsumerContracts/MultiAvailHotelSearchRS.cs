using System;
using System.Collections.Generic;
using System.Text;
using SystemContracts.Attributes.HotelAttributes;

namespace SystemContracts.ConsumerContracts
{
    public class MultiAvailHotelSearchRS
    {
        public string CallerSessionId { get; }

        public int ResultsCount { get; }

        public HotelItinerary[] Itineraries { get; }

        public MultiAvailHotelSearchRS(string callerSessionId,int resultsCount,HotelItinerary[] itineraries)
        {
            CallerSessionId = callerSessionId;
            ResultsCount = resultsCount;
            Itineraries = itineraries;
        }
    }
}

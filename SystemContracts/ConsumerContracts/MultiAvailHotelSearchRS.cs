using System;
using System.Collections.Generic;
using System.Text;
using SystemContracts.Attributes.HotelAttributes;

namespace SystemContracts.ConsumerContracts
{
    public class MultiAvailHotelSearchRS
    {
        public int ResultsCount { get; }

        public HotelItinerary[] Itineraries { get; }

        public MultiAvailHotelSearchRS(int resultsCount,HotelItinerary[] itineraries)
        {
            ResultsCount = resultsCount;
            Itineraries = itineraries;
        }
    }
}

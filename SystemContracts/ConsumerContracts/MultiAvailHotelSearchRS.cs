using System;
using System.Collections.Generic;
using System.Text;
using SystemContracts.Attributes.HotelAttributes;
using SystemContracts.ServiceContracts;

namespace SystemContracts.ConsumerContracts
{
    public class MultiAvailHotelSearchRS : IEngineServiceRS
    {
        public string CallerSessionId { get; }

        public int ResultsCount { get; }

        public Itinerary[] Itineraries { get; }

        public MultiAvailHotelSearchRS(string callerSessionId,int resultsCount,Itinerary[] itineraries)
        {
            CallerSessionId = callerSessionId;
            ResultsCount = resultsCount;
            Itineraries = itineraries;
        }
    }
}

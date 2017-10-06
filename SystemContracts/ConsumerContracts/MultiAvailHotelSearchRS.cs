using System;
using System.Collections.Generic;
using System.Text;
using SystemContracts.Attributes.HotelAttributes;
using SystemContracts.ServiceContracts;

namespace SystemContracts.ConsumerContracts
{
    public class MultiAvailHotelSearchRS : IEngineServiceRS
    {
        public string CallerSessionId { get; set; }

        public int ResultsCount { get; set; }

        public Itinerary[] Itineraries { get; set; }
    }
}

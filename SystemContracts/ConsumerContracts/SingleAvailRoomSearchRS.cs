using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ExternalServices.HotelSearchEngine;
using SystemContracts.ServiceContracts;
using SystemContracts.Attributes.HotelAttributes;

namespace SystemContracts.ConsumerContracts
{
    public class SingleAvailRoomSearchRS : IEngineServiceRS
    {
        public string CallerSessionId { get; set; }

        public Itinerary Itinerary { get; set; }
    }
}

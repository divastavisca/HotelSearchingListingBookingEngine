using System;
using System.Collections.Generic;
using System.Text;
using SystemContracts.ServiceContracts;
using SystemContracts.Attributes;

namespace SystemContracts.ConsumerContracts
{
    public class SingleAvailRoomSearchRQ : IEngineServiceRQ
    {
        public string CallerSessionId { get; set; }

        public string ItineraryId { get; set; }

        public GeoCoordinates GeoCode { get; set; }
    }
}

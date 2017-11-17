using System;
using System.Collections.Generic;
using System.Text;
using SystemContracts.Attributes.HotelAttributes;
using SystemContracts.ServiceContracts;

namespace SystemContracts.ConsumerContracts
{
    public class RoomPricingRQ : IEngineServiceRQ
    {
        public string CallerSessionId { get; set; }

        public string RoomId { get; set; }
    }
}

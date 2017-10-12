using System;
using System.Collections.Generic;
using System.Text;
using SystemContracts.Attributes.HotelAttributes;

namespace SystemContracts.ConsumerContracts
{
    public class RoomPricingRQ
    {
        public string CallerSessionId { get; set; }

        public string RoomId { get; set; }
    }
}

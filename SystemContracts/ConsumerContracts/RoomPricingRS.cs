using System;
using System.Collections.Generic;
using System.Text;

namespace SystemContracts.ConsumerContracts
{
    public class RoomPricingRS
    {
        public string CallerSessionId { get; set; }

        public bool IsUpdated { get; set; }

        public string Currency { get; set; }

        public decimal RoomPrice { get; set; }
    }
}

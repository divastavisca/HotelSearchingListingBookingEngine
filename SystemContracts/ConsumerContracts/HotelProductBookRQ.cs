using System;
using System.Collections.Generic;
using System.Text;
using SystemContracts.Attributes;
using SystemContracts.ServiceContracts;

namespace SystemContracts.ConsumerContracts
{
    public class HotelProductBookRQ : IEngineServiceRQ
    {
        public string CallerSessionId { get; set; }

        public Guest[] Guests { get; set; }

        public PaymentDetails PaymentDetails { get; set; }
    }
}

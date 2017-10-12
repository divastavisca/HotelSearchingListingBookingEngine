using System;
using System.Collections.Generic;
using System.Text;
using SystemContracts.Attributes;

namespace SystemContracts.ConsumerContracts
{
    public class HotelProductBookRQ
    {
        public string CallerSessionId { get; set; }

        public Guest[] Guests { get; set; }

        public PaymentDetails PaymentDetails { get; set; }
    }
}

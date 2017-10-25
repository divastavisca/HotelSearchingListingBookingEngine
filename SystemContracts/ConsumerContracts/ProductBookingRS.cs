using System;
using System.Collections.Generic;
using System.Text;
using SystemContracts.ServiceContracts;

namespace SystemContracts.ConsumerContracts
{
    public class ProductBookingRS : IEngineServiceRS
    {
        public string TransactionId { get; set; }

        public bool IsCompleted { get; set; }
    }
}

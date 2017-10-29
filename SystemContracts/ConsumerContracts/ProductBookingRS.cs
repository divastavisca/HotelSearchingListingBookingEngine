using System;
using System.Collections.Generic;
using System.Text;
using SystemContracts.ServiceContracts;
using SystemContracts.InternalContracts;

namespace SystemContracts.ConsumerContracts
{
    public class ProductBookingRS : IEngineServiceRS
    {
        public string TransactionId { get; set; }

        public string ConfirmationId { get; set; }

        public bool IsCompleted { get; set; }

        public BookingSummary BookingSummary { get; set; }
    }
}

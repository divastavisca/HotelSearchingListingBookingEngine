using System;
using System.Collections.Generic;
using System.Text;

namespace SystemContracts.ConsumerContracts
{
    public class ServiceRequest
    {
        public string ServiceName { get; set; }

        public string JsonRequest { get; set; }
    }
}

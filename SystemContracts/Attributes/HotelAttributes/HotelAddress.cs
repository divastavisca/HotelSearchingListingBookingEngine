using System;
using System.Collections.Generic;
using System.Text;

namespace SystemContracts.Attributes.HotelAttributes
{
    public class HotelAddress
    {
        public string AddressLine1 { get; set; }

        public string AddressLine2 { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string Country { get; set; }

        public string ZipCode { get; set; }
    }
}

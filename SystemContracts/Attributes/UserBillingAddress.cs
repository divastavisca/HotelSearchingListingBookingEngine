using System;
using System.Collections.Generic;
using System.Text;

namespace SystemContracts.Attributes
{
    public class UserBillingAddress
    {
        public string AddressLine1 { get; set; }

        public string AddressLine2 { get; set; }

        public string AddressContext { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string Country { get; set; }

        public string ZipCode { get; set; }

        public string PhoneNumber { get; set; }
    }
}

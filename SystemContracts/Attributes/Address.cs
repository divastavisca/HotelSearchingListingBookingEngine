using System;
using System.Collections.Generic;
using System.Text;

namespace SystemContracts.Attributes
{
    public class Address
    {
        public string AddressLine1 { get; }

        public string AddressLine2 { get; }

        public string City { get; }

        public string State { get; }

        public string Country { get; }

        public string ZipCode { get; }

        public Address(string addressLine1,string addressLine2,string city,string state,string country,string zipcode)
        {
            AddressLine1 = addressLine1;
            AddressLine2 = addressLine2;
            City = city;
            State = state;
            Country = country;
            ZipCode = zipcode;
        }
    }
}

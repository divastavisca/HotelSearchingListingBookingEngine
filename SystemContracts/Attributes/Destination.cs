using System;
using System.Collections.Generic;
using System.Text;

namespace SystemContracts.Attributes
{
    public class Destination
    {
        public string Name { get; set; }

        public string Type { get; set; }

        public GeoCoordinates GeoCode { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace SystemContracts.Attributes
{
    public class Destination
    {
        public string Name { get; }

        public GeoCoordinates GeoCode { get; }

        public Destination(string name,GeoCoordinates geoCode)
        {
            Name = name;
            GeoCode = geoCode;
        }
    }
}

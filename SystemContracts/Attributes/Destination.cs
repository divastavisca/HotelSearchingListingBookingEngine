using System;
using System.Collections.Generic;
using System.Text;

namespace SystemContracts.Attributes
{
    public class Destination
    {
        public string Name { get; }

        public string Type { get; }

        public GeoCoordinates GeoCode { get; }

        public Destination(string name,string type,GeoCoordinates geoCode)
        {
            Name = name;
            GeoCode = geoCode;
            Type = type;
        }
    }
}

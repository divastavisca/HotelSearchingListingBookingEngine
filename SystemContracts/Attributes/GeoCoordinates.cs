using System;
using System.Collections.Generic;
using System.Text;

namespace SystemContracts.Attributes
{
    public class GeoCoordinates
    {
        public float Latitude { get; }

        public float Longitude { get; }

        public GeoCoordinates(float latitude,float longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }
    }
}

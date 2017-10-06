using System;
using System.Collections.Generic;
using System.Text;

namespace SystemContracts.Attributes.HotelAttributes
{
    public class Itinerary
    {
        public string Name { get; set; }

        public HotelAddress Address { get; set; }

        public GeoCoordinates GeoCode { get; set; }

        public Amenity[] Amenities { get; set; }

        public float StarRating { get; set; }

        public string Currency { get; set; }

        public float MinimumPrice { get; set; }
    }
}

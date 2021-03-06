﻿using System;
using System.Collections.Generic;
using System.Text;

namespace SystemContracts.Attributes.HotelAttributes
{
    public class ItinerarySummary
    {
        public string ItineraryId { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public GeoCoordinates GeoCode { get; set; }

        public string[] Amenities { get; set; }

        public string[] ImageUrl { get; set; }

        public float StarRating { get; set; }

        public string Currency { get; set; }

        public decimal MinimumPrice { get; set; }
    }
}

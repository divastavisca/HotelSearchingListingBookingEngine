﻿using System;
using System.Collections.Generic;
using System.Text;

namespace SystemContracts.Attributes.HotelAttributes
{
    public class HotelItinerary
    {
        public string Name { get; }

        public Address Address { get; }

        public GeoCoordinates GeoCode { get; }

        public Amenity[] Amenities { get; }

        public float StarRating { get; }

        public string Currency { get; }

        public float MinimumPrice { get; }

        public HotelItinerary(string name,Address address,GeoCoordinates geoCode,Amenity[] amenities,float starRating,string currency,float minimumPrice)
        {
            Name = name;
            Address = address;
            GeoCode = geoCode;
            Amenities = amenities;
            StarRating = starRating;
            Currency = currency;
            MinimumPrice = minimumPrice;
        }
    }
}

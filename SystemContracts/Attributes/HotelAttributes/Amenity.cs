using System;
using System.Collections.Generic;
using System.Text;
using SystemContracts.Attributes;

namespace SystemContracts.Attributes.HotelAttributes
{
    public class Amenity
    {
        public string Name { get; }

        public Media Media { get; }

        public string Description { get; }

        public Amenity(string name,Media media,string description)
        {
            Name = name;
            Media = media;
            Description = description;
        }
    }
}

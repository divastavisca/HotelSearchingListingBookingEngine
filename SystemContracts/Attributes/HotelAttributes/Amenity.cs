using System;
using System.Collections.Generic;
using System.Text;
using SystemContracts.Attributes;

namespace SystemContracts.Attributes.HotelAttributes
{
    public class Amenity
    {
        public string Name { get; set; }

        public Media Media { get; set; }

        public string Description { get; set; }
        
    }
}

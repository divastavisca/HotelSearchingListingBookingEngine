using System;
using System.Collections.Generic;
using System.Text;
using SystemContracts.Attributes;

namespace SystemContracts.Attributes.HotelAttributes
{
    public class HotelAmenity
    {
        public string Name { get; set; }

        public HotelMedia Media { get; set; }

        public string Description { get; set; }
        
    }
}

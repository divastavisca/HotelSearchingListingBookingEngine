using System;
using System.Collections.Generic;
using System.Text;

namespace SystemContracts.Attributes.HotelAttributes
{
    public class Room
    {
        public string HotelId { get; set; }

        public string Description { get; set; }

        public List<string> OccupancyTypes { get; set; }

        public List<int> OccupancyCount { get; set; }
    }
}

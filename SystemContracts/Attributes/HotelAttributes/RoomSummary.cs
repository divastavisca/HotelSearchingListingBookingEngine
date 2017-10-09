using System;
using System.Collections.Generic;
using System.Text;

namespace SystemContracts.Attributes.HotelAttributes
{
    public class RoomSummary
    {
        public string HotelId { get; set; }

        public string RoomId { get; set; }

        public string Description { get; set; }

        public int MaxOccupancy { get; set; }

        public bool IsPrepaid { get; set; }

        public string Currency { get; set; }

        public decimal TotalFare { get; set; }
    }
}

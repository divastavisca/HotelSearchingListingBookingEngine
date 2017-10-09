using System;
using System.Collections.Generic;
using System.Text;

namespace SystemContracts.Attributes.HotelAttributes
{
    public class Itinerary
    {
        public ItinerarySummary ItinerarySummary { get; set; }

        public DateTime CheckInDate { get; set; }

        public DateTime CheckOutDate { get; set; }

        public int AdultCount { get; set; }

        public int ChildrensCount { get; set; }

        public List<string> Reviews { get; set; }
        
        public float Fare { get; set; }

        public List<Room> Rooms { get; set; }
    }
}

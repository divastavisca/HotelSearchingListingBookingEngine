using System;
using System.Collections.Generic;
using System.Text;
using SystemContracts.Attributes;
using SystemContracts.Attributes.HotelAttributes;

namespace SystemContracts.InternalContracts
{
    public class BookingSummary
    {
        public string HotelName { get; set; }

        public Guest[] Guests { get; set; }

        public string RoomName { get; set; }

        public DateTime CheckInDate { get; set; }

        public DateTime CheckOutDate { get; set; }

        public decimal TotalAmount { get; set; }

        public string Currency { get; set; }
    }
}

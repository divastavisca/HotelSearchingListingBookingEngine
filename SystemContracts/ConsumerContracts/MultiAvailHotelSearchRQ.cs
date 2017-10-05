using System;
using System.Collections.Generic;
using System.Text;
using SystemContracts.Attributes;
using SystemContracts.ConsumerContracts;

namespace SystemContracts.ConsumerContracts
{
    public class MultiAvailHotelSearchRQ
    {
        public Destination SearchLocation { get; }

        public DateTime CheckInDate { get; }

        public DateTime CheckOutDate { get; }

        public int AdultsCount { get; }

        public int ChildrensCount { get; }

        public int[] ChildrenAges { get; }

        public MultiAvailHotelSearchRQ(Destination searchLocation,DateTime checkInDate,DateTime checkOutDate,int adultsCount,int childrensCount,int[] childrenAges)
        {
            SearchLocation = searchLocation;
            CheckInDate = checkInDate;
            CheckOutDate = checkOutDate;
            AdultsCount = adultsCount;
            ChildrensCount = childrensCount;
            ChildrenAges = childrenAges;
        }
    }
}

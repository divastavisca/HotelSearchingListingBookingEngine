using System;
using System.Collections.Generic;
using System.Text;
using SystemContracts.Attributes;
using SystemContracts.ConsumerContracts;
using SystemContracts.ServiceContracts;

namespace SystemContracts.ConsumerContracts
{
    public class MultiAvailHotelSearchRQ : IEngineServiceRQ
    {
        public Destination SearchLocation { get; set; }

        public DateTime CheckInDate { get; set; }

        public DateTime CheckOutDate { get; set; }

        public int AdultsCount { get; set; }

        public int ChildrenCount { get; set; }

        public List<int> ChildrenAge { get; set; }

        //public MultiAvailHotelSearchRQ(Destination searchLocation,DateTime checkInDate,DateTime checkOutDate,int adultsCount,int childrenCount,List<int> childrenAges)
        //{
        //    SearchLocation = searchLocation;
        //    CheckInDate = checkInDate;
        //    CheckOutDate = checkOutDate;
        //    AdultsCount = adultsCount;
        //    ChildrensCount = childrenCount;
        //    ChildrenAges = childrenAges;
        //}
    }
}

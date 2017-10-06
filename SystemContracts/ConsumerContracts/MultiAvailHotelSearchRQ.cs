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

        public int ChildrensCount { get; set; }

        public int[] ChildrenAges { get; set; }
    }
}

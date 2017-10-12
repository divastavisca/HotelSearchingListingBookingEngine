using System;
using System.Collections.Generic;
using System.Text;
using HotelSearchingListingBookingEngine.Core.ServiceEngines;
using HotelSearchingListingBookingEngine.Core.CustomExceptions;
using SystemContracts.ConsumerContracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CoreEngine.Tests
{
    [TestClass]
    public class MultiAvailHotelSearchEngineTests
    {
        MultiAvailHotelSearchEngine engine;
        MultiAvailHotelSearchRQ searchRq;

        public MultiAvailHotelSearchEngineTests()
        {
            engine = new MultiAvailHotelSearchEngine();
        }
    }
}

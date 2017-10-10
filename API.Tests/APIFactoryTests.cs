using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using SystemContracts.ServiceContracts;
using SystemContracts.ConsumerContracts;
using HotelSearchingListingBooking.API.Models;
using HotelSearchingListingBookingEngine.Core.ServiceProviders;

namespace API.Tests
{
    [TestClass]
    public class APIFactoryTests
    {
        [TestMethod]
        public void API_Factory_Should_Return_Correct_Service_Provider_Instance()
        {
            Assert.IsInstanceOfType(APIServiceFactory.GetServiceProvider(typeof(MultiAvailHotelSearchRQ)), typeof(MultiAvailHotelSearchProvider));
        }
    }
}

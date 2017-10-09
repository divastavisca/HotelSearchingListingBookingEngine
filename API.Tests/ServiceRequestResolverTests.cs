using Microsoft.VisualStudio.TestTools.UnitTesting;
using HotelSearchingListingBooking.API.Models;
using SystemContracts.ConsumerContracts;

namespace API.Tests
{
    [TestClass]
    public class ServiceRequestResolverTests
    {
        ServiceRequest recievedRequest;

        public ServiceRequestResolverTests()
        {
            recievedRequest = new ServiceRequest()
            {
                ServiceName = "MultiAvail",
                JsonRequest = null
            };
        }

        [TestMethod]
        public void Get_Type_Should_Return_Type_As_Requested()
        {
            Assert.IsTrue(ServiceRequestResolver.GetServiceType(recievedRequest) == typeof(MultiAvailHotelSearchRQ));
        }
    }
}

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
                JsonRequest = null
            };
        }

        [TestMethod]
        public void Get_Type_Should_Return_Type_As_Requested()
        {
            recievedRequest.ServiceName = "MultiAvail";
            Assert.IsTrue(ServiceRequestResolver.GetServiceType(recievedRequest) == typeof(MultiAvailHotelSearchRQ));
        }

        [TestMethod]
        public void Get_Type_Should_Return_Only_Valid_Request_Type()
        {
            recievedRequest.ServiceName = "NoService";
            Assert.IsNull(ServiceRequestResolver.GetServiceType(recievedRequest));
        }
    }
}

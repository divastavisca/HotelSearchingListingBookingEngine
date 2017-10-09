using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SystemContracts.ServiceContracts;
using SystemContracts.ConsumerContracts;
using HotelSearchingListingBookingEngine.Core.ServiceProviders;
using System.Reflection;

namespace HotelSearchingListingBooking.API.Models
{
    public class APIServiceFactory
    {
        private readonly Dictionary<Type, Type> _serviceProviderMap = new Dictionary<Type, Type>()
        {
            {typeof(MultiAvailHotelSearchRQ) ,typeof(MultiAvailHotelSearchProvider)},
            {typeof(SingleAvailRoomSearchRQ) , typeof(SingleAvailRoomSearchProvider)}
        };
        
        public IEngineServiceProvider GetServiceProvider(Type serviceRequestType)
        {
            return (IEngineServiceProvider)Activator.CreateInstance(serviceRequestType);
        }
    }
}

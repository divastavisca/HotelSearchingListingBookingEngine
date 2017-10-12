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
        private static readonly Dictionary<Type, Type> _serviceProviderMap = new Dictionary<Type, Type>()
        {
            {typeof(MultiAvailHotelSearchRQ) ,typeof(MultiAvailHotelSearchProvider)},
            {typeof(SingleAvailRoomSearchRQ) , typeof(SingleAvailRoomSearchProvider)},
            {typeof(HotelRoomPricingRQ), typeof(HotelRoomPricingRequestProvider)}
        };
        
        public static IEngineServiceProvider GetServiceProvider(Type serviceRequestType)
        {
            return
           (
                _serviceProviderMap.ContainsKey(serviceRequestType)
                ?
                (IEngineServiceProvider) Activator.CreateInstance(_serviceProviderMap[serviceRequestType])
                :
                null
           );
        }
    }
}

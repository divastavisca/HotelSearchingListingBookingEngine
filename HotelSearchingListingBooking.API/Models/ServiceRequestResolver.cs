using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SystemContracts.ConsumerContracts;
using SystemContracts.ServiceContracts;

namespace HotelSearchingListingBooking.API.Models
{
    public class ServiceRequestResolver
    {
        private static readonly Dictionary<string, Type> _serviceRequestMap = new Dictionary<string, Type>()
        {
            {"MultiAvail", typeof(MultiAvailHotelSearchRQ)},
            {"SingleAvail", typeof(SingleAvailRoomSearchRQ)}
        };

        public static Type GetServiceType(ServiceRequest serviceRequest)
        {
            return _serviceRequestMap.ContainsKey(serviceRequest.ServiceName)? _serviceRequestMap[serviceRequest.ServiceName] : null;
        }
    }
}

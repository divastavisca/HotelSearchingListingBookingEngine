using System;
using System.Collections.Generic;
using System.Text;
using HotelSearchingListingBookingEngine.Core.InternalServiceEngines;
using SystemContracts.ServiceContracts;
using System.Reflection;
using SystemContracts.CustomExceptions;

namespace HotelSearchingListingBookingEngine.Core
{
    public class InternalServiceEnginesFactory
    {
        public static Dictionary<string, Type> _internalEnginesMap = new Dictionary<string, Type>()
        {
            {"Staging", typeof(HotelProductStagingEngine) },
            {"Booking", typeof(HotelProductBookingEngine) }
        };

        public static IInternalServiceEngine GetSupportEngine(string requestedEngine)
        {
            if (_internalEnginesMap.ContainsKey(requestedEngine))
                return (IInternalServiceEngine)Activator.CreateInstance(_internalEnginesMap[requestedEngine]);
            else throw new FactoryException()
            {
                Source = requestedEngine
            };
        }
    }
}

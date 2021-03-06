﻿using System;
using System.Collections.Generic;
using SystemContracts.ServiceContracts;
using System.Text;

namespace HotelSearchingListingBookingEngine.Core.InternalEngineHandshakes
{
    public class ProductStagingInfo : IEngineServiceRQ, IEngineServiceRS
    {
        public string CallerSessionId { get; set; }

        public Guid TripFolderId { get; set; }

        public HotelSearchingListingBooking.ExternalServices.PricingPolicyEngine.Money ProductFare { get; set; }

        public HotelSearchingListingBooking.ExternalServices.PricingPolicyEngine.Payment Payment { get; set; }

        public HotelSearchingListingBooking.ExternalServices.PricingPolicyEngine.HotelTripProduct Product { get; set; }
    }
}

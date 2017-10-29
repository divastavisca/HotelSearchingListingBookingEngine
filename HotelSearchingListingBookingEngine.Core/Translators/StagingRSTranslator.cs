using System;
using System.Collections.Generic;
using System.Text;
using HotelSearchingListingBookingEngine.Core.InternalEngineHandshakes;
using ExternalServices.PricingPolicyEngine;
using HotelSearchingListingBookingEngine.Core.CustomExceptions;

namespace HotelSearchingListingBookingEngine.Core.Translators
{
    public class StagingRSTranslator
    {
        public ProductStagingInfo Translate(TripFolderBookRS tripFolderBookRS)
        {
            try
            {
                if (tripFolderBookRS.TripFolder == null)
                    throw new InvalidObjectRequestException()
                    {
                        Source = typeof(TripFolder).Name
                    };
                return new ProductStagingInfo()
                {
                    CallerSessionId = tripFolderBookRS.SessionId,
                    TripFolderId = tripFolderBookRS.TripFolder.Id,
                    Product = (HotelTripProduct)tripFolderBookRS.TripFolder.Products[0],
                    Payment = tripFolderBookRS.TripFolder.Payments[0],
                    ProductFare = ((HotelTripProduct)tripFolderBookRS.TripFolder.Products[0]).HotelItinerary.Rooms[0].DisplayRoomRate.TotalFare
                };
            }
            catch(InvalidObjectRequestException invalidObjectRequestException)
            {
                Logger.LogException(invalidObjectRequestException.ToString(), invalidObjectRequestException.StackTrace);
                throw new ServiceResponseTranslatorException()
                {
                    Source = invalidObjectRequestException.Source
                };
            }
            catch(Exception baseException)
            {
                Logger.LogException(baseException.ToString(), baseException.StackTrace);
                throw new ServiceResponseTranslatorException()
                {
                    Source = baseException.Source
                };
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using HotelSearchingListingBooking.ExternalServices.HotelSearchEngine;
using SystemContracts.ConsumerContracts;
using SystemContracts.CustomExceptions;
using HotelSearchingListingBooking.Caches;
using HotelSearchingListingBooking.ExternalServices.PricingPolicyEngine;
using HotelSearchingListingBooking.Translators.Utilities;
using Newtonsoft.Json;

namespace HotelSearchingListingBooking.Translators
{
    public class TripProductPriceRQTranslator
    {
        public HotelSearchingListingBooking.ExternalServices.PricingPolicyEngine.TripProductPriceRQ Translate(RoomPricingRQ hotelRoomPricingRQ)
        {
            try
            {
                HotelSearchingListingBooking.ExternalServices.PricingPolicyEngine.TripProductPriceRQ _translatedRQ = new HotelSearchingListingBooking.ExternalServices.PricingPolicyEngine.TripProductPriceRQ();
                _translatedRQ.SessionId = hotelRoomPricingRQ.CallerSessionId;
                _translatedRQ.ResultRequested = HotelSearchingListingBooking.ExternalServices.PricingPolicyEngine.ResponseType.Complete;
                _translatedRQ.TripProduct = getHotelProduct(hotelRoomPricingRQ);
                if (_translatedRQ.TripProduct == null)
                    throw new InvalidObjectRequestException()
                    {
                        Source = _translatedRQ.TripProduct.GetType().Name
                    };
                if (PricingRequestCache.IsPresent(_translatedRQ.SessionId))
                    PricingRequestCache.Remove(_translatedRQ.SessionId);
                PricingRequestCache.AddToCache(_translatedRQ.SessionId, hotelRoomPricingRQ.RoomId);
                return _translatedRQ;
            }
            catch(InvalidObjectRequestException invalidObjectRequestException)
            {
                Logger.LogException(invalidObjectRequestException.ToString(), invalidObjectRequestException.StackTrace);
                throw new ServiceRequestTranslatorException()
                {
                    Source = invalidObjectRequestException.Source
                };
            }
            catch(NullReferenceException nullRefException)
            {
                Logger.LogException(nullRefException.ToString(), nullRefException.StackTrace);
                throw new ServiceRequestTranslatorException()
                {
                    Source = nullRefException.Source
                };
            }
            catch(Exception baseException)
            {
                Logger.LogException(baseException.ToString(), baseException.StackTrace);
                throw new ServiceRequestTranslatorException()
                {
                    Source = baseException.Source
                };
            }
        }

        private TripProduct getHotelProduct(RoomPricingRQ roomPricingRQ)
        {
            try
            {
                return new HotelSearchingListingBooking.ExternalServices.PricingPolicyEngine.HotelTripProduct()
                {
                    Attributes = new HotelSearchingListingBooking.ExternalServices.PricingPolicyEngine.StateBag[1] { new HotelSearchingListingBooking.ExternalServices.PricingPolicyEngine.StateBag() { Name = "API_SESSION_ID", Value = roomPricingRQ.CallerSessionId } },
                    HotelItinerary = getUpdatedItinerary(SelectedItineraryCache.GetSelecetedItinerary(roomPricingRQ.CallerSessionId), roomPricingRQ.RoomId,roomPricingRQ.CallerSessionId),
                    HotelSearchCriterion = JsonConvert.DeserializeObject<HotelSearchingListingBooking.ExternalServices.PricingPolicyEngine.HotelSearchCriterion>(JsonConvert.SerializeObject(SearchCriterionCache.GetSearchCriterion(roomPricingRQ.CallerSessionId)))
                };
            }
            catch (Exception baseException)
            {
                Logger.LogException(baseException.ToString(), baseException.StackTrace);
                throw new ServiceRequestTranslatorException()
                {
                    Source = baseException.GetType().Name
                };
            }
        }

        private HotelSearchingListingBooking.ExternalServices.PricingPolicyEngine.HotelItinerary getUpdatedItinerary(HotelSearchingListingBooking.ExternalServices.HotelSearchEngine.HotelItinerary userSelectedItinerary, string selectedRoomId,string sessionId)
        {
            try
            {
                if (SelectedItineraryRoomsCache.IsPresent(sessionId))
                {
                    userSelectedItinerary.Rooms = getRequestedRoom(SelectedItineraryRoomsCache.GetAllRooms(sessionId), selectedRoomId);
                    if(userSelectedItinerary.Rooms!=null || userSelectedItinerary.Rooms.Length!=0)
                        return JsonConvert.DeserializeObject<HotelSearchingListingBooking.ExternalServices.PricingPolicyEngine.HotelItinerary>(JsonConvert.SerializeObject(userSelectedItinerary));
                }
                throw new InvalidObjectRequestException()
                {
                    Source = typeof(SelectedItineraryRoomsCache).Name
                };
            }
            catch(JsonException jsonException)
            {
                Logger.LogException(jsonException.ToString(), jsonException.StackTrace);
                throw new ServiceRequestTranslatorException()
                {
                    Source = userSelectedItinerary.GetType().Name
                };
            }
            catch (InvalidObjectRequestException invalidObjectRequestException)
            {
                Logger.LogException(invalidObjectRequestException.ToString(), invalidObjectRequestException.StackTrace);
                throw new ServiceRequestTranslatorException()
                {
                    Source = invalidObjectRequestException.Source
                };
            }
            catch (NullReferenceException nullRefException)
            {
                Logger.LogException(nullRefException.ToString(), nullRefException.StackTrace);
                throw new ServiceRequestTranslatorException()
                {
                    Source = userSelectedItinerary.GetType().Name
                };
            }
        }

        private HotelSearchingListingBooking.ExternalServices.HotelSearchEngine.Room[] getRequestedRoom(HotelSearchingListingBooking.ExternalServices.HotelSearchEngine.Room[] rooms,string roomId)
        {
            try
            {
                HotelSearchingListingBooking.ExternalServices.HotelSearchEngine.Room selectedRoom = null;
                foreach (HotelSearchingListingBooking.ExternalServices.HotelSearchEngine.Room room in rooms)
                {
                    if (room.RoomId.ToString() == roomId)
                    {
                        selectedRoom = room;
                        break;
                    }
                }
                if (selectedRoom != null)
                {
                    return new HotelSearchingListingBooking.ExternalServices.HotelSearchEngine.Room[1]
                    {
                        selectedRoom
                    };
                }
                else throw new InvalidObjectRequestException()
                {
                    Source = selectedRoom.GetType().Name
                };
            }
            catch(Exception baseException)
            {
                Logger.LogException(baseException.ToString(), baseException.StackTrace);
                throw new ServiceRequestTranslatorException()
                {
                    Source = baseException.GetType().Name
                };
            }
        }
    }
}

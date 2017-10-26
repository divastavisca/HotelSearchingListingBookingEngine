using System;
using System.Collections.Generic;
using System.Text;
using ExternalServices.HotelSearchEngine;
using SystemContracts.ConsumerContracts;
using HotelSearchingListingBookingEngine.Core.CustomExceptions;
using HotelSearchingListingBookingEngine.Core.Caches;
using ExternalServices.PricingPolicyEngine;
using Newtonsoft.Json;

namespace HotelSearchingListingBookingEngine.Core.Translators
{
    public class TripProductPriceRQTranslator
    {
        public ExternalServices.PricingPolicyEngine.TripProductPriceRQ Translate(RoomPricingRQ hotelRoomPricingRQ)
        {
            try
            {
                ExternalServices.PricingPolicyEngine.TripProductPriceRQ parsedRQ = new ExternalServices.PricingPolicyEngine.TripProductPriceRQ();
                parsedRQ.SessionId = hotelRoomPricingRQ.CallerSessionId;
                parsedRQ.ResultRequested = ExternalServices.PricingPolicyEngine.ResponseType.Complete;
                parsedRQ.TripProduct = getHotelProduct(hotelRoomPricingRQ);
                if (parsedRQ.TripProduct == null)
                    throw new InvalidObjectRequestException()
                    {
                        Source = parsedRQ.TripProduct.GetType().Name
                    };
                if (PricingRequestCache.IsPresent(parsedRQ.SessionId))
                    PricingRequestCache.Remove(parsedRQ.SessionId);
                PricingRequestCache.AddToCache(parsedRQ.SessionId, hotelRoomPricingRQ.RoomId);
                return parsedRQ;
            }
            catch(InvalidObjectRequestException invalidObjectRequestException)
            {
                Logger.LogException(invalidObjectRequestException.ToString(), invalidObjectRequestException.StackTrace);
                throw new ServiceRequestParserException()
                {
                    Source = invalidObjectRequestException.Source
                };
            }
            catch(NullReferenceException nullRefException)
            {
                Logger.LogException(nullRefException.ToString(), nullRefException.StackTrace);
                throw new ServiceRequestParserException()
                {
                    Source = nullRefException.Source
                };
            }
            catch(Exception baseException)
            {
                Logger.LogException(baseException.ToString(), baseException.StackTrace);
                throw new ServiceRequestParserException()
                {
                    Source = baseException.Source
                };
            }
        }

        private TripProduct getHotelProduct(RoomPricingRQ roomPricingRQ)
        {
            try
            {
                return new ExternalServices.PricingPolicyEngine.HotelTripProduct()
                {
                    Attributes = new ExternalServices.PricingPolicyEngine.StateBag[1] { new ExternalServices.PricingPolicyEngine.StateBag() { Name = "API_SESSION_ID", Value = roomPricingRQ.CallerSessionId } },
                    HotelItinerary = getUpdatedItinerary(SelectedItineraryCache.GetSelecetedItinerary(roomPricingRQ.CallerSessionId), roomPricingRQ.RoomId,roomPricingRQ.CallerSessionId),
                    HotelSearchCriterion = JsonConvert.DeserializeObject<ExternalServices.PricingPolicyEngine.HotelSearchCriterion>(JsonConvert.SerializeObject(SearchCriterionCache.GetSearchCriterion(roomPricingRQ.CallerSessionId)))
                };
            }
            catch (Exception baseException)
            {
                Logger.LogException(baseException.ToString(), baseException.StackTrace);
                throw new ServiceRequestParserException()
                {
                    Source = baseException.GetType().Name
                };
            }
        }

        private ExternalServices.PricingPolicyEngine.HotelItinerary getUpdatedItinerary(ExternalServices.HotelSearchEngine.HotelItinerary userSelectedItinerary, string selectedRoomId,string sessionId)
        {
            try
            {
                if (SelectedItineraryRoomsCache.IsPresent(sessionId))
                {
                    userSelectedItinerary.Rooms = getRequestedRoom(SelectedItineraryRoomsCache.GetAllRooms(sessionId), selectedRoomId);
                    if(userSelectedItinerary.Rooms!=null || userSelectedItinerary.Rooms.Length!=0)
                        return JsonConvert.DeserializeObject<ExternalServices.PricingPolicyEngine.HotelItinerary>(JsonConvert.SerializeObject(userSelectedItinerary));
                }
                throw new InvalidObjectRequestException()
                {
                    Source = typeof(SelectedItineraryRoomsCache).Name
                };
            }
            catch(JsonException jsonException)
            {
                Logger.LogException(jsonException.ToString(), jsonException.StackTrace);
                throw new ServiceRequestParserException()
                {
                    Source = userSelectedItinerary.GetType().Name
                };
            }
            catch (InvalidObjectRequestException invalidObjectRequestException)
            {
                Logger.LogException(invalidObjectRequestException.ToString(), invalidObjectRequestException.StackTrace);
                throw new ServiceRequestParserException()
                {
                    Source = invalidObjectRequestException.Source
                };
            }
            catch (NullReferenceException nullRefException)
            {
                Logger.LogException(nullRefException.ToString(), nullRefException.StackTrace);
                throw new ServiceRequestParserException()
                {
                    Source = userSelectedItinerary.GetType().Name
                };
            }
        }

        private ExternalServices.HotelSearchEngine.Room[] getRequestedRoom(ExternalServices.HotelSearchEngine.Room[] rooms,string roomId)
        {
            try
            {
                ExternalServices.HotelSearchEngine.Room selectedRoom = null;
                foreach (ExternalServices.HotelSearchEngine.Room room in rooms)
                {
                    if (room.RoomId.ToString() == roomId)
                    {
                        selectedRoom = room;
                        break;
                    }
                }
                if (selectedRoom != null)
                {
                    return new ExternalServices.HotelSearchEngine.Room[1]
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
                throw new ServiceRequestParserException()
                {
                    Source = baseException.GetType().Name
                };
            }
        }
    }
}

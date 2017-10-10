using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ExternalServices.HotelSearchEngine;
using SystemContracts.ConsumerContracts;
using SystemContracts.Attributes.HotelAttributes;

namespace HotelSearchingListingBookingEngine.Core.Parsers
{
    public class SingleAvailRoomSearchRSParser
    {
        public SingleAvailRoomSearchRS Parse(HotelRoomAvailRS hotelRoomSearchRS)
        {
            SingleAvailRoomSearchRS parsedResponse = new SingleAvailRoomSearchRS();
            try
            {
                var userSearchResults = ItineraryCache.GetItineraries(hotelRoomSearchRS.SessionId);
                foreach (HotelItinerary itinerary in userSearchResults)
                {
                    if (itinerary.HotelProperty.SupplierHotelId == hotelRoomSearchRS.Itinerary.HotelProperty.SupplierHotelId)
                    {
                        parsedResponse.Itinerary = fillResponseItinerary(hotelRoomSearchRS);
                        if (parsedResponse.Itinerary == null)
                            throw new Exception("Unable to fill itinerary for single avail request");
                        ItinerarySummary summary;
                        (new MultiAvailHotelSearchRSParser()).TryParseItinerary(itinerary, out summary, 10);
                        parsedResponse.Itinerary.ItinerarySummary = summary ?? throw new Exception("Cannot parse unique itinerary");
                        parsedResponse.CallerSessionId = hotelRoomSearchRS.SessionId;
                        return parsedResponse;
                    }
                }
            }
            catch (NullReferenceException nullRefExcep)
            {
                Logger.LogException(nullRefExcep.ToString(), nullRefExcep.StackTrace);
                return null;
            }
            catch (Exception baseExcep)
            {
                Logger.LogException(baseExcep.ToString(), baseExcep.StackTrace);
                return null;
            }
            return null;
        }

        private Itinerary fillResponseItinerary(HotelRoomAvailRS hotelRoomSearchRS)
        {
            try
            {
                Itinerary itinerary = new Itinerary();
                itinerary.CheckInDate = hotelRoomSearchRS.Itinerary.StayPeriod.Start;
                itinerary.CheckOutDate = hotelRoomSearchRS.Itinerary.StayPeriod.End;
                if (tryFillGuestsCount(hotelRoomSearchRS.SessionId, itinerary) == false)
                    throw new Exception("Error in filling guests details");
                if (hotelRoomSearchRS.Itinerary.Rooms.Length > 0)
                {
                    List<RoomSummary> roomsItinerarySummary;
                    if (tryFillRoomsSummary(hotelRoomSearchRS.Itinerary.HotelProperty.SupplierHotelId, hotelRoomSearchRS.Itinerary.Rooms, out roomsItinerarySummary) == false)
                        throw new Exception("Error in filling rooms summary itinerary");
                    itinerary.Rooms = roomsItinerarySummary;
                }
                if (hotelRoomSearchRS.Itinerary.HotelProperty.Reviews.Length > 0)
                {
                    List<string> hotelItineraryReviews = null;
                    if (tryFillItineraryReviews(hotelRoomSearchRS.Itinerary.HotelProperty.Reviews, out hotelItineraryReviews))
                        throw new Exception("Error in filling itinerary reviews");
                    itinerary.Reviews = hotelItineraryReviews;
                }
                return itinerary;
            }
            catch (NullReferenceException nullRefExcep)
            {
                Logger.LogException(nullRefExcep.ToString(), nullRefExcep.StackTrace);
                return null;
            }
            catch (Exception baseExcep)
            {
                Logger.LogException(baseExcep.ToString(), baseExcep.StackTrace);
                return null;
            }
        }

        private bool tryFillItineraryReviews(Review[] reviews, out List<string> hotelItineraryReviews)
        {
            try
            {
                hotelItineraryReviews = new List<string>();
                foreach (Review review in reviews)
                {
                    hotelItineraryReviews.Add(review.Text);
                    if (hotelItineraryReviews.Count > 10)
                        break;
                }
                return true;
            }
            catch (NullReferenceException nullRefExcep)
            {
                Logger.LogException(nullRefExcep.ToString(), nullRefExcep.StackTrace);
                hotelItineraryReviews = null;
                return false;
            }
            catch (Exception baseExcep)
            {
                Logger.LogException(baseExcep.ToString(), baseExcep.StackTrace);
                hotelItineraryReviews = null;
                return false;
            }
        }

        private bool tryFillRoomsSummary(string supplierHotelId, Room[] hotelRooms, out List<RoomSummary> roomsItinerarySummary)
        {
            try
            {
                roomsItinerarySummary = new List<RoomSummary>();
                foreach (Room hotelRoom in hotelRooms)
                {
                    RoomSummary roomSummary = new RoomSummary();
                    roomSummary.HotelId = supplierHotelId;
                    roomSummary.Description = hotelRoom.RoomDescription;
                    roomSummary.RoomId = hotelRoom.RoomId.ToString();
                    roomSummary.MaxOccupancy = hotelRoom.MaximumOccupancy;
                    roomSummary.IsPrepaid = hotelRoom.Prepaid;
                    roomSummary.Currency = hotelRoom.DisplayRoomRate.TotalFare.Currency;
                    roomSummary.TotalFare = hotelRoom.DisplayRoomRate.TotalFare.Amount;
                    roomsItinerarySummary.Add(roomSummary);
                }
                return true;
            }
            catch (NullReferenceException nullRefExcep)
            {
                Logger.LogException(nullRefExcep.ToString(), nullRefExcep.StackTrace);
                roomsItinerarySummary = null;
                return false;
            }
            catch (Exception baseExcep)
            {
                Logger.LogException(baseExcep.ToString(), baseExcep.StackTrace);
                roomsItinerarySummary = null;
                return false;
            }
        }

        private bool tryFillGuestsCount(string sessionId, Itinerary itinerary)
        {
            try
            {
                PassengerTypeQuantity[] guests = (SearchCriterionCache.GetSearchCriterion(sessionId)).Guests;
                if (guests[0].PassengerType == PassengerType.Adult)
                {
                    itinerary.AdultCount = guests[0].Quantity;
                    if (guests[1].PassengerType == PassengerType.Child)
                    {
                        itinerary.ChildrensCount = guests[1].Quantity;
                    }
                }
                else if (guests[0].PassengerType == PassengerType.Child)
                {
                    itinerary.ChildrensCount = guests[0].Quantity;
                    if (guests[1].PassengerType == PassengerType.Adult)
                    {
                        itinerary.AdultCount = guests[1].Quantity;
                    }
                }
                return true;
            }
            catch (NullReferenceException nullRefExcep)
            {
                Logger.LogException(nullRefExcep.ToString(), nullRefExcep.StackTrace);
                return false;
            }
            catch (Exception baseExcep)
            {
                Logger.LogException(baseExcep.ToString(), baseExcep.StackTrace);
                return false;
            }
        }
    }
}

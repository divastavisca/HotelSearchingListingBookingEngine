using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ExternalServices.HotelSearchEngine;
using SystemContracts.ConsumerContracts;
using SystemContracts.Attributes.HotelAttributes;
using HotelSearchingListingBookingEngine.Core.CustomExceptions;
using HotelSearchingListingBookingEngine.Core.Caches;

namespace HotelSearchingListingBookingEngine.Core.Translators
{
    public class SingleAvailRoomSearchRSTranslator
    {
        private readonly string[] _deafultSuppliers = { "HotelBeds", "TouricoTGSTest" };

        public SingleAvailRoomSearchRS Translate(HotelRoomAvailRS hotelRoomSearchRS)
        {
            SingleAvailRoomSearchRS parsedResponse = new SingleAvailRoomSearchRS();
            try
            {
                parsedResponse.Itinerary = fillResponseItinerary(hotelRoomSearchRS);
                if (parsedResponse.Itinerary == null)
                    throw new InvalidObjectRequestException()
                    {
                        Source = parsedResponse.Itinerary.GetType().Name
                    };
                ItinerarySummary summary;
                (new MultiAvailHotelSearchRSTranslator()).TryParseItinerary(hotelRoomSearchRS.Itinerary, out summary, 10);
                parsedResponse.Itinerary.ItinerarySummary =
                    summary
                    ??
                    throw new ParseException()
                    {
                        Source = parsedResponse.Itinerary.ItinerarySummary.GetType().Name
                    };
                parsedResponse.CallerSessionId = hotelRoomSearchRS.SessionId;
                return parsedResponse;
            }
            catch (InvalidObjectRequestException invalidObjectRequestException)
            {
                Logger.LogException(invalidObjectRequestException.ToString(), invalidObjectRequestException.StackTrace);
                throw new ServiceResponseParserException()
                {
                    Source = invalidObjectRequestException.Source
                };
            }
            catch (ParseException parseException)
            {
                Logger.LogException(parseException.ToString(), parseException.StackTrace);
                throw new ServiceResponseParserException()
                {
                    Source = parseException.Source
                };
            }
            catch (NullReferenceException nullRefExcep)
            {
                Logger.LogException(nullRefExcep.ToString(), nullRefExcep.StackTrace);
                throw new ServiceResponseParserException()
                {
                    Source = nullRefExcep.Source
                };
            }
            catch (Exception baseExcep)
            {
                Logger.LogException(baseExcep.ToString(), baseExcep.StackTrace);
                throw new ServiceResponseParserException()
                {
                    Source = baseExcep.Source
                };
            }
        }

        private Itinerary fillResponseItinerary(HotelRoomAvailRS hotelRoomSearchRS)
        {
            try
            {
                Itinerary itinerary = new Itinerary();
                itinerary.CheckInDate = hotelRoomSearchRS.Itinerary.StayPeriod.Start;
                itinerary.CheckOutDate = hotelRoomSearchRS.Itinerary.StayPeriod.End;
                if (tryFillGuestsCount(hotelRoomSearchRS.SessionId, itinerary) == false)
                    throw new ObjectInitializationException()
                    {
                        Source = "Guests Count"
                    };
                if (hotelRoomSearchRS.Itinerary.Rooms.Length > 0)
                {
                    List<RoomSummary> roomsItinerarySummary;
                    if (tryFillRoomsSummary(hotelRoomSearchRS.Itinerary.HotelProperty.SupplierHotelId, hotelRoomSearchRS.Itinerary.Rooms, out roomsItinerarySummary) == false)
                        throw new ObjectInitializationException()
                        {
                            Source = roomsItinerarySummary.GetType().Name
                        };
                    itinerary.Rooms = roomsItinerarySummary;
                }
                if (hotelRoomSearchRS.Itinerary.HotelProperty.Reviews.Length > 0)
                {
                    List<string> hotelItineraryReviews = null;
                    if (tryFillItineraryReviews(hotelRoomSearchRS.Itinerary.HotelProperty.Reviews, out hotelItineraryReviews))
                        throw new ObjectInitializationException()
                        {
                            Source = hotelItineraryReviews.GetType().Name
                        };
                    itinerary.Reviews = hotelItineraryReviews;
                }
                return itinerary;
            }
            catch (ObjectInitializationException objectInitializationException)
            {
                Logger.LogException(objectInitializationException.ToString(), objectInitializationException.StackTrace);
                throw new Exception()
                {
                    Source = objectInitializationException.Source
                };
            }
            catch (NullReferenceException nullRefExcep)
            {
                Logger.LogException(nullRefExcep.ToString(), nullRefExcep.StackTrace);
                throw new Exception()
                {
                    Source = nullRefExcep.Source
                };
            }
            catch (Exception baseExcep)
            {
                Logger.LogException(baseExcep.ToString(), baseExcep.StackTrace);
                throw new Exception()
                {
                    Source = baseExcep.Source
                };
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
                throw new Exception()
                {
                    Source = nullRefExcep.Source
                };
            }
            catch (Exception baseExcep)
            {
                Logger.LogException(baseExcep.ToString(), baseExcep.StackTrace);
                hotelItineraryReviews = null;
                throw new Exception()
                {
                    Source = baseExcep.Source
                };
            }
        }

        private bool tryFillRoomsSummary(string supplierHotelId, Room[] hotelRooms, out List<RoomSummary> roomsItinerarySummary)
        {
            try
            {
                roomsItinerarySummary = new List<RoomSummary>();
                foreach (Room hotelRoom in hotelRooms)
                {
                    if (hotelRoom.HotelFareSource.Name.StartsWith(_deafultSuppliers[0]) || hotelRoom.HotelFareSource.Name.StartsWith(_deafultSuppliers[1]))
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
                }
                if (roomsItinerarySummary.Count > 0)
                    return true;
                else return false;
            }
            catch (NullReferenceException nullRefExcep)
            {
                Logger.LogException(nullRefExcep.ToString(), nullRefExcep.StackTrace);
                roomsItinerarySummary = null;
                throw new Exception()
                {
                    Source = nullRefExcep.Source
                };
            }
            catch (Exception baseExcep)
            {
                Logger.LogException(baseExcep.ToString(), baseExcep.StackTrace);
                roomsItinerarySummary = null;
                throw new Exception()
                {
                    Source = baseExcep.Source
                };
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
                throw new Exception()
                {
                    Source = nullRefExcep.Source
                };
            }
            catch (Exception baseExcep)
            {
                Logger.LogException(baseExcep.ToString(), baseExcep.StackTrace);
                throw new Exception()
                {
                    Source = baseExcep.Source
                };
            }
        }
    }
}

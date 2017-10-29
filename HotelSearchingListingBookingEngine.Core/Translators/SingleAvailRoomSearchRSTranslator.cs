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
            SingleAvailRoomSearchRS translatedResponse = new SingleAvailRoomSearchRS();
            try
            {
                if(hotelRoomSearchRS.Itinerary.Rooms==null || hotelRoomSearchRS.Itinerary.Rooms.Length==0)
                {
                    throw new NoResultsFoundException()
                    {
                        Source = typeof(Room).Name
                    };
                }
                translatedResponse.Itinerary = fillResponseItinerary(hotelRoomSearchRS);
                if (translatedResponse.Itinerary == null)
                    throw new InvalidObjectRequestException()
                    {
                        Source = translatedResponse.Itinerary.GetType().Name
                    };
                ItinerarySummary summary;
                (new MultiAvailHotelSearchRSTranslator()).TryTranslateItinerary(hotelRoomSearchRS.Itinerary, out summary, 10);
                translatedResponse.Itinerary.ItinerarySummary =
                    summary
                    ??
                    throw new TranslationException()
                    {
                        Source = translatedResponse.Itinerary.ItinerarySummary.GetType().Name
                    };
                translatedResponse.CallerSessionId = hotelRoomSearchRS.SessionId;
                return translatedResponse;
            }
            catch(NoResultsFoundException noResultsFoundException)
            {
                Logger.LogException(noResultsFoundException.ToString(), noResultsFoundException.StackTrace);
                throw new ServiceResponseTranslatorException()
                {
                    Source = noResultsFoundException.Source
                };
            }
            catch (InvalidObjectRequestException invalidObjectRequestException)
            {
                Logger.LogException(invalidObjectRequestException.ToString(), invalidObjectRequestException.StackTrace);
                throw new ServiceResponseTranslatorException()
                {
                    Source = invalidObjectRequestException.Source
                };
            }
            catch (TranslationException parseException)
            {
                Logger.LogException(parseException.ToString(), parseException.StackTrace);
                throw new ServiceResponseTranslatorException()
                {
                    Source = parseException.Source
                };
            }
            catch (NullReferenceException nullRefExcep)
            {
                Logger.LogException(nullRefExcep.ToString(), nullRefExcep.StackTrace);
                throw new ServiceResponseTranslatorException()
                {
                    Source = nullRefExcep.Source
                };
            }
            catch (Exception baseExcep)
            {
                Logger.LogException(baseExcep.ToString(), baseExcep.StackTrace);
                throw new ServiceResponseTranslatorException()
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
                if (guests.Length > 1)
                {
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
                }
                else
                {
                    if (guests[0].PassengerType == PassengerType.Adult)
                    {
                        itinerary.AdultCount = guests[0].Quantity;
                        itinerary.ChildrensCount = 0;
                    }
                    else
                    {
                        itinerary.AdultCount = 0;
                        itinerary.ChildrensCount = guests[0].Quantity;
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

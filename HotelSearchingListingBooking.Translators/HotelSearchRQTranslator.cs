using System;
using System.Collections.Generic;
using System.Text;
using HotelSearchingListingBooking.ExternalServices.HotelSearchEngine;
using SystemContracts.ServiceContracts;
using SystemContracts.Attributes;
using SystemContracts.Attributes.HotelAttributes;
using SystemContracts.ConsumerContracts;
using HotelSearchingListingBookingEngine.Core;
using Newtonsoft.Json;
using System.IO;
using SystemContracts.CustomExceptions;
using HotelSearchingListingBooking.Translators.Utilities;

namespace HotelSearchingListingBooking.Translators
{
    public class HotelSearchRQTranslator
    {
        private readonly bool _returnOnlyAvailableItineraries = true;
        private readonly string _stateBagObjHscAttributes = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "StateBagData", "StateBagObjectData1.txt");
        private readonly string _stateBagObjAdditionalInfo = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "StateBagData", "StateBagAdditionalAttributesObject.txt");
        private readonly int _maxResults = 1500;
        private readonly bool _matrixResults = true;
        private readonly int _defaultPosId = 101;
        private readonly LocationCodeContext _defaultLocationCodeContext = LocationCodeContext.Address;
        private readonly int _deafaultGmtOffsetMinutes = 0;
        private readonly int _defaultAddressId = 0;
        private readonly string _defaultAddressLine1 = "Test 1";
        private readonly string _defaultAddressLine2 = "Test 2";
        private readonly string _defaultAgencyName = "WV";
        private readonly string _defaultCompanyCode = "DTP";
        private readonly CompanyCodeContext _defaultCompanyCodeContext = CompanyCodeContext.PersonalTravelClient;
        private readonly string _defaultCompanyDk = "3285301P";
        private readonly string _defaultCompanyName = "Rovia";
        private readonly int _defaultCompanyId = 0;
        private readonly string _defaultPriceCurrencyCode = "USD";
        private readonly float _deafultSearchRadius = 50;
        private readonly int _defaultPagingInfoStartNumber = 100;
        private readonly int _defaultPagingInfoEndNumber = 120;
        private readonly int _defaultTotalRecordsBeforeFiltering = 0;
        private readonly int _defaultTotalResults = 0;
        private StaticFilesHandler<StateBag[]> _staticFilesHandler = new StaticFilesHandler<StateBag[]>();

        public HotelSearchRQ Translate(MultiAvailHotelSearchRQ request)
        {
            HotelSearchRQ _translatedRQ = new HotelSearchRQ();
            try
            {
                _translatedRQ.ResultRequested = ResponseType.Complete;
                _translatedRQ.SessionId = Guid.NewGuid().ToString();
                _translatedRQ.Filters = new AvailabilityFilter[1]
                {
                    new AvailabilityFilter()
                    {
                       ReturnOnlyAvailableItineraries = _returnOnlyAvailableItineraries
                    }
                };
                _translatedRQ.HotelSearchCriterion = new HotelSearchCriterion();
                _translatedRQ.HotelSearchCriterion.Attributes = _staticFilesHandler.ParseFileData(_stateBagObjHscAttributes);
                if (_translatedRQ.HotelSearchCriterion.Attributes == null)
                    throw new ObjectInitializationException()
                    {
                        Source = _translatedRQ.HotelSearchCriterion.Attributes.GetType().Name
                    };
                _translatedRQ.HotelSearchCriterion.MatrixResults = _matrixResults;
                _translatedRQ.HotelSearchCriterion.MaximumResults = _maxResults;
                _translatedRQ.HotelSearchCriterion.Pos = new PointOfSale();
                _translatedRQ.HotelSearchCriterion.Pos.PosId = _defaultPosId;
                _translatedRQ.HotelSearchCriterion.Pos.Requester = getDefaultRequester();
                _translatedRQ.HotelSearchCriterion.Pos.AdditionalInfo = _staticFilesHandler.ParseFileData(_stateBagObjAdditionalInfo);
                foreach (StateBag stateBag in _translatedRQ.HotelSearchCriterion.Pos.AdditionalInfo)
                {
                    if (stateBag.Name == "API_SESSION_ID")
                    {
                        stateBag.Value = _translatedRQ.SessionId;
                        break;
                    }
                }
                if (_translatedRQ.HotelSearchCriterion.Pos.Requester == null)
                    throw new ObjectInitializationException()
                    {
                        Source = _translatedRQ.HotelSearchCriterion.Pos.Requester.GetType().Name
                    };
                _translatedRQ.HotelSearchCriterion.PriceCurrencyCode = _defaultPriceCurrencyCode;
                _translatedRQ.HotelSearchCriterion.Guests = getGuestsDetails(request.AdultsCount, request.ChildrenAge.ToArray());
                if (_translatedRQ.HotelSearchCriterion.Guests == null)
                    throw new ObjectInitializationException()
                    {
                        Source = _translatedRQ.HotelSearchCriterion.Guests.GetType().Name
                    };
                _translatedRQ.HotelSearchCriterion.Location = getLocation(request.SearchLocation.Name, request.SearchLocation.Type, request.SearchLocation.GeoCode);
                if (_translatedRQ.HotelSearchCriterion.Location == null)
                    throw new ObjectInitializationException()
                    {
                        Source = _translatedRQ.HotelSearchCriterion.Location.GetType().Name
                    };
                _translatedRQ.HotelSearchCriterion.NoOfRooms = getMinimumRoomsRequired(request.AdultsCount, request.ChildrenAge.Count);
                if (_translatedRQ.HotelSearchCriterion.NoOfRooms <= 0)
                    throw new InvalidValueInitializationException()
                    {
                        Source = _translatedRQ.HotelSearchCriterion.NoOfRooms.GetType().Name
                    };
                _translatedRQ.HotelSearchCriterion.ProcessingInfo = new HotelSearchProcessingInfo()
                {
                    DisplayOrder = HotelDisplayOrder.ByRelevanceScoreDescending
                };
                _translatedRQ.HotelSearchCriterion.RoomOccupancyTypes = new RoomOccupancyType[1]
                {
                    new RoomOccupancyType()
                    {
                         PaxQuantities = _translatedRQ.HotelSearchCriterion.Guests
                    }
                };
                _translatedRQ.HotelSearchCriterion.SearchType = HotelSearchType.City;
                _translatedRQ.HotelSearchCriterion.StayPeriod = getStayPeriod(request.CheckInDate, request.CheckOutDate);
                if (_translatedRQ.HotelSearchCriterion.StayPeriod == null)
                    throw new ObjectInitializationException()
                    {
                        Source = _translatedRQ.HotelSearchCriterion.StayPeriod.GetType().Name
                    };
                _translatedRQ.PagingInfo = new PagingInfo()
                {
                    Enabled = false,
                    StartNumber = _defaultPagingInfoStartNumber,
                    EndNumber = _defaultPagingInfoEndNumber,
                    TotalRecordsBeforeFiltering = _defaultTotalRecordsBeforeFiltering,
                    TotalResults = _defaultTotalResults
                };
            }
            catch(ObjectInitializationException objectInitialisationException)
            {
                Logger.LogException(objectInitialisationException.ToString(), objectInitialisationException.StackTrace);
                throw new ServiceRequestTranslatorException()
                {
                    Source = objectInitialisationException.Source
                };
            }
            catch(InvalidValueInitializationException invalidValueInitializationException)
            {
                Logger.LogException(invalidValueInitializationException.ToString(), invalidValueInitializationException.StackTrace);
                throw new ServiceRequestTranslatorException()
                {
                    Source = invalidValueInitializationException.Source
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
            catch(InvalidOperationException invalidOpExcep)
            {
                Logger.LogException(invalidOpExcep.ToString(),invalidOpExcep.StackTrace);
                throw new ServiceRequestTranslatorException()
                {
                    Source = invalidOpExcep.Source
                };
            }
            catch(Exception baseExcep)
            {
                Logger.LogException(baseExcep.ToString(),baseExcep.StackTrace);
                throw new ServiceRequestTranslatorException()
                {
                    Source = baseExcep.Source
                };
            }
            return _translatedRQ;
        }

        private DateTimeSpan getStayPeriod(DateTime checkInDate, DateTime checkOutDate)
        {
            try
            {
                return new DateTimeSpan()
                {
                    Start = checkInDate,
                    End = checkOutDate,
                    Duration = 0
                };
            }
            catch(Exception excep)
            {
                Logger.LogException(excep.ToString(), excep.StackTrace);
                throw new Exception();
            }
        }

        private int getMinimumRoomsRequired(int adultsCount, int childrensCount) //2 adults + 1 children in a single room or 4 childrens in one room
        {
            int roomsRequired = 0;
            roomsRequired = adultsCount / 2;
            if (roomsRequired < childrensCount)
            {
                childrensCount = childrensCount - roomsRequired;
                int addititionalRooms = childrensCount / 4;
                roomsRequired += addititionalRooms;
                if (adultsCount % 2 != 0)
                {
                    if (childrensCount % 4 == 0)
                        roomsRequired++;
                }
            }
            if (adultsCount == 1)
                roomsRequired++;
            return roomsRequired;
        }

        private Location getLocation(string name, string type, GeoCoordinates geoCode)
        {
            try
            {
                return new Location()
                {
                    CodeContext = LocationCodeContext.GeoCode,
                    Radius = new Distance()
                    {
                        Amount = _deafultSearchRadius,
                        From = LocationCodeContext.GeoCode,
                        Unit = DistanceUnit.mi
                    },
                    GeoCode = JsonConvert.DeserializeObject<GeoCode>(JsonConvert.SerializeObject(geoCode))
                };
            }
            catch(JsonException jsonException)
            {
                Logger.LogException(jsonException.ToString(), jsonException.StackTrace);
                throw new Exception();
            }
            catch(NullReferenceException nullRefExcep)
            {
                Logger.LogException(nullRefExcep.ToString(),nullRefExcep.StackTrace);
                throw new Exception();
            }
            catch(Exception excep)
            {
                Logger.LogException(excep.ToString(),excep.StackTrace);
                throw new Exception();
            }
        }

        private PassengerTypeQuantity[] getGuestsDetails(int adultsCount, int[] childrenAges)
        {
            try
            {
                PassengerTypeQuantity[] passengerTypeQuantity;
                PassengerTypeQuantity adultPassengers = new PassengerTypeQuantity();
                adultPassengers.PassengerType = PassengerType.Adult;
                adultPassengers.Ages = getAges(adultsCount);
                adultPassengers.Quantity = adultsCount;
                if (childrenAges.Length > 0)
                {
                    passengerTypeQuantity = new PassengerTypeQuantity[2];
                    PassengerTypeQuantity childPassengers = new PassengerTypeQuantity();
                    childPassengers.PassengerType = PassengerType.Child;
                    if (childrenAges.Length == 0)
                    {
                        childPassengers.Ages = new int[1] { 0 };
                        childPassengers.Quantity = 1;
                    }
                    else
                    {
                        childPassengers.Quantity = childrenAges.Length;
                        childPassengers.Ages = childrenAges;
                    }
                    passengerTypeQuantity[1] = childPassengers ;
                }
                else passengerTypeQuantity = new PassengerTypeQuantity[1];
                passengerTypeQuantity[0] = adultPassengers;
                return passengerTypeQuantity;
            }
            catch(NullReferenceException nullRefExcep)
            {
                Logger.LogException(nullRefExcep.ToString(), nullRefExcep.StackTrace);
                throw new Exception();
            }
        }

        private int[] getAges(int adultsCount)
        {
            int[] ages = new int[adultsCount];
            int i = 0;
            while (i < adultsCount)
                ages[i++] = 22;
            return ages;
        }

        private Company getDefaultRequester()
        {
            try
            {
                Company company = new Company();
                Agency agency = new Agency();
                Address address = new Address();
                address.CodeContext = _defaultLocationCodeContext;
                address.GmtOffsetMinutes = _deafaultGmtOffsetMinutes;
                address.Id = _defaultAddressId;
                address.AddressLine1 = _defaultAddressLine1;
                address.AddressLine2 = _defaultAddressLine2;
                City city = new City();
                city.CodeContext = LocationCodeContext.City;
                city.GmtOffsetMinutes = _deafaultGmtOffsetMinutes;
                city.Id = _defaultAddressId;
                address.City = city;
                agency.AgencyId = 0;
                agency.AgencyName = _defaultAgencyName;
                company.Code = _defaultCompanyCode;
                company.CodeContext = _defaultCompanyCodeContext;
                company.DK = _defaultCompanyDk;
                company.FullName = _defaultCompanyName;
                company.ID = _defaultCompanyId;
                return company;
            }
            catch(NullReferenceException nullRefExcep)
            {
                Logger.LogException(nullRefExcep.ToString(), nullRefExcep.StackTrace);
                throw new Exception();
            }
            catch(Exception baseExcep)
            {
                Logger.LogException(baseExcep.ToString(), baseExcep.StackTrace);
                throw new Exception();
            }
        }
    }
}

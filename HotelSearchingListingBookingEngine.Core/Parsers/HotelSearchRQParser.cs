using System;
using System.Collections.Generic;
using System.Text;
using ExternalServices.HotelSearchEngine;
using SystemContracts.ServiceContracts;
using SystemContracts.Attributes;
using SystemContracts.Attributes.HotelAttributes;
using SystemContracts.ConsumerContracts;
using HotelSearchingListingBookingEngine.Core;
using Newtonsoft.Json;
using System.IO;
using HotelSearchingListingBookingEngine.Core.CustomExceptions;

namespace HotelSearchingListingBookingEngine.Core.Parsers
{
    public class HotelSearchRQParser
    {
        private readonly bool _returnOnlyAvailableItineraries = true;
        private readonly string _stateBagObjHscAttributes = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"StateBagData","StateBagObjectData1.txt");
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
        private readonly string _defaultPriceCurrencyCode = "INR";
        private readonly float _deafultSearchRadius = 30;
        private readonly int _defaultPagingInfoStartNumber = 100;
        private readonly int _defaultPagingInfoEndNumber = 120;
        private readonly int _defaultTotalRecordsBeforeFiltering = 0;
        private readonly int _defaultTotalResults = 0;

        public HotelSearchRQ Parse(MultiAvailHotelSearchRQ request)
        {
            HotelSearchRQ parsedRQ = new HotelSearchRQ();
            try
            {
                parsedRQ.ResultRequested = ResponseType.Complete;
                parsedRQ.SessionId = Guid.NewGuid().ToString();
                parsedRQ.Filters = new AvailabilityFilter[1]
                {
                    new AvailabilityFilter()
                    {
                       ReturnOnlyAvailableItineraries = _returnOnlyAvailableItineraries
                    }
                };
                parsedRQ.HotelSearchCriterion = new HotelSearchCriterion();
                parsedRQ.HotelSearchCriterion.Attributes = getStateBags(_stateBagObjHscAttributes);
                if (parsedRQ.HotelSearchCriterion.Attributes == null)
                    throw new ObjectInitializationException()
                    {
                        Source = parsedRQ.HotelSearchCriterion.Attributes.GetType().Name
                    };
                parsedRQ.HotelSearchCriterion.MatrixResults = _matrixResults;
                parsedRQ.HotelSearchCriterion.MaximumResults = _maxResults;
                parsedRQ.HotelSearchCriterion.Pos = new PointOfSale();
                parsedRQ.HotelSearchCriterion.Pos.PosId = _defaultPosId;
                parsedRQ.HotelSearchCriterion.Pos.Requester = getDefaultRequester();
                if (parsedRQ.HotelSearchCriterion.Pos.Requester == null)
                    throw new ObjectInitializationException()
                    {
                        Source = parsedRQ.HotelSearchCriterion.Pos.Requester.GetType().Name
                    };
                parsedRQ.HotelSearchCriterion.PriceCurrencyCode = _defaultPriceCurrencyCode;
                parsedRQ.HotelSearchCriterion.Guests = getGuestsDetails(request.AdultsCount, request.ChildrenAge.ToArray());
                if (parsedRQ.HotelSearchCriterion.Guests == null)
                    throw new ObjectInitializationException()
                    {
                        Source = parsedRQ.HotelSearchCriterion.Guests.GetType().Name
                    };
                parsedRQ.HotelSearchCriterion.Location = getLocation(request.SearchLocation.Name, request.SearchLocation.Type, request.SearchLocation.GeoCode);
                if (parsedRQ.HotelSearchCriterion.Location == null)
                    throw new ObjectInitializationException()
                    {
                        Source = parsedRQ.HotelSearchCriterion.Location.GetType().Name
                    };
                parsedRQ.HotelSearchCriterion.NoOfRooms = getMinimumRoomsRequired(request.AdultsCount, request.ChildrenAge.Count);
                if (parsedRQ.HotelSearchCriterion.NoOfRooms <= 0)
                    throw new InvalidValueInitializationException()
                    {
                        Source = parsedRQ.HotelSearchCriterion.NoOfRooms.GetType().Name
                    };
                parsedRQ.HotelSearchCriterion.ProcessingInfo = new HotelSearchProcessingInfo()
                {
                    DisplayOrder = HotelDisplayOrder.ByRelevanceScoreDescending
                };
                parsedRQ.HotelSearchCriterion.RoomOccupancyTypes = new RoomOccupancyType[1]
                {
                    new RoomOccupancyType()
                    {
                         PaxQuantities = parsedRQ.HotelSearchCriterion.Guests
                    }
                };
                parsedRQ.HotelSearchCriterion.SearchType = HotelSearchType.GeoCode;
                parsedRQ.HotelSearchCriterion.StayPeriod = getStayPeriod(request.CheckInDate, request.CheckOutDate);
                if (parsedRQ.HotelSearchCriterion.StayPeriod == null)
                    throw new ObjectInitializationException()
                    {
                        Source = parsedRQ.HotelSearchCriterion.StayPeriod.GetType().Name
                    };
                parsedRQ.PagingInfo = new PagingInfo()
                {
                    Enabled = true,
                    StartNumber = _defaultPagingInfoStartNumber,
                    EndNumber = _defaultPagingInfoEndNumber,
                    TotalRecordsBeforeFiltering = _defaultTotalRecordsBeforeFiltering,
                    TotalResults = _defaultTotalResults
                };
            }
            catch(ObjectInitializationException objectInitialisationException)
            {
                Logger.LogException(objectInitialisationException.ToString(), objectInitialisationException.StackTrace);
                throw new ServiceRequestParserException()
                {
                    Source = objectInitialisationException.Source
                };
            }
            catch(InvalidValueInitializationException invalidValueInitializationException)
            {
                Logger.LogException(invalidValueInitializationException.ToString(), invalidValueInitializationException.StackTrace);
                throw new ServiceRequestParserException()
                {
                    Source = invalidValueInitializationException.Source
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
            catch(InvalidOperationException invalidOpExcep)
            {
                Logger.LogException(invalidOpExcep.ToString(),invalidOpExcep.StackTrace);
                throw new ServiceRequestParserException()
                {
                    Source = invalidOpExcep.Source
                };
            }
            catch(Exception baseExcep)
            {
                Logger.LogException(baseExcep.ToString(),baseExcep.StackTrace);
                throw new ServiceRequestParserException()
                {
                    Source = baseExcep.Source
                };
            }
            return parsedRQ;
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
                PassengerTypeQuantity[] passengerTypeQuantity = new PassengerTypeQuantity[2];
                PassengerTypeQuantity adultPassengers = new PassengerTypeQuantity();
                adultPassengers.PassengerType = PassengerType.Adult;
                adultPassengers.Quantity = adultsCount;
                PassengerTypeQuantity childPassengers = new PassengerTypeQuantity();
                childPassengers.PassengerType = PassengerType.Child;
                childPassengers.Quantity = childrenAges.Length;
                childPassengers.Ages = childrenAges;
                passengerTypeQuantity[0] = adultPassengers;
                passengerTypeQuantity[1] = childPassengers;
                return passengerTypeQuantity;
            }
            catch(NullReferenceException nullRefExcep)
            {
                Logger.LogException(nullRefExcep.ToString(), nullRefExcep.StackTrace);
                throw new Exception();
            }
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

        private StateBag[] getStateBags(string fileName)
        {
            byte[] fileData;
            try
            {
                fileData = File.ReadAllBytes(fileName);
            }
            catch (IOException ioException)
            {
                Logger.StoreLog(ioException.ToString());
                throw new Exception();
            }
            catch (Exception exception)
            {
                Logger.StoreLog(exception.ToString());
                throw new Exception();
            }
            try
            {
                string data = ASCIIEncoding.ASCII.GetString(fileData);
                data=data.TrimStart('?');
                return (JsonConvert.DeserializeObject<StateBag[]>(data));
            }
            catch (Exception baseException)
            {
                Logger.StoreLog(baseException.ToString());
                throw new Exception();
            }
        }
    }
}

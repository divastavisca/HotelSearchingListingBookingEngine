using System;
using System.Collections.Generic;
using System.Text;
using ExternalServices.HotelSearchEngineConnecter;
using SystemContracts.ServiceContracts;
using SystemContracts.Attributes;
using SystemContracts.Attributes.HotelAttributes;
using SystemContracts.ConsumerContracts;
using HotelSearchingListingBookingEngine.Core;
using Newtonsoft.Json;
using System.IO;

namespace HotelSearchingListingBookingEngine.Core.Parsers
{
    public class HotelSearchRQParser
    {
        private readonly bool _returnOnlyAvailableItineraries = true;
        private readonly string _stateBagObjHscAttributes = "SBObject.txt";
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
        private readonly Dictionary<string, LocationCodeContext> _locationTypeResolver = new Dictionary<string, LocationCodeContext>()
        {
            {"Address",LocationCodeContext.Address },
            {"Airport",LocationCodeContext.Airport },
            {"City",LocationCodeContext.City },
            {"GeoCode",LocationCodeContext.GeoCode },
            {"PointOfInterest",LocationCodeContext.PointOfInterest},
            {"RentalLocation",LocationCodeContext.RentalLocation },
            {"ZipCode", LocationCodeContext.ZipCode }
        };
        private readonly Dictionary<string, HotelSearchType> _hotelSearchTypeResolver = new Dictionary<string, HotelSearchType>()
        {
            {"Address",HotelSearchType.Address },
            {"Airport",HotelSearchType.Airport },
            {"City",HotelSearchType.City },
            {"GeoCode",HotelSearchType.GeoCode },
            {"PointOfInterest",HotelSearchType.PointOfInterest},
            {"ZipCode", HotelSearchType.ZipCode },
            {"IdList",HotelSearchType.IdList },
            {"Tags",HotelSearchType.Tags }
        };
        private readonly int _defaultPagingInfoStartNumber = 100;
        private readonly int _defaultPagingInfoEndNumber = 120;
        private readonly int _defaultTotalRecordsBeforeFiltering = 0;
        private readonly int _defaultTotalResults = 0;

        public HotelSearchRQ Parse(MultiAvailHotelSearchRQ request)
        {
            HotelSearchRQ parsedRQ = new HotelSearchRQ();
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
            parsedRQ.HotelSearchCriterion.MatrixResults = _matrixResults;
            parsedRQ.HotelSearchCriterion.MaximumResults = _maxResults;
            parsedRQ.HotelSearchCriterion.Pos = new PointOfSale();
            parsedRQ.HotelSearchCriterion.Pos.PosId = _defaultPosId;
            parsedRQ.HotelSearchCriterion.Pos.Requester = getDefaultRequester();
            parsedRQ.HotelSearchCriterion.PriceCurrencyCode = _defaultPriceCurrencyCode;
            parsedRQ.HotelSearchCriterion.Guests = getGuestsDetails(request.AdultsCount, request.ChildrenAges);
            parsedRQ.HotelSearchCriterion.Location = getLocation(request.SearchLocation.Name, request.SearchLocation.Type, request.SearchLocation.GeoCode);
            parsedRQ.HotelSearchCriterion.NoOfRooms = getMinimumRoomsRequired(request.AdultsCount, request.ChildrenAges.Length);
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
            parsedRQ.HotelSearchCriterion.SearchType = _hotelSearchTypeResolver[request.SearchLocation.Type];
            parsedRQ.HotelSearchCriterion.StayPeriod = getStayPeriod(request.CheckInDate,request.CheckOutDate);
            parsedRQ.PagingInfo = new PagingInfo()
            {
                Enabled = true,
                StartNumber = _defaultPagingInfoStartNumber,
                EndNumber = _defaultPagingInfoEndNumber,
                TotalRecordsBeforeFiltering = _defaultTotalRecordsBeforeFiltering,
                TotalResults = _defaultTotalResults
            };
            return parsedRQ;
        }

        private DateTimeSpan getStayPeriod(DateTime checkInDate, DateTime checkOutDate)
        {
            return new DateTimeSpan()
            {
                Start = checkInDate,
                End = checkOutDate,
                Duration = 0
            };
        }

        private int getMinimumRoomsRequired(int adultsCount, int childrensCount)
        {
            return (adultsCount / 2 + childrensCount / 2);
        }

        private Location getLocation(string name, string type, GeoCoordinates geoCode)
        {
            return new Location()
            {
                CodeContext = _locationTypeResolver[type],
                Radius = new Distance()
                {
                    Amount = _deafultSearchRadius,
                    From = _locationTypeResolver[type],
                    Unit = DistanceUnit.mi
                },
                GeoCode = JsonConvert.DeserializeObject<GeoCode>(JsonConvert.SerializeObject(geoCode))
            };
        }

        private PassengerTypeQuantity[] getGuestsDetails(int adultsCount, int[] childrenAges)
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

        private Company getDefaultRequester()
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

        private StateBag[] getStateBags(string fileName)
        {
            byte[] fileData;
            try
            {
                fileData = File.ReadAllBytes(Path.GetFullPath(fileName));
            }
            catch (IOException ioException)
            {
                Logger.StoreLog(ioException.ToString());
                return null;
            }
            catch (Exception exception)
            {
                Logger.StoreLog(exception.ToString());
                return null;
            }
            try
            {
                return JsonConvert.DeserializeObject<StateBag[]>(ASCIIEncoding.ASCII.GetString(fileData));
            }
            catch (Exception baseException)
            {
                Logger.StoreLog(baseException.ToString());
                return null;
            }
        }

    }
}

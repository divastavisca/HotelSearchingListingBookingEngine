using System;
using System.Collections.Generic;
using System.Text;
using HotelSearchingListingBookingEngine.Core.Caches;
using HotelSearchingListingBookingEngine.Core.CustomExceptions;
using ExternalServices.PricingPolicyEngine;
using SystemContracts.ConsumerContracts;
using Newtonsoft.Json;
using System.IO;
using ExternalServices.HotelSearchEngine;
using SystemContracts.Attributes;

namespace HotelSearchingListingBookingEngine.Core.Parsers
{
    public class TripFolderBookRQParser
    {
        private readonly string _creatorAdditionalInfoDataFile = @"StateBagData\StateBagDataCreaterAdditionalInfo.txt";
        private readonly string _creatorEMail = "sbejugam@v-worldventures.com";
        private readonly string _creatorFirstName = "Sandbox";
        private readonly string _creatorMiddleName = "User";
        private readonly string _creatorLastName = "Test";
        private readonly string _creatorPrefix = "Mr.";
        private readonly string _creatorTitle = "Mr";
        private readonly long _creatorUserId = 169050;
        private readonly string _creatorUserName = "3285301";
        private readonly string _defaultKnownTravelerNumber = "789456";
        private readonly string _ownerAdditionalDataFile = @"StateBagData\StateBagDataOwnerAdditionalData.txt";
        private readonly string _folderAdditionalDataFile = @"StateBagData\StateBagDataFolderAdditionalData.txt";
        private readonly string _folderPassengerCustomData = @"StateBagData\StateBagCustomData.txt";
        private readonly Dictionary<string, ExternalServices.PricingPolicyEngine.PassengerType> _passengerTypeMap = new Dictionary<string, ExternalServices.PricingPolicyEngine.PassengerType>()
        {
            {"Adult",ExternalServices.PricingPolicyEngine.PassengerType.Adult },
            {"Child",ExternalServices.PricingPolicyEngine.PassengerType.Child }
        };
        private readonly Dictionary<string, ExternalServices.PricingPolicyEngine.LocationCodeContext> _locationCodeContext = new Dictionary<string, ExternalServices.PricingPolicyEngine.LocationCodeContext>()
        {
            {"City", ExternalServices.PricingPolicyEngine.LocationCodeContext.City },
            {"Address", ExternalServices.PricingPolicyEngine.LocationCodeContext.Address },
            {"GeoCode", ExternalServices.PricingPolicyEngine.LocationCodeContext.GeoCode }
        };

        public TripFolderBookRQ Parse(HotelProductBookRQ hotelProductBookRQ)
        {
            try
            {
                TripFolderBookRQ parsedRQ = new TripFolderBookRQ();
                parsedRQ.AdditionalInfo = getStateBags(_folderAdditionalDataFile);
                parsedRQ.TripFolder = constructTripFolder(hotelProductBookRQ);
                if (parsedRQ.TripFolder == null)
                    throw new InvalidObjectRequestException()
                    {
                        Source = parsedRQ.TripFolder.GetType().Name
                    };
                parsedRQ.ResultRequested = ExternalServices.PricingPolicyEngine.ResponseType.Unknown;
                parsedRQ.SessionId = hotelProductBookRQ.CallerSessionId;
                parsedRQ.TripProcessingInfo = new TripProcessingInfo();
                parsedRQ.TripProcessingInfo.TripProductRphs = new int[1] { 0 };
                return parsedRQ;
            }
            catch (InvalidObjectRequestException invalidObjectRequestException)
            {
                Logger.LogException(invalidObjectRequestException.ToString(), invalidObjectRequestException.StackTrace);
                throw new ServiceRequestParserException()
                {
                    Source = invalidObjectRequestException.Source
                };
            }
            catch (NullReferenceException nullReferenceException)
            {
                Logger.LogException(nullReferenceException.ToString(), nullReferenceException.StackTrace);
                throw new ServiceRequestParserException()
                {
                    Source = nullReferenceException.Source
                };
            }
            catch (Exception baseException)
            {
                Logger.LogException(baseException.ToString(), baseException.StackTrace);
                throw new ServiceRequestParserException()
                {
                    Source = baseException.Source
                };
            }
        }

        private TripFolder constructTripFolder(HotelProductBookRQ hotelProductBookRQ)
        {
            try
            {
                TripFolder tripFolder = new TripFolder();
                tripFolder.FolderName = "TripFolder"+DateTime.Now.ToString();
                tripFolder.Creator = getUser(_creatorAdditionalInfoDataFile);
                tripFolder.CreatedDate = DateTime.Now;
                tripFolder.Owner = getUser(_ownerAdditionalDataFile);
                tripFolder.Pos = getPos((SearchCriterionCache.GetSearchCriterion(hotelProductBookRQ.CallerSessionId)).Pos);
                tripFolder.Type = TripFolderType.Personal;
                tripFolder.Passengers = parsePassengers(hotelProductBookRQ.Guests);
                tripFolder.Payments = getPayment(hotelProductBookRQ.PaymentDetails, hotelProductBookRQ.CallerSessionId);
                tripFolder.Products = getProducts(tripFolder,hotelProductBookRQ.CallerSessionId);
                tripFolder.Status = TripStatus.Planned;
                return tripFolder;
            }
            catch (Exception baseException)
            {
                Logger.LogException(baseException.ToString(), baseException.StackTrace);
                throw new ServiceRequestParserException()
                {
                    Source = baseException.Source
                };
            }
        }

        private TripProduct[] getProducts(TripFolder folder,string sessionId)
        {
            var tripProduct = new TripProduct[1];
            tripProduct[0] = (HotelTripProduct)Caches.TripProductCache.GetItineraries(sessionId);
            ((HotelTripProduct)tripProduct[0]).HotelItinerary.HotelCancellationPolicy.CancellationRules = new ExternalServices.PricingPolicyEngine.HotelCancellationRule[] { new ExternalServices.PricingPolicyEngine.HotelCancellationRule() };
            tripProduct[0].LeadPassengerRph = 0;
            tripProduct[0].Owner = folder.Owner;
            tripProduct[0].PassengerSegments = new PassengerSegment[1]
            {
                        new PassengerSegment()
                        {
                            BookingStatus = TripProductStatus.Planned,
                            LineNumber = 0,
                            PassengerRph = 0,
                            PostBookingStatus = PostBookingTripStatus.None,
                            Rph = 0,
                        }
            };
            tripProduct[0].PaymentBreakups = new PaymentBreakup[1]
            {
                        new PaymentBreakup()
                        {
                            Amount = ((HotelTripProduct)tripProduct[0]).HotelItinerary.Rooms[0].DisplayRoomRate.TotalFare,
                            PassengerRph = 0,
                            PaymentRph = 0,
                        }
            };
            tripProduct[0].PaymentOptions = new PaymentType[1]
            {
                        PaymentType.Credit
            };
            tripProduct[0].Rph = 0;
            return tripProduct;
        }

        private Payment[] getPayment(PaymentDetails paymentDetails, string sessionId)
        {
            try
            {
                return new Payment[1]
                {
                    new CreditCardPayment()
                    {
                        Attributes = new ExternalServices.PricingPolicyEngine.StateBag[1] { new ExternalServices.PricingPolicyEngine.StateBag() { Name = "API_SESSION_ID", Value = sessionId } },
                        Amount = new ExternalServices.PricingPolicyEngine.Money()
                        {
                            Amount = paymentDetails.Amount,
                            BaseEquivAmount =0,
                            Currency = paymentDetails.Currency,
                            DisplayAmount = paymentDetails.Amount,
                            DisplayCurrency = paymentDetails.Currency,
                            UsdEquivAmount = 0
                        },
                        BillingAddress = getBillingAddress(paymentDetails),
                        PaymentType = PaymentType.Credit,
                        Rph = 0,
                        CardMake = new CreditCardMake()
                        {
                            Code = paymentDetails.CreditCardDetails.Code,
                            Name = paymentDetails.CreditCardDetails.CardName
                        },
                        CardType = CreditCardType.Personal,
                        ExpiryMonthYear = paymentDetails.CreditCardDetails.Expiry,
                        IsThreeDAuthorizeRequired = paymentDetails.CreditCardDetails.IsThreeDAuth,
                        NameOnCard = paymentDetails.CreditCardDetails.NameOnCard,
                        Number = paymentDetails.CreditCardDetails.CardNumber,
                        SecurityCode = paymentDetails.CreditCardDetails.Cvv
                    }
                };
            }
            catch (Exception baseException)
            {
                Logger.LogException(baseException.ToString(), baseException.StackTrace);
                throw new ServiceRequestParserException()
                {
                    Source = baseException.Source
                };
            }
        }

        private ExternalServices.PricingPolicyEngine.Address getBillingAddress(PaymentDetails paymentDetails)
        {
            return new ExternalServices.PricingPolicyEngine.Address()
            {
                CodeContext = _locationCodeContext[paymentDetails.BillingAddress.AddressContext],
                AddressLine1 = paymentDetails.BillingAddress.AddressLine1,
                AddressLine2 = paymentDetails.BillingAddress.AddressLine2,
                GmtOffsetMinutes = 0,
                Id = 0,
                PhoneNumber = paymentDetails.BillingAddress.PhoneNumber,
                City = new ExternalServices.PricingPolicyEngine.City()
                {
                    CodeContext = ExternalServices.PricingPolicyEngine.LocationCodeContext.City,
                    GmtOffsetMinutes = 0,
                    Id = 0,
                    Name = paymentDetails.BillingAddress.City,
                    State = paymentDetails.BillingAddress.State
                },
                ZipCode = paymentDetails.BillingAddress.ZipCode,
            };
        }

        private Passenger[] parsePassengers(Guest[] guests)
        {
            try
            {
                List<Passenger> guestList = new List<Passenger>();
                foreach (Guest guest in guests)
                {
                    Passenger passenger = new Passenger();
                    passenger.Age = guest.Age;
                    passenger.BirthDate = guest.DateOfBirth;
                    passenger.Email = guest.Email;
                    passenger.FirstName = guest.Name.FirstName;
                    passenger.MiddleName = guest.Name.MiddleName;
                    passenger.LastName = guest.Name.LastName;
                    passenger.PassengerType = _passengerTypeMap[guest.Type];
                    passenger.Rph = 0;
                    passenger.KnownTravelerNumber = _defaultKnownTravelerNumber;
                    passenger.CustomFields = getStateBags(_folderPassengerCustomData);
                    guestList.Add(passenger);
                }
                return guestList.ToArray();
            }
            catch (Exception baseException)
            {
                Logger.LogException(baseException.ToString(), baseException.StackTrace);
                throw new ServiceRequestParserException()
                {
                    Source = baseException.Source
                };
            }
        }

        private ExternalServices.PricingPolicyEngine.PointOfSale getPos(ExternalServices.HotelSearchEngine.PointOfSale pos)
        {
            return JsonConvert.DeserializeObject<ExternalServices.PricingPolicyEngine.PointOfSale>(JsonConvert.SerializeObject(pos));
        }

        private User getUser(string file)
        {
            return new User()
            {
                AdditionalInfo = getStateBags(_creatorAdditionalInfoDataFile),
                Email = _creatorEMail,
                FirstName = _creatorFirstName,
                MiddleName = _creatorMiddleName,
                LastName = _creatorLastName,
                Prefix = _creatorPrefix,
                Title = _creatorTitle,
                UserId = _creatorUserId,
                UserName = _creatorUserName
            };
        }

        private ExternalServices.PricingPolicyEngine.StateBag[] getStateBags(string fileName)
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
                data = data.TrimStart('?');
                return (JsonConvert.DeserializeObject<ExternalServices.PricingPolicyEngine.StateBag[]>(data));
            }
            catch (Exception baseException)
            {
                Logger.StoreLog(baseException.ToString());
                throw new Exception();
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using SystemContracts.ConsumerContracts;
using ExternalServices.HotelSearchEngine;
using SystemContracts.Attributes.HotelAttributes;
using Newtonsoft.Json;
using SystemContracts.Attributes;
using HotelSearchingListingBookingEngine.Core.CustomExceptions;

namespace HotelSearchingListingBookingEngine.Core.Translators
{
    public class MultiAvailHotelSearchRSTranslator
    {
        public MultiAvailHotelSearchRS Translate(HotelSearchRS hotelSearchRS)
        {
            try
            {
                MultiAvailHotelSearchRS multiAvailHotelSearchRS = new MultiAvailHotelSearchRS()
                {
                    CallerSessionId = hotelSearchRS.SessionId
                };
                multiAvailHotelSearchRS.Itineraries = translateItineraries(hotelSearchRS.Itineraries);
                multiAvailHotelSearchRS.ResultsCount = multiAvailHotelSearchRS.Itineraries.Length;
                return multiAvailHotelSearchRS;
            }
            catch (NullReferenceException nullRefExcep)
            {
                Logger.LogException(nullRefExcep.ToString(), nullRefExcep.StackTrace);
                Logger.StoreLog(_exceptionMap[0]);
                throw new ServiceResponseTranslatorException()
                {
                    Source = nullRefExcep.Source
                };
            }
            catch (Exception baseExcep)
            {
                Logger.LogException(baseExcep.ToString(), baseExcep.ToString());
                Logger.StoreLog(_exceptionMap[0]);
                throw new ServiceResponseTranslatorException()
                {
                    Source = baseExcep.Source
                };
            }
        }

        private ItinerarySummary[] translateItineraries(HotelItinerary[] itineraries)
        {
            try
            {
                List<ItinerarySummary> fetchedItineraries = new List<ItinerarySummary>();
                foreach (HotelItinerary hotelItinerary in itineraries)
                {
                    ItinerarySummary uniqueItinerary;
                    if (TryTranslateItinerary(hotelItinerary, out uniqueItinerary,3) && uniqueItinerary != null)
                        fetchedItineraries.Add(uniqueItinerary);
                }
                return fetchedItineraries.Count > 0 ? fetchedItineraries.ToArray() : null;
            }
            catch (NullReferenceException nullRefExcep)
            {
                Logger.LogException(nullRefExcep.ToString(), nullRefExcep.StackTrace);
                Logger.StoreLog(_exceptionMap[1]);
                throw new Exception()
                {
                    Source = nullRefExcep.Source
                };
            }
            catch (Exception baseExcep)
            {
                Logger.LogException(baseExcep.ToString(), baseExcep.ToString());
                Logger.StoreLog(_exceptionMap[1]);
                throw new Exception()
                {
                    Source = baseExcep.Source
                };
            }
        }

        public bool TryTranslateItinerary(HotelItinerary hotelItinerary, out ItinerarySummary uniqueItinerary,int maxImagesCount)
        {
            uniqueItinerary = new ItinerarySummary();
            try
            {
                uniqueItinerary.ItineraryId = hotelItinerary.HotelProperty.SupplierHotelId;
                uniqueItinerary.Name = hotelItinerary.HotelProperty.Name;
                uniqueItinerary.Address = hotelItinerary.HotelProperty.Address.CompleteAddress;
                uniqueItinerary.GeoCode = JsonConvert.DeserializeObject<GeoCoordinates>(JsonConvert.SerializeObject(hotelItinerary.HotelProperty.GeoCode));
                List<string> uniqueAmenities = new List<string>();
                foreach (Amenity hotelAmenity in hotelItinerary.HotelProperty.Amenities)
                {
                    if (hotelAmenity != null)
                        uniqueAmenities.Add(hotelAmenity.Name);
                }
                uniqueItinerary.Amenities = uniqueAmenities.ToArray();
                List<string> imageUrls = new List<string>();
                foreach (Media mediaInfo in hotelItinerary.HotelProperty.MediaContent)
                {
                    if (imageUrls.Count >= maxImagesCount)
                        break;
                    if (mediaInfo.Type == MediaType.Photo && mediaInfo.Url != null)
                    {
                        imageUrls.Add(mediaInfo.Url);
                    }
                }
                uniqueItinerary.ImageUrl = imageUrls.Count > 0 ? imageUrls.ToArray() : null;
                uniqueItinerary.StarRating = hotelItinerary.HotelProperty.HotelRating.Rating;
                uniqueItinerary.Currency = hotelItinerary.Fare.BaseFare.Currency;
                uniqueItinerary.MinimumPrice = hotelItinerary.Fare.BaseFare.Amount;
                return true;
            }
            catch(JsonException jsonException)
            {
                Logger.LogException(jsonException.ToString(), jsonException.StackTrace);
                throw new Exception()
                {
                    Source = jsonException.Source
                };
            }
            catch (NullReferenceException nullRefExcep)
            {
                Logger.LogException(nullRefExcep.ToString(), nullRefExcep.StackTrace);
                Logger.StoreLog(_exceptionMap[2]);
                throw new Exception()
                {
                    Source = nullRefExcep.Source
                };
            }
            catch (Exception baseExcep)
            {
                Logger.LogException(baseExcep.ToString(), baseExcep.ToString());
                Logger.StoreLog(_exceptionMap[2]);
                throw new Exception()
                {
                    Source = baseExcep.Source
                };
            }
        }

        private readonly Dictionary<int, string> _exceptionMap = new Dictionary<int, string>()
        {
            {0, "Unable to translate Response object" },
            {1, "Unable to translate Itineraries object" },
            {2, "Unable to translate unique Itinerary object"}
        };
    }
}

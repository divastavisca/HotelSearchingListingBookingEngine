using System;
using System.Collections.Generic;
using System.Text;
using SystemContracts.ConsumerContracts;
using ExternalServices.HotelSearchEngine;
using SystemContracts.Attributes.HotelAttributes;
using Newtonsoft.Json;
using SystemContracts.Attributes;

namespace HotelSearchingListingBookingEngine.Core.Parsers
{
    public class MultiAvailHotelSearchRSParser
    {
        public MultiAvailHotelSearchRS Parse(HotelSearchRS hotelSearchRS)
        {
            try
            {
                MultiAvailHotelSearchRS multiAvailHotelSearchRS = new MultiAvailHotelSearchRS()
                {
                    CallerSessionId = hotelSearchRS.SessionId,
                    ResultsCount = hotelSearchRS.Itineraries.Length
                };
                multiAvailHotelSearchRS.Itineraries = parseItineraries(hotelSearchRS.Itineraries);
                return multiAvailHotelSearchRS;
            }
            catch (NullReferenceException nullRefExcep)
            {
                Logger.LogException(nullRefExcep.ToString(), nullRefExcep.StackTrace);
                Logger.StoreLog(_exceptionMap[0]);
                return null;
            }
            catch (Exception baseExcep)
            {
                Logger.LogException(baseExcep.ToString(), baseExcep.ToString());
                Logger.StoreLog(_exceptionMap[0]);
                return null;
            }
        }

        private ItinerarySummary[] parseItineraries(HotelItinerary[] itineraries)
        {
            try
            {
                List<ItinerarySummary> fetchedItineraries = new List<ItinerarySummary>();
                foreach (HotelItinerary hotelItinerary in itineraries)
                {
                    ItinerarySummary uniqueItinerary;
                    if (TryParseItinerary(hotelItinerary, out uniqueItinerary,3) && uniqueItinerary != null)
                        fetchedItineraries.Add(uniqueItinerary);
                }
                return fetchedItineraries.Count > 0 ? fetchedItineraries.ToArray() : null;
            }
            catch (NullReferenceException nullRefExcep)
            {
                Logger.LogException(nullRefExcep.ToString(), nullRefExcep.StackTrace);
                Logger.StoreLog(_exceptionMap[1]);
                return null;
            }
            catch (Exception baseExcep)
            {
                Logger.LogException(baseExcep.ToString(), baseExcep.ToString());
                Logger.StoreLog(_exceptionMap[1]);
                return null;
            }
        }

        public bool TryParseItinerary(HotelItinerary hotelItinerary, out ItinerarySummary uniqueItinerary,int maxImagesCount)
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
            catch (NullReferenceException nullRefExcep)
            {
                Logger.LogException(nullRefExcep.ToString(), nullRefExcep.StackTrace);
                Logger.StoreLog(_exceptionMap[2]);
                return false;
            }
            catch (Exception baseExcep)
            {
                Logger.LogException(baseExcep.ToString(), baseExcep.ToString());
                Logger.StoreLog(_exceptionMap[2]);
                return false;
            }
        }

        private readonly Dictionary<int, string> _exceptionMap = new Dictionary<int, string>()
        {
            {0, "Unable to parse Response object" },
            {1, "Unable to parse Itineraries object" },
            {2, "Unable to parse unique Itinerary object"}
        };
    }
}

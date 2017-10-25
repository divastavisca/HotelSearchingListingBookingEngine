var autoCompleteMapping = {};
var sessionId;
var jsonResponseData;
var placeSuggestions;
var suggestionArray = new Array();
var multiAvailHotelSearchRS;
var childrenAgeArray;
var currentAutoCompleteRequest = $.ajax("", {});
var currentMultiAvailRequest = $.ajax("", {});
function getStars(stars)
{
    var starHtml="<span class='rating'>";
    for (var starCount = 0; starCount < stars; starCount++)
    {
        if ((stars - starCount) % 1 == 0)
        {
            starHtml += "<img src='../resources/star.png'/>";
        }
        else
        {
            starHtml += "<img src='../resources/star-half-empty.png'/>"
        }
    }
    starHtml += "</span>";
    return starHtml;
}
function getAmenities(itineraryAmenities)
{
    var amenitiesHtml="Amenities: ";
    for (var amenitiesCount = 0; amenitiesCount < 5; amenitiesCount++)
    {
        if (itineraryAmenities[amenitiesCount])
        {
            amenitiesHtml += itineraryAmenities[amenitiesCount]+" ";
        }
        else
        {
            break;
        }
    }
    return amenitiesHtml;
}
function listHotels(jsonObject) {
    sessionId = jsonObject["callerSessionId"];
    
    for (var hotel = 0; hotel < jsonObject["resultsCount"]; hotel++)
    {
        $("#hotels-list-container").empty();
        var itinerariesCount = jsonObject['itineraries'].length;
        for (var itinerary = 0; itinerary < itinerariesCount; itinerary++)
        {
            var currentItineraries = jsonObject["itineraries"][itinerary];
            var htmlToAppend = "<div class='itinerary'>";
            htmlToAppend += "<img class ='hero-image' src=" + currentItineraries['imageUrl'][0] + ">";
            htmlToAppend += "<div class='hotel-details'>";
            htmlToAppend += "<div><h2>" + currentItineraries['name'] + "</h2>" + getStars(currentItineraries['starRating']) + "</div>";
            htmlToAppend += "<div><div class='amenities'>" + getAmenities(currentItineraries['amenities']) + "</div><button class='view-on-map'>View On Map</button></div>";
            htmlToAppend += "<div><div class='address'>Address:" + currentItineraries['address'] + "</div>";
            htmlToAppend += "<a class='slide-cover' target='_blank' href='singleavail.html?cid=" + sessionId + "&iid=" + currentItineraries['itineraryId'] + "'> View Deal</a ></div>";
            htmlToAppend += "</div>";
            htmlToAppend += "</div>";
            $("#hotels-list-container").append(htmlToAppend);
        } 
    }
}
function showLoadingImage()
{
    var loadingImageHtml ='<div class="loading-image-container"><img src="../resources/loading1.gif"><h3>Searching...</h3></div>';
    $("#hotels-list-container").attr('min-height','120px');
    $("#hotels-list-container").html(loadingImageHtml);
}

$(document).ready(function ()
{    
        {
            $("#location").autocomplete(
                {
                    source: suggestionArray
                });
        }//initialising autocomplete widget to location
        
        $("#location").focus();
        $("#location").on('keyup input propertychange', function () {
            $("#secondaryElementsContainer").css("display", "block");
            var locationtext = $("#location").val();
            var autoSuggestRequestUrl = "http://portal.dev-rovia.com/Services/api/Content/GetAutoCompleteDataGroups?type=poi&query=" + locationtext;
            currentAutoCompleteRequest = $.ajax({
                url: autoSuggestRequestUrl,
                method: 'get',
                data: null,
                crossDomain: true,
                dataType: 'jsonp',
                success: function (json) {
                    if (json != null) {
                        jsonResponseData = json;
                        var culturedText;
                        for (var counter = 0; counter < json.length; counter++) 
                        {
                            var itemsList = json[counter].ItemList;
                            for (var innercounter = 0; innercounter < itemsList.length; innercounter++)
                            {
                                if (counter == 0 && innercounter == 0)
                                {
                                    if (itemsList[innercounter].SubItemList)
                                    {
                                        for (var SubItem = 0; SubItem < itemsList[innercounter].SubItemList.length; SubItem++)
                                        {
                                            culturedText = itemsList[innercounter].SubItemList[SubItem].CulturedText;
                                            autoCompleteMapping[culturedText] = itemsList[innercounter].SubItemList[SubItem].Id;
                                            placeSuggestions = itemsList[innercounter].CulturedText + "|";
                                        }
                                    }
                                    else
                                    {
                                        culturedText = itemsList[innercounter].CulturedText;
                                        autoCompleteMapping[culturedText] = itemsList[innercounter].Id;
                                        placeSuggestions = itemsList[innercounter].CulturedText + "|";
                                    }
                                    
                                }
                                else
                                {
                                    if (itemsList[innercounter].SubItemList) {
                                        for (var SubItem = 0; SubItem < itemsList[innercounter].SubItemList.length; SubItem++)
                                        {
                                            culturedText = itemsList[innercounter].SubItemList[SubItem].CulturedText;
                                            autoCompleteMapping[culturedText] = itemsList[innercounter].SubItemList[SubItem].Id;
                                            placeSuggestions += itemsList[innercounter].CulturedText + "|";
                                        }
                                    }
                                    else {
                                        culturedText = itemsList[innercounter].CulturedText;
                                        autoCompleteMapping[culturedText] = itemsList[innercounter].Id;
                                        placeSuggestions += itemsList[innercounter].CulturedText + "|";
                                    }
                                }

                            }
                        }
                        suggestionArray = placeSuggestions.split('|');
                        $("#location").autocomplete("option", "source", suggestionArray);
                    }
                }
            });
        });
        $("#checkoutdate").datepicker({
            dateFormat: 'dd-mm-yy',
            defaultDate: 1,
            minDate: 2,
            maxDate: '+10m',
            onSelect: function (selectedDate) {
                var dateParts = selectedDate.split("-");
                var maxDateString = dateParts[1] + "-" + dateParts[0] + "-" + dateParts[2];
                var maxCheckInDate = new Date(maxDateString);
                maxCheckInDate.setDate(maxCheckInDate.getDate() - 1);
                $("#checkindate").datepicker("option", "maxDate", maxCheckInDate);
            }
        });
        $("#checkindate").datepicker({
            dateFormat: 'dd-mm-yy',
            defaultDate: null,
            minDate: 1,
            maxDate: '+10m',
            onSelect: function (selectedDate) {
                var dateParts = selectedDate.split("-");
                var minDateString = dateParts[1] + "-" + dateParts[0] + "-" + dateParts[2];
                var mincheckoutdate = new Date(minDateString);
                mincheckoutdate.setDate(mincheckoutdate.getDate() + 1);
                $("#checkoutdate").datepicker("option", "minDate", mincheckoutdate);

            }
        });
        $("#childrencount").change(function () {
            var count = $("#childrencount").val();
            var childrenAgeHtml = "";
            var childAgeOptions =
                '<option value="1">1</option>' +
                '<option value="2">2</option>' +
                '<option value="3">3</option>' +
                '<option value="4">4</option>' +
                '<option value="5">5</option>' +
                '<option value="6">6</option>' +
                '<option value="7">7</option>' +
                '<option value="8">8</option>' +
                '<option value="9">9</option>' +
                '<option value="10">10</option>' +
                '<option value="11">11</option>' +
                '<option value="12">12</option>' +
                '<option value="13">13</option>' +
                '<option value="14">14</option>' +
                '<option value="15">15</option>' +
                '<option value="16">16</option>' +
                '<option value="17">17</option>';

            $("#children-age-container").empty();
            for (var counter = 0; counter < count; counter++) {
                childrenAgeHtml += "<label>Child " + (counter + 1) + " Age</label>"
                childrenAgeHtml += "<select id='child" + counter + "age'>"
                    + childAgeOptions +
                    "</select>";
            }
            $("#children-age-container").append(childrenAgeHtml);
        });
        $("#search-hotels-button").on('click', function () {
            var locationId = autoCompleteMapping[$("#location").val()];
            childrenAgeArray = new Array();
            childrenAgeArray.pop();
            for (var index = 0; index < $("#childrencount").val(); index++) {
                var childAgeDivId = "#child" + (index) + "age";
                childrenAgeArray.push($(childAgeDivId).val());
            }
            for (var counter = 0; counter < (jsonResponseData.length); counter++) {
                {
                    for (var innerCounter = 0; innerCounter < (jsonResponseData[counter].ItemList.length); innerCounter++) {
                        if (jsonResponseData[counter].ItemList[innerCounter].Id == locationId) {
                            var Date = ($('#checkindate').val()).split("-");
                            var year = Date[2];
                            Date[2] = Date[0];
                            Date[0] = year;
                            var checkInDate = Date;
                            Date = ($('#checkoutdate').val()).split("-");
                            year = Date[2];
                            Date[2] = Date[0];
                            Date[0] = year;
                            var checkOutDate = Date;
                            var jsonLocationObject = jsonResponseData[counter].ItemList[innerCounter];
                            var multiAvailRQ = {
                                "SearchLocation": {
                                    'Name': jsonLocationObject.Name,
                                    'Type': jsonLocationObject.SearchType,
                                    'GeoCode':
                                    {
                                        'Latitude': parseFloat(jsonLocationObject.Latitude).toFixed(4),
                                        'Longitude': parseFloat(jsonLocationObject.Longitude).toFixed(4)
                                    },

                                },
                                "CheckInDate": checkInDate[0] + '-' + checkInDate[1] + '-' + checkInDate[2],
                                "CheckOutDate": checkOutDate[0] + '-' + checkOutDate[1] + '-' + checkOutDate[2],
                                "AdultsCount": parseInt($('#adultcount').val()),
                                "ChildrenCount": parseInt($('#childrencount').val()),
                                "ChildrenAge": childrenAgeArray
                            };
                            var multiAvailRQString = JSON.stringify(multiAvailRQ);
                            // multiAvailRQString=multiAvailRQString.replace('"',"'");
                            var IEngineServiceRQ =
                                {
                                    "ServiceName": "MultiAvail",
                                    "JsonRequest": multiAvailRQString
                                };
                            //                                       var IEngineServiceRQ='{\"ServiceName\": "MultiAvail"' + '\"JsonRequest\":'+multiAvailRQString+'};';
                            //                                       
                            //currentMultiAvailRequest.abort();
                            currentMultiAvailRequest = $.ajax({
                                type: 'post',
                                headers:
                                {
                                    "Content-Type": "application/json"
                                },
                                url: "../padharojanab/value",
                                cache: false,
                                data: JSON.stringify(IEngineServiceRQ),
                                beforeSend: showLoadingImage,
                                success: function (response) { $("#hotels-list-container").empty(); listHotels(response); location.href = "#hotels-list-container"; },
                                error: function (response) { $("#hotels-list-container").html("No Results Found"); location.href = "#hotels-list-container" }
                                //error: function (response) { window.alert("Sorry! Some error occured.Please Try Again"); console.log(response);},
                                //complete:function(jqXHR,textStatus){console.log("multi avail request status:"+jqXHR.getResponseHeader());}
                            });
                        }
                    }
                }
            }
        });
        //(".itinerary").on('mouseover', function () { this.children('.slide-cover').css('width', '100%'); });
        //$(".itinerary").on('', function () { });
});

function listHotels(jsonObject) {
    sessionId = jsonObject["callerSessionId"];
    for (var hotel = 0; hotel < jsonObject["resultsCount"]; hotel++) {
        $("#hotels-list-container").empty();
        var itinerariesCount = jsonObject['itineraries'].length;
        for (var itinerary = 0; itinerary < itinerariesCount; itinerary++) {
            var currentItineraries = jsonObject["itineraries"][itinerary];
            var htmlToAppend =
                "<tr class='itinerary'><td> Name :" + currentItineraries['name'] + "</td>";
            htmlToAppend += "<td> <img class='hero-image' src=" + currentItineraries['imageUrl'][0] + "></td>";
            htmlToAppend +="<td> <input type='hidden' value='"+ currentItineraries['itineraryId'] +"></td>";
            htmlToAppend += "<td>Address:<p>" + currentItineraries['address'] + "</p></td>";
            htmlToAppend +="<td><button type='button' onclick='showOnMap(" +currentItineraries['geoCode']['latitude'] + "," +
                currentItineraries['geoCode']['longitude'] +
                ")' >View On Map</button></td>";
            htmlToAppend +=
                "<td>Rating :" + currentItineraries['starRating'] + " Stars</td>";
            htmlToAppend += "<td> Starting From :" + currentItineraries['minimumPrice'] + currentItineraries['currency'] + "</td></tr>";
            htmlToAppend += "<td><a class='view-deal-button' target='_blank' href='singleavail.html?cid=" + sessionId + "&iid=" + currentItineraries['itineraryId'] + "'>View Deal</a>";
            $("#hotels-list-container").append(htmlToAppend);
        }
    }
}
//function showLoadingImage()
//{
//    var loadingImageHtml='<img src="../resources/loading1.gif">';
//    $("#hotels-list-container").attr('min-height','120px');
//    $("#hotels-list-container").html(loadingImageHtml);
//}
//function hideLoadingImage() {
//    $("#hotels-list-container").empty();
//}

//function listHotels(jsonObject)
//{
//    var hotelListTemplate=$("#hotel-list-template").html();
//    var hotelList=Handlebars.compile(hotelListTemplate);
//    var htmlToBeAppended=hotelList(jsonObject);
//    $("#hotels-list-container").html(htmlToBeAppended);
//}
$(document).ready(
    function () {
        var sessionId;
        var jsonResponseData;
        var placeSuggestions;
        var suggestionArray = new Array();
        var multiAvailHotelSearchRS;
        var childrenAgeArray;
        var currentAutoCompleteRequest = $.ajax("", {});
        var currentMultiAvailRequest = $.ajax("", {});
        {
            $("#location").autocomplete(
                {
                    source: suggestionArray
                });
        }//initialising autocomplete widget to location

        $("#location").on('keyup input propertychange', function () {
            currentAutoCompleteRequest.abort();
            $("#secondaryElementsContainer").css("display", "block");
            var locationtext = $("#location").val();
            var autoSuggestRequestUrl = "http://portal.dev-rovia.com/Services/api/Content/GetAutoCompleteDataGroups?type=city|airport|poi&query=" + locationtext;
            currentAutoCompleteRequest = $.ajax({
                url: autoSuggestRequestUrl,
                method: 'get',
                data: null,
                crossDomain: true,
                dataType: 'jsonp',
                success: function (json) {
                    if (json != null) {
                        jsonResponseData = json;
                        for (var counter = 0; counter < json.length; counter++) {
                            var itemsList = json[counter].ItemList;
                            for (var innercounter = 0; innercounter < itemsList.length; innercounter++) {
                                if (counter == 0 && innercounter == 0) {
                                    placeSuggestions = itemsList[innercounter].Name + '|' + itemsList[innercounter].CityName + '|' + itemsList[innercounter].Id + '|' + itemsList[innercounter].SearchType + ',';
                                }
                                else {
                                    placeSuggestions += itemsList[innercounter].Name + '|' + itemsList[innercounter].CityName + '|' + itemsList[innercounter].Id + '|' + itemsList[innercounter].SearchType + ',';
                                }

                            }
                        }
                        suggestionArray = placeSuggestions.split(',');
                        suggestionArray.pop();
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
            var locationParts = ($("#location").val().split('|'));
            var locationId = locationParts[2];
            var locationType = locationParts[locationParts.length - 1];
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
                                success: function (response) { listHotels(response); },
                                //error:function(response){console.log(response);},
                                //complete:function(jqXHR,textStatus){console.log("multi avail request status:"+jqXHR.getResponseHeader());}
                            });
                        }
                    }
                }
            }
        });

    });
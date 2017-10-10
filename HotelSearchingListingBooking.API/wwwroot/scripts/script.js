function listHotels(jsonObject){
    for(var hotel=0;hotel<jsonObject["resultsCount"];hotel++)
        {
            var itinerariesCount=jsonObject['itineraries'].length;
            for(var itinerary=0;itinerary<itinerariesCount;itinerary++)
                {
                    var currentItineraries=jsonObject["itineraries"][itinerary];
                    var htmlToAppend=
                    "<tr class='itinerary'><td> Name :"+currentItineraries['name']+"</td>";
                    htmlToAppend+="<td> <img class='hero-image' src="+currentItineraries['imageUrl'][0]+"></td>";
                    htmlToAppend+="<td> ItineraryID :"+currentItineraries['itineraryId']+"</td>";
                    htmlToAppend+="<td> Latitude :"+currentItineraries['geoCode']['latitude']+"</td>";
                    htmlToAppend+="<td> Longitude :"+currentItineraries['geoCode']['longitude']+"</td>";
                    htmlToAppend+="<td> Star Rating :"+currentItineraries['starRating']+"</td>";
                    htmlToAppend+="<td> Currency :"+currentItineraries['currency']+"</td>";
                    htmlToAppend+="<td> Starting From :"+currentItineraries['minimumPrice']+"</td></tr>";
                    $("#hotels-list-container").append(htmlToAppend);
                }
        }
}
$(document).ready(
function (){
    var multiAvailHotelSearchRS=  {
      "callerSessionId": "e3612fa3-d3a4-4b01-a721-dbdaa8940279",
      "resultsCount": 1,
      "itineraries": [
          {
              "itineraryId": "686292",
              "name": "Hotel Ashish Palace",
              "address": {
                          "addressLine1": "Tourist Complex Area, Fatehabad Road",
                          "addressLine2": "",
                          "city": "Agra",
                          "state": "",
                          "country": "IN",
                          "zipCode": "282001"
                      },
              "geoCode": {
                          "latitude": 27.16237,
                          "longitude": 78.03633
                      },
              "amenities": [
                  "Dry cleaning/laundry service",
                  "Air conditioning",
                  "Coffee/tea maker",
                  "Laundry facilities",
                  "24-hour front desk",
                  "Premium TV channels",
                  "Refrigerator",
                  "Flat-panel TV",
                  "Daily housekeeping",
                  "Free self parking",
                  "Cable TV service",
                  "Free newspapers in lobby",
                  "Free WiFi",
                  "Free newspaper",
                  "Phone",
                  "Desk",
                  "Safe-deposit box at front desk",
                  "24-hour business center",
                  "Private bathroom",
                  "Restaurant",
                  "Pay movies",
                  "Breakfast available (surcharge)",
                  "Coffee shop or café",
                  "Shower only",
                  "Free bottled water",
                  "Separate sitting area",
                  "One meeting room",
                  "Spa services on site",
                  "Airport transportation (surcharge)",
                  "In-room safe",
                  "Tours/ticket assistance",
                  "Ceiling fan",
                  "Luggage storage",
                  "Minibar",
                  "Free toiletries",
                  "Iron/ironing board (on request)",
                  "Concierge services",
                  "Room service (24 hours)",
                  "Rooftop terrace",
                  "Separate dining area",
                  "Separate living room",
                  "Rollaway/extra beds (surcharge)",
                  "Fireplace",
                  "In-room climate control (air conditioning)",
                  "Accessible bathroom",
                  "In-room accessibility"
],
              "imageUrl": [
                  "https://assets.wvholdings.com/2/IMAGES/HotelImagesV3/250000/241601/ff32d7e8_b.jpg",
                  "https://assets.wvholdings.com/2/IMAGES/HotelImagesV3/250000/241601/e4114df6_b.jpg",
                  "https://assets.wvholdings.com/2/IMAGES/HotelImagesV3/250000/241601/f7f74a7e_b.jpg"
],
              "starRating": 3,
              "currency": "INR",
              "minimumPrice": 7000
          }
                            ]
    };
    listHotels(multiAvailHotelSearchRS);
    var placeSuggestions;var suggestionArray=new Array();var multiAvailHotelSearchRS;
    {for(var index=1;index<18;index++)
        {
            window.childAgeList+="<option value="+index+">"+index+"</option>";
        }
    }//generating 17 options for child age
    {
        $("#location").autocomplete(
            {
                source:suggestionArray
            });
    }//initialising autocomplete widget to location
    {
        $("#location").on('input propertychange',function(){$("#secondaryElementsContainer").css("display","block")});
    }//binding function to location input field-on input secondary elements show
    $("#location").on('keyup input propertychange',function()
            {
                var locationtext=$("#location").val();
                var autoSuggestRequestUrl="http://portal.dev-rovia.com/Services/api/Content/GetAutoCompleteDataGroups?type=city|airport|poi&query="+locationtext;
                $.ajax({
                    url:autoSuggestRequestUrl,
                    method:'get',
                    data:null,
                    crossDomain:true,
                    dataType:'jsonp',
                    success:function(json){
                        if(json!=null)
                        {
                            window.jsonResponseData=json;
                            for(var counter=0;counter<json.length;counter++)
                            {
                                var itemsList =json[counter].ItemList;
                                for(var innercounter=0;innercounter<itemsList.length;innercounter++)
                                    {
                                        if(counter==0&&innercounter==0)
                                            {
                                               placeSuggestions=itemsList[innercounter].Name+'|'+itemsList[innercounter].CityName+'|'+itemsList[innercounter].Id+'|'+itemsList[innercounter].SearchType+',';
                                            }
                                        else
                                            {
                                               placeSuggestions+=itemsList[innercounter].Name+'|'+itemsList[innercounter].CityName+'|'+itemsList[innercounter].Id+'|'+itemsList[innercounter].SearchType+',';
                                            }

                                    }
                            }
                            suggestionArray=placeSuggestions.split(',');
                            suggestionArray.pop();
                            $("#location").autocomplete("option","source",suggestionArray);
                        }
                        
                    }
                    });
    });
    $("#checkoutdate").datepicker({
        dateFormat:'dd-mm-yy',
        defaultDate:1,
        minDate:2,
        maxDate:'+10m',
        onSelect:function(selectedDate){
        $("#checkindate").datepicker("option","maxDate",selectedDate);
    }});
    $("#checkindate").datepicker({
        dateFormat:'dd-mm-yy',
        defaultDate:null,
        minDate:1,
        maxDate:'+10m',
        onSelect:function(selectedDate){
            var dateParts=selectedDate.split("-");
            var mincheckoutdate=new Date(dateParts[2],dateParts[1],dateParts[0]);
            mincheckoutdate.setDate(mincheckoutdate.getDate()+1);
            $("#checkoutdate").datepicker("option","minDate",mincheckoutdate);
            
    }});        
    $("#childrencount").change(function(){
        var count=$("#childrencount").val();
        var childrenAgeHtml="";
        $("#children-age-container").empty();
        for(var counter=0;counter<count;counter++)
            {
                childrenAgeHtml+="<label>Child "+counter+" Age</label>"
                childrenAgeHtml+="<select id='child"+counter+"age'>"
                +childAgeList+
                    "</select>";
            }
        $("#children-age-container").append(childrenAgeHtml);
    });
    $("#search-hotels-button").click(function(){
        var locationParts=($("#location").val().split('|'));
        var locationId=locationParts[2];
        var locationType=locationParts[locationParts.length-1];
        var childrenAgeArray=new Array();
        childrenAgeArray.pop();
        for(var index=0;index<$("#childrencount").val();index++)
            {
                var childAgeDivId="#child"+index+"age";
                childrenAgeArray.push($(childAgeDivId).val());
            }
        for(var counter=0;counter<(jsonResponseData.length);counter++)
            {
                    {
                        for(var innerCounter=0;innerCounter<(jsonResponseData[counter].ItemList.length);innerCounter++)
                            {
                                if(jsonResponseData[counter].ItemList[innerCounter].Id==locationId)
                                   {
                                       var jsonLocationObject= jsonResponseData[counter].ItemList[innerCounter];
                                       var multiAvailRQ = {
                                                                'SearchLocation':{
                                                                                    'Name':jsonLocationObject.Name,
                                                                                    'Type':jsonLocationObject.SearchType,
                                                                                    'GeoCode':
                                                                                        {
                                                                                           'Latitude':jsonLocationObject.Latitude,
                                                                                           'Longitude':jsonLocationObject.Longitude
                                                                                        },
                                                                                    
                                                                                },
                                                                'CheckInDate':$('#checkindate').val(),
                                                                'CheckOutDate':$('#checkoutdate').val(),
                                                                'AdultsCount':parseInt($('#adultcount').val()),
                                                                'ChildrenCount':parseInt($('#childrencount').val()),
                                                                'ChildrenAges':childrenAgeArray
                                                            };
                                       var multiAvailRQString=JSON.stringify(multiAvailRQ);
                                       multiAvailRQString=multiAvailRQString.replace('"',"'");
                                       var IEngineServiceRQ=
                                        {
                                            "ServiceName":"MultiAvail",
                                           "JsonRequest":multiAvailRQ
                                       };
//                                       var IEngineServiceRQ='{\"ServiceName\": "MultiAvail"' + '\"JsonRequest\":'+multiAvailRQString+'};';
//                                       
                                       $.ajax(
                                           {
                                               type:'post',
                                               headers:
                                                {
                                                    "Content-Type": "application/json"
                                                },
                                               url:"../padharojanab/value",
                                               data:JSON.stringify(IEngineServiceRQ),
                                               success:function(response){console.log(response);},
                                               error:function(response){console.log(response);},
                                               
                                           });
                                   }
                            }
                    }
            }
        });    
});

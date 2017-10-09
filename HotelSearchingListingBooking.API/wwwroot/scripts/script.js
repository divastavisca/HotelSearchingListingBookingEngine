$(document).ready(
function (){
    var placeSuggestions;var suggestionArray=new Array();
    for(var index=1;index<18;index++)
    {
                window.childAgeList+="<option value="+index+">"+index+"</option>";
            }
    $("#location").autocomplete({source:suggestionArray});
    $("#location").on('input propertychange',function(){$("#secondaryElementsContainer").css("display","block")});
    $("#location").on('keyup input propertychange',function(){
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
        window.isMultiAvailObjectFound=false;
        var multiAvailRQ;
        for(var counter=0;counter<(jsonResponseData.length);counter++)
            {
//                if((jsonResponseData[counter].SearchType==locationType)||(jsonResponseData[counter].SearchType=="POI"))
                    {
                        for(var innerCounter=0;innerCounter<(jsonResponseData[counter].ItemList.length);innerCounter++)
                            {
                                if(jsonResponseData[counter].ItemList[innerCounter].Id==locationId)
                                   {
                                    var jsonLocationObject = jsonResponseData[counter].ItemList[innerCounter];
                                    //multiAvailRQ = {
                                    //    "SearchLocation": {
                                    //        "Name": "Taj Mahal",
                                    //        "Type": "GeoCode",
                                    //        "GeoCode": {
                                    //            "Latitude": 27.173891,
                                    //            "Longitude": 78.04207
                                    //        }
                                    //    },
                                    //    "CheckInDate": "2017-10-10",
                                    //    "CheckOutDate": "2017-10-12",
                                    //    "AdultsCount": 2,
                                    //    "ChildrensCount": 3,
                                    //    "ChildrenAges": [
                                    //        12,
                                    //        12,
                                    //        12
                                    //    ]
                                    //}
                                    multiAvailRQ = {
                                                                "SearchLocation":{
                                                                                    "Name":jsonLocationObject.Name,
                                                                                    "Type":jsonLocationObject.SearchType,
                                                                                    "GeoCode":
                                                                                        {
                                                                                           "Latitude":jsonLocationObject.Latitude,
                                                                                           "Longitude":jsonLocationObject.Longitude
                                                                                        }
                                                                                },
                                                                "CheckInDate":$("#checkindate").val(),
                                                                "CheckOutDate":$("#checkoutdate").val(),
                                                                "AdultsCount":parseInt($("#adultcount").val()),
                                                                "ChildrensCount":parseInt($("#childrencount").val()),
                                                                "ChildrenAges":childrenAgeArray
                                                            };
                                       isMultiAvailObjectFound=true;
                                       break;
                                   }
                                    else {continue;}
                            }
                    }
            if(isMultiAvailObjectFound){break;}
            }
        var multiAvailUrl = "../padharojanab/value";
        //var jsondata = JSON.parse(multiAvailRQ);
        //$.post(multiAvailUrl,multiAvailRQ).done(function(data){console.log(data);});
        $.ajax({
            //    headers:{"Content-Type": "application/json"},
            contentType: "application/json",
            type: "POST",
            url: multiAvailUrl,
            data: multiAvailRQ,
            //dataType:'jsonp',
            //cache: false,
            //crossDomain:true,
            success:function(data){console.log(JSON.stringify(data));}
        });
        
        });    
});
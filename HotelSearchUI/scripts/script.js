$(document).ready(
function(){
    var placeSuggestions;var suggestionArray=new Array();
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
                            for(var counter=0;counter<json.length;counter++)
                            {
                            var itemsList =json[counter].ItemList;
                            for(var innercounter=0;innercounter<itemsList.length;innercounter++)
                                {
                                    if(counter==0&&innercounter==0)
                                        {
                                            placeSuggestions=itemsList[innercounter].Name+' '+itemsList[innercounter].CityName+' '+itemsList[innercounter].Id.toString()+',';
                                        }
                                    else
                                    {
                                        placeSuggestions+=itemsList[innercounter].Name+' '+itemsList[innercounter].CityName+' '+itemsList[innercounter].Id.toString()+',';
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
    
    });


}
);
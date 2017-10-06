$(document).ready(
function(){
    var placeSuggestions;
    $("#destination").autocomplete({source:new Array()});
    $("#destination").keypress(function(){
        var destinationtext=$("#destination").val();
            
                var requestUrl="http://portal.dev-rovia.com/Services/api/Content/GetAutoCompleteDataGroups?type=city|airport|poi&query="+destinationtext;
              $.ajax({
                    url:requestUrl,
                    method:'get',
                    data:null,
                    crossDomain:true,
                    dataType:'jsonp',
                    success:function(json){
                        for(var counter=0;counter<json.length;counter++)
                        {
                            var itemsList =json[counter].ItemList;
                            for(var innercounter=0;innercounter<itemsList.length;innercounter++)
                                {
                                    if(counter==0&&innercounter==0)
                                        {
                                            placeSuggestions=itemsList[innercounter].Name+',';
                                        }
                                    else
                                    {
                                        placeSuggestions+=itemsList[innercounter].Name+',';
                                    }
                                    
                                }
                        }
                        var suggestionArray=placeSuggestions.split(',');
                        suggestionArray.pop();
                        $("#destination").autocomplete("option","source",suggestionArray);
                    }
                    });
    })
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
    $("")


}
);
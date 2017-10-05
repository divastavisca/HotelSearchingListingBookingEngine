$(document).ready(
function(){
    $("#destination").keypress(function(){
        var destinationtext=$("#destination").val();
        if(destinationtext.length>1)
            {
                var requestUrl="http://portal.dev-rovia.com/Services/api/Content/GetAutoCompleteDataGroups?type=city|airport|poi&query="+destinationtext;
//                $.get(requestUrl).done(function(data){var jqueryXml=$(data);console.log(jqueryXml);});
                    $.ajax({
                    url:requestUrl,
                    method:'get',
                    data:null,
                    dataType:'jsonp',
                    success:function(json){
                        var jqueryXml=$(json);
                        console.log(jqueryXml.text());
                        jqueryXml.find('').text();}
                    });
            }
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
    


}
);
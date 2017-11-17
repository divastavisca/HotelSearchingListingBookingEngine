//function initMap(){
//    var latLngObj = { lat: itineraryDetails['geoCode']['latitude'], lng: itineraryDetails['geoCode']['longitude'] };
//    var map = new google.maps.Map(document.getElementById('map-container'), { zoom: 4, center: latLngObj });
//    var marker = new google.maps.marker({ position: latLngObj, map: map });
//}
var itineraryName;
var itineraryAddress;
{
function showMap() {
    $("#map-container").css({ "height": "100%", "width": "100%" });
}
function listHotelDetails(jsonObject)
{
    itineraryDetails = jsonObject['itinerary']['itinerarySummary'];
    itineraryName = itineraryDetails['name'];
    var checkInDate = jsonObject['itinerary']['checkInDate'];
    var checkOutDate = jsonObject['itinerary']['checkOutDate'];
    var adultsCount = jsonObject['itinerary']['adultCount'];
    var childrenCount = jsonObject['itinerary']['childrensCount'];
    var reviews= jsonObject['itinerary']['reviews'];
    var rooms = jsonObject['itinerary']['rooms'];
    itineraryAddress=itineraryDetails["address"];
    $("#hotel-name-address>h1").text(itineraryName);

    $("#hotel-address>p").text(itineraryAddress);
    var imageFrames = $(".mySlides img");
    for (var imageNumber = 0; imageNumber < imageFrames.length; imageNumber++)
    {
        (imageFrames[imageNumber]).src= itineraryDetails['imageUrl'][imageNumber];

    }
    var ratingDisplay = $("#rating");
    var amenitiesDisplay = $("#amenities");
    var amenitiesList = "<ul><li>" + itineraryDetails['amenities'][0] + "</li><li>" + itineraryDetails['amenities'][1] + "</li><li>" + itineraryDetails['amenities'][2] + "</li><li>" + itineraryDetails['amenities'][3]+"</li></ul>";
    for (var starCount = 0; starCount < itineraryDetails['starRating']; starCount++)
    {
        if ((starCount - itineraryDetails['starRating']) % 1 == 0)
        {
            var star = "<img class='rating-stars' src='../resources/star.png'/>";
        }
        else
        {
            var star = "<img class='rating-stars' src='../resources/star-half-empty.png'/>";
        }
        
        ratingDisplay.append(star);
    }
    amenitiesDisplay.append(amenitiesList);
    $("#check-in-date").text((checkInDate.split("T"))[0]);
    $("#check-out-date").text((checkOutDate.split("T"))[0]);
    $("#adult-count").text(adultsCount);
    $("#child-count").text(childrenCount);
    var proceedButtonHtml = "<button type='button' class='proceed-button'>Proceed To Book</button>";
    $("#booking-details>form>fieldset:last").append(proceedButtonHtml);
    var roomList = "<select class='control-button' name=\"roomType\" id=\"rooms\">";
    for (var roomCount = 0; roomCount < rooms.length; roomCount++)
    {
        roomMapping[roomCount] = rooms[roomCount]['roomId'];
        roomList += "<option value=\""+roomCount + "\">" + (parseInt(roomCount)+1) + ". " + rooms[roomCount]['description']+"</option>";
    }
    roomList += "</select >";
    $("#rooms-field-set").append(roomList);
    if(reviews!=null)
    {
        var reviewHtml = "";
        for (var reviewCount = 0; reviewCount < reviews.length; reviewCount++) {
            reviewHtml += "<article class=\"reviews\">" + review[reviewCount] + "</article>"
        }
        $("#reviews").append(reviewHtml);
    }
}
{
    var callerSessionId;
    var roomMapping = new Object();
    var slideIndex = 1;
    var itineraryDetails;
    var maps_API_key = "AIzaSyABK2PpnRa8xpSBogGa1qHBkro3RDpDEvM";
}
function updatePrice(selectedRoomIndex) {
    $("#price").text("Loading ...");
    var roomPricingRQ = {
        "CallerSessionId": callerSessionId,
        "RoomId": roomMapping[selectedRoomIndex]
    };
    var serviceRQ = {
        "ServiceName": "HotelPricing",
        "JsonRequest": JSON.stringify(roomPricingRQ)
    };
    $.ajax({
        type: 'post',
        headers:
        {
            "Content-Type": "application/json"
        },
        url: "../padharojanab/value",
        cache: false,
        data: JSON.stringify(serviceRQ),
        success: function (response) {
            $("#price").text(response.roomPrice + response.currency);
        }
    });
}
function plusSlides(n) {
    showSlides(slideIndex += n);
}
function currentSlide(n) {
    showSlides(slideIndex = n);
}
function showSlides(n) {
    var i;
    var slides = document.getElementsByClassName("mySlides");
    var dots = document.getElementsByClassName("dot");
    if (n > slides.length) { slideIndex = 1 }
    if (n < 1) { slideIndex = slides.length }
    for (i = 0; i < slides.length; i++) {
        slides[i].style.display = "none";
    }
    for (i = 0; i < dots.length; i++) {
        dots[i].className = dots[i].className.replace(" active", "");
    }
    slides[slideIndex - 1].style.display = "block";
    dots[slideIndex - 1].className += " active";
}
function setSummaryCookies()
{
    document.cookie = "sessionId=" + callerSessionId + ";path=/";
    document.cookie = "hotelName=" + itineraryName + ";path=/";
    document.cookie = "hotelAddress=" + itineraryAddress + ";path=/";
    document.cookie = "adultCount=" + $("#adult-count").text() + ";path=/";
    document.cookie = "childCount=" + $("#child-count").text() + ";path=/";
    document.cookie = "checkInDate=" + $("#check-in-date").text() + ";path=/";
    document.cookie = "checkOutDate=" + $("#check-out-date").text() + ";path=/";
    document.cookie = "roomDescription=" + $("#rooms option:selected").text(); + ";path=/";
    document.cookie = "amenities=" + $("#amenities").text() + ";path=/";
    document.cookie = "roomPrice=" + $("#price").text() + ";path=/";
}
$(document).ready(function () {
        var urlParams = new URLSearchParams(window.location.search);
        if ((urlParams).has("cid") && (urlParams).has("iid"))
        {
            callerSessionId = urlParams.get("cid");
            ItineraryId = urlParams.get("iid");
            var singleAvailRoomSearchRQ = {
                "CallerSessionId": callerSessionId,
                "ItineraryId": ItineraryId
            };
            var serviceRequest = {
                "ServiceName": "SingleAvail",
                "JsonRequest": JSON.stringify(singleAvailRoomSearchRQ)
            };
            $.ajax({
                type: 'post',
                headers:
                {
                    "Content-Type": "application/json"
                },
                url: "../padharojanab/value",
                cache: false,
                data: JSON.stringify(serviceRequest),
                 error: function () { $(document).html("<h2>SORRY NO ROOMS AVAILABLE</h2>") },
                success: function (response) {
                    listHotelDetails(response);
                    updatePrice(0);
                    $("#rooms").on('change', function () { var roomIndex = $("#rooms").val(); updatePrice(roomIndex); });
                    $(".proceed-button").on('click', function () { setSummaryCookies(); window.location.href = "checkoutpage.html"; });
                    //var latLngObj = { lat: itineraryDetails['geoCode']['latitude'], lng: itineraryDetails['geoCode']['longitude'] };
                    //var map = new google.maps.Map(document.getElementById('map-container'), { zoom: 20, center: latLngObj });
                    //var marker = new google.maps.Marker({ position: latLngObj, map: map });
                }

            })
        }
        showSlides(slideIndex);
        $("#show-on-map").on("click", showMap);
           
    }
)
}









//{
//    var callerSessionId;
//    var ItineraryId;
//}
//function sendPriceRequest() {
//    var urlParams = new URLSearchParams(window.location.search);
//    if ((urlParams).has("cid") && (urlParams).has("iid")) {
//        callerSessionId = urlParams.get("cid");
//        ItineraryId = urlParams.get("iid");
//        var singleAvailRoomSearchRQ = {
//            "CallerSessionId": callerSessionId,
//            "ItineraryId": ItineraryId
//        };
//        var serviceRQ = {
//            "ServiceName": "SingleAvail",
//            "JsonRequest": JSON.stringify(singleAvailRoomSearchRQ)
//        };
//        $.ajax({
//            type: 'post',
//            headers: { "Content-Type": "application/json" },
//            url: "../padharojanab/value",
//            cache: false,
//            data: JSON.stringify(serviceRQ),
//            success: function (response)
//            {
//                listHotelDetails(response);
//                updatePrice(0);
//                $("#rooms").on('change', function () { var roomIndex = $("#rooms").val(); updatePrice(roomIndex); });
//                $(".proceed-button").on('click', function () { setSummaryCookies(); window.location.href = "checkoutpage.html"; });
//            },
//            error: function () { $(document).html("<h2>SORRY NO ROOMS AVAILABLE</h2>") }
//        });
//    }
//}
//function initialiser()
//{
//    sendPriceRequest();
//}
//$(document).ready(initialiser)
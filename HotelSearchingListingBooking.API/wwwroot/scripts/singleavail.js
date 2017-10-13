function listHotelDetails(jsonObject)
{
    var itineraryDetails = jsonObject['itinerary']['itinerarySummary'];
    var itineraryName = itineraryDetails['name'];
    var checkInDate = jsonObject['itinerary']['checkInDate'];
    var checkOutDate = jsonObject['itinerary']['checkOutDate'];
    var adultsCount = jsonObject['itinerary']['adultCount'];
    var childrenCount = jsonObject['itinerary']['childrensCount'];
    var reviews= jsonObject['itinerary']['reviews'];
    var rooms = jsonObject['itinerary']['rooms'];
    $("#hotel-name-address>h3").text(itineraryName);
    $("#hotel-name-address>span").text(itineraryDetails["address"]);
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
    var roomList = "<select name=\"roomType\" id=\"child-count\">";
    for (var roomCount = 0; roomCount < rooms.length; roomCount++)
    {
        roomList += "<option value=\""+roomCount + "\">" + roomCount + 1 + ". " + rooms[roomCount]['description']+"</option>";
    }
    roomList += "</select >";
    $("#rooms-field-set").append(roomList);
    var reviewHtml="";
    for (var reviewCount = 0; reviewCount < reviews.length; reviewCount++)
    {
        reviewHtml+="<article class=\"reviews\">"+ review[reviewCount] + "</article>"
    }
    $("#reviews").append(reviewHtml);
}
var slideIndex = 1;
showSlides(slideIndex);
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
                success: function (response) { listHotelDetails(response); }

            })
        }
        $("#get-current-price").on("click", function () {

        })
    }
)
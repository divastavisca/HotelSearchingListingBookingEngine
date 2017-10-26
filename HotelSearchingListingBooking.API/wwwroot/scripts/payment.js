{
    var sessionId;
    var summaryFieldsPlaceHolders;
    var summaryFieldValues;
    var guestDetailsHtml = "<fieldset class='contact-details'> <h3>Contact Information</h3> <div> <label for='first name'>First name:<span class='mandatory'><sup>*</sup></span> </label> <input class='first-name' type='text' name='Contact name' placeholder='First name' required> <label for='first name'>Middle name:</label> <input class='second-name' type='text' name='Contact name' placeholder='Middle name' required> <label for='first name'>Last name:<span class='mandatory'><sup>*</sup></span> </label> <input class='last-name' type='text' name='Contact name' placeholder='Last name' required> <label>Gender</label> <select class='gender' required> <option value='m'>Male</option> <option value='f'>Female</option> <select/> <p class='name-validation '>Please enter first name,middle name and last name using letters</p></div><div> <label>Email</label> <input class='email' type='email' required/> <label for='Country name'>Country name<span class='mandatory'><sup>*</sup></span> </label> <select required class='country-name'></select> <p class='name-validation'>Please enter first and last name using letters</p></div><div> <label for='Contact number'>Contact number<span class='mandatory'><sup>*</sup></span> </label> <input class='contact-number' type='number' name='Contact number' placeholder='Mobile number' required> <p class='name- validation '>Please enter valid number </p></div></fieldset><fieldset class='personal-details'> <h1>Personal Details</h1> <div> <label for='DateOfBirth'>Date Of Birth</label> <input class='dob' type='date' name='DOB'> </div></fieldset>";
    var countriesOptions = "<option value='India'>India</option>";
    var guestCount;
}
function getAge(date)
{
    var calculatedAge = new Date().getFullYear() - (date.split('-'))[0];
    return calculatedAge;
}
function getGuestField()
{
    var response = "[";
    var guestForms = $(".guest-form-container");
    for (var guest = 0; guest < guestCount; guest++)
    {
        var firstName = $(guestForms[guest]).find(".first-name").val();
        var middleName = $(guestForms[guest]).find(".second-name").val();
        var lastName = $(guestForms[guest]).find(".last-name").val();
        var email = $(guestForms[guest]).find(".email").val();
        var gender = $(guestForms[guest]).find(".gender").val();
        var type;
        var nameObject = "{\"FirstName\":" +'"' +firstName+'"' + "," +"\"MiddleName\":" + '"'+middleName +'"'+ "," + "\"LastName\":" + '"'+lastName +'"'+ "}";
        var dob=$(guestForms[guest]).find(".dob").val();
        var age = getAge(dob);
        if (age < 18) { type = "Child"; }
        else { type = "Adult"; }
        response +=
            "{\"Name\":" + nameObject +
            ',\"DateOfBirth\":' +'"'+ dob+'"' +
            ",\"Age\":" +'"' +age + '"' +
        ",\"Email\":" + '"' + email + '"' +
        ",\"Gender\":" + '"' + gender + '"' +
        ",\"Type\":" + '"' + type + '"'+
            "},";
    }
    response = response.substring(0, response.length - 1);
    response += "]";
    return response;
}
function sendBookRequest() {
    var paymentDetails = $("#payment>input");
    var addressParts = $("#address-info>input");
    var summaryFields = $(".summary-field");
    var priceDetails = summaryFields[8].value;
    var HotelProductBookRQ = "{" +
        '"CallerSessionId":' + '"' + sessionId + '"' + ',' +
        '"Guests":' + getGuestField() + ',' +
        '"PaymentDetails":' +
        '{' +
        '"Price":' + '"' + priceDetails + '"' + ',' +
        '"BillingAddress":' +
        '{' +
        '"AddressLine1":' + '"' + addressParts[0].value + '"' + ',' +
        '"AddressLine2":' + '"' + addressParts[1].value + '"' + ',' +
        '"AddressContext":' + '"Address"' + ',' +
        '"City":' + '"' + addressParts[2].value + '"' + ',' +
        '"State":' + '"' + addressParts[3].value + '"' + ',' +
        '"Country":' + '"India"' + ',' +
        '"ZipCode":' + '"' + addressParts[4].value + '"' + ',' +
        '"PhoneNumber":' + '"' + addressParts[5].value + '"' + '},' +
        '"CreditCardDetails":' +
        '{' +
        '"NameOnCard":' + '"' + paymentDetails[0].value + '"' + ',' +
        '"CardNumber":' + '"' + paymentDetails[1].value + '"' + ',' +
        '"Cvv":' + '"' + paymentDetails[2].value + '"' + ',' +
        '"Code":' + '"VI"' + ',' +
        '"CardName":' + '"VISA"' + ',' +
        '"Expiry":' + '"' + $("#Year").val() + "-" + $("#Month").val() + "-" + "01" + '"' + ',' +
        '"IsThreeDAuth":' + '"true"' + '}' + '}' + '}';
    var IEngineServiceRQ =
        {
            "ServiceName": "ProcessProductBooking",
            "JsonRequest": HotelProductBookRQ
        };
    $.ajax({
        type: 'post',
        headers:
        {
            "Content-Type": "application/json"
        },
        url: "../padharojanab/value",
        cache: false,
        data: JSON.stringify(IEngineServiceRQ),
        //beforeSend: showLoadingImage,
        success: function (response) { console.log(response); },
        error: function (response) { console.log("error :" + response); }
    });
}
function initialiser() {
    
    summaryFieldsPlaceHolders = $(".summary-field");
    summaryFieldValues = (document.cookie).split(";");
    {
        sessionId = ((summaryFieldValues[0]).split("="))[1];

        summaryFieldsPlaceHolders[0].value = (summaryFieldValues[1].split("="))[1];
        $(summaryFieldsPlaceHolders[0]).attr('size', $(summaryFieldsPlaceHolders[0]).val().length);

        summaryFieldsPlaceHolders[1].value = (summaryFieldValues[2].split("="))[1];
        $(summaryFieldsPlaceHolders[1]).attr('size', $(summaryFieldsPlaceHolders[1]).val().length);

        summaryFieldsPlaceHolders[2].value = (summaryFieldValues[3].split("="))[1];
        $(summaryFieldsPlaceHolders[2]).attr('size', $(summaryFieldsPlaceHolders[2]).val().length);

        summaryFieldsPlaceHolders[3].value = (summaryFieldValues[4].split("="))[1];
        $(summaryFieldsPlaceHolders[3]).attr('size', $(summaryFieldsPlaceHolders[3]).val().length);

        summaryFieldsPlaceHolders[4].value = (summaryFieldValues[5].split("="))[1];
        $(summaryFieldsPlaceHolders[4]).attr('size', $(summaryFieldsPlaceHolders[4]).val().length);

        summaryFieldsPlaceHolders[5].value = (summaryFieldValues[6].split("="))[1];
        $(summaryFieldsPlaceHolders[5]).attr('size', $(summaryFieldsPlaceHolders[5]).val().length);

        summaryFieldsPlaceHolders[6].value = (summaryFieldValues[7].split("="))[1];
        $(summaryFieldsPlaceHolders[6]).attr('size', $(summaryFieldsPlaceHolders[6]).val().length);

        summaryFieldsPlaceHolders[7].value = (summaryFieldValues[8].split("="))[1];
        $(summaryFieldsPlaceHolders[7]).attr('size', $(summaryFieldsPlaceHolders[7]).val().length);

        summaryFieldsPlaceHolders[8].value = (summaryFieldValues[9].split("="))[1];
        $(summaryFieldsPlaceHolders[8]).attr('size', $(summaryFieldsPlaceHolders[8]).val().length);
    }
    guestCount = parseInt((summaryFieldValues[3].split("="))[1]) + parseInt((summaryFieldValues[4].split("="))[1]);
    var guestFormHtml="";
    for (var guest = 0; guest < guestCount; guest++)
    {
        guestFormHtml += "<div class='guest-form-container'><h1> Guest " + (parseInt(guest) + 1) + " Details </h1>";
        guestFormHtml += guestDetailsHtml+"</div>";
    }
    $("#guest-details").html(guestFormHtml);
    $(".country-name").append(countriesOptions);
    $("input").on('change keyup', function () { $(this).siblings('p').css("display", "block")});
    $("#complete-booking").on("click", sendBookRequest);
}
$(document).ready(initialiser);


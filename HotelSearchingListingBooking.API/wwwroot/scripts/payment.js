{
    var sessionId;
    var summaryFieldsPlaceHolders;
    var summaryFieldValues;
    var guestDetailsHtml = "<fieldset id='contact-details'><h3 id='contact'>Contact Information</h3><div><label for='first name'>First name:<span class='mandatory'><sup>*</sup></span></label><input type='text' name='Contact name' placeholder='First name' required><label for='first name'>Middle name:</label><input type='text' name='Contact name' placeholder='Middle name' required><label for='first name'>Last name:<span class='mandatory'><sup>*</sup></span></label><input type='text' name='Contact name' placeholder='Last name' required><label>Gender</label><select required><option value='m'>Male</option><option value='f'>Female</option><select/><p class='name- validation '>Please enter first name,middle name and last name using letters</p></div><div><label>Email</label><input type='email' required /><label for='Country name'>Country name<span class='mandatory'><sup>*</sup></span></label><select required class='country-name'></select><p class='name-validation'>Please enter first and last name using letters</p></div><div><label for='Contact number'>Contact number<span class='mandatory'><sup>*</sup></span></label><input type='number' name='Contact number' placeholder='Mobile number' required><p class='name- validation '>Please enter valid number </p></div></fieldset><fieldset id='personal-details'><h1>Personal Details</h1><div><label for='DateOfBirth'>Date Of Birth</label><input type='date' name='DOB'></div><div><h3>Address:</h3><fieldset style='overflow: auto'><label for='Address line 1'>Address line 1</label><input type='text' name='Address line 1' placeholder='Address line 1 '><label for='Address line 2'>Address line 2</label><input type='text' name='Address line 2' placeholder='Address line 2 '><label for='City'>City</label><input type='text' name='city name' placeholder='city'><label for='state'>State</label><input type='text' name='state name' placeholder='state'><label for='ZipCode'>Zip Code</label><input type='number' name='ZipCode' placeholder='Zip Code'><p class='name-validation'>Please enter valid 6-digits Code </p></fieldset></div></fieldset>";
    var countriesOptions = "<option value='India'>India</option>";
    var guestCount;
}
function getAge(date)
{
    var calculatedAge = new Date().getFullYear() - (dob.split('-'))[0];
    return calculatedAge;
}
function getGuestField()
{
    var response = "[";
    var guestForms = $(".guest-form-container");
    for (var guest = 0; guest < guestCount; guest++)
    {
        var firstName = $(guestForms[guest]).children("fieldset:first-child>div:first-child>input:nth-child(1)").val();
        var middleName = $(guestForms[guest]).children("fieldset:first-child>div:first-child>input:nth-child(2)").val();
        var lastName = $(guestForms[guest]).children("fieldset:first-child>div:first-child>input:nth-child(3)").val();
        var email = $(guestForms[guest]).children("fieldset:first-child>div:nth-child(2)>input:first-child").val();
        var gender = $(guestForms[guest]).children("fieldset:first-child>div:first-child>select").val();
        var type;
        var nameObject = "{\"FirstName\":" + firstName + "," + "\"MiddleName\":" + middleName + "," + "\"LastName\":" + lastName + "," + "}";
        var dob;
        var dateParts = $(guestForms[guest]).children("fieldset:nth-child(2)>div:first-child>input").val();
        dateParts = dateParts.split("-");
        dob = dateParts[2] + '-' + dateParts[0] + '-' + dateParts[1];
        var age = getAge(dob);
        if (age < 18) { type = "Child"; }
        else { type = "Adult"; }
        response +=
            "{\"Name\":" + nameObject +
            "\"DateOfBirth\":" + dob +
            "\"Age\":" + age +
            "\"Email\":" + email +
            "\"Gender\":" + gender +
            "\"Type\":" + type +
            +"},";
    }
    response = response.substring(0, response.length - 1);
    response += "]";
    return response;
}

function sendBookRequest()
{
    var HotelProductBookRQ =
        {
            "CallerSessionId": CallerSessionId,
            "Guest": getGuestField(),
            "PaymentDetails":
            {
                "Amount": ($("#booking-summary>input:nth-child(9)").val()).substring(0,-3),
                "Currency": "USD",
                "BillingAddress":
                {
                    "AddressLine1":$("#billing-address>input:nth-child(1)"),
                    "AddressLine2": $("#billing-address>input:nth-child(2)"),
                    "AddressContext": "Address",
                    "City": $("#billing-address>input:nth-child(3)"),
                    "State": $("#billing-address>input:nth-child(4)"),
                    "Country": "India",
                    "ZipCode": $("#billing-address>input:nth-child(5)"),
                    "PhoneNumber": $("#billing-address>input:nth-child(6)"),
                },
                "CreditCardDetails":
                {
                    "NameOnCard": $("#payment>input:first-child").val(),
                    "CardNumber": $("#payment>input:nth-child(2)").val(),
                    "Cvv": $("#payment>input:nth-child(3)").val(),
                    "Code": "VI",
                    "CardName":"VISA",
                    "Expiry": $("#Year").val() + "-"+$("#Month").val()+"-"+"01",
                    "IsThreeDAuth": true
                }
            }
    }

}

function initialiser() {
    
    summaryFieldsPlaceHolders = $(".summary-field");
    summaryFieldValues = (document.cookie).split(";");
    {
        sessionId = summaryFieldValues[0];

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


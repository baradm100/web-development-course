var rowNum = 1
var openingHours = [];
var name, city, street, buildingNumber, lon, lat
var days = ["Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"]

$("#add_day").click(function () {
    if (rowNum < 7) {
        $("#table_body").append('<tr><th scope="row"><select class="form-select day_selection" id="day_selection' + rowNum + '" aria-label="Default select example">
            + for (var i = 0; i < days.length; i++) {
                /<option value=""$i>days[i]</option>
            } +/</select > <span class="text-danger d-none" id="day_error' "$rowNum">Error</span></th>
                <td><div scope="row"><input id="open' + rowNum + '" class="form-select" type="time"/><span class="text-danger d-none" id="open_error' + rowNum + '">Error</span></div ></td><td><div scope="row"><input id ="close' + rowNum +'" class= "form-select" type = "time" /><span class="text-danger d-none" id="close_error'+rowNum+'">Error</span></div ></td> </tr > ')
rowNum++
        }
    });

$("#create").click(function () {

    if (validHour() && validDay() && validBranch()) {
        var newBranch = {
            "__RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val(),
            'Name': name,
            'Address': {
                'City': city,
                'Street': street,
                'BuildingNumber': buildingNumber,
                'Longitude': lon,
                'Latitude': lat
            },
            'OpeningHours': openingHours
        }

        $.ajax({
            type: 'POST',
            url: "/Branches/Create",
            dataType: 'json',
            data: newBranch
        }).done(function () {
            console.log("done")
        });

    }
    else {
        // clear opening hour
        openingHours = [];
    }
});

function validHour() {
    console.log("validHour")
    var rowNumberIndex = rowNum - 1;
    while (rowNumberIndex >= 0) {
        if (!$("#open_error" + rowNumberIndex).hasClass("d-none")) {
            $("#open_error" + rowNumberIndex).addClass("d-none")
        }
        if (!$("#close_error" + rowNumberIndex).hasClass("d-none")) {
            $("#close_error" + rowNumberIndex).addClass("d-none")
        }

        var openHour = document.getElementById("open" + rowNumberIndex).value
        var closeHour = document.getElementById("close" + rowNumberIndex).value

        if (!openHour || !closeHour || openHour > closeHour) {
            console.log("all bad")
            $("#open_error" + rowNumberIndex).removeClass("d-none")
            $("#close_error" + rowNumberIndex).removeClass("d-none")

            // rasie some error
            return false
        }

        var item = {}
        item["DayOfWeek"] = document.getElementById("day_selection" + rowNumberIndex).value
        item["Open"] = openHour
        item["Close"] = closeHour
        openingHours.push(item)

        rowNumberIndex--
    }
    return true
}

function validDay() {
    console.log("validDay")

    var days = document.getElementsByClassName("day_selection")
    var hlper = [0, 0, 0, 0, 0, 0, 0];

    for (var i = 0; i < days.length; i++) {
        console.log("day val " + days[i].value)
        hlper[days[i].value] += 1
    }

    hlper.forEach(function (item, index) {
        if (item > 1) {
            //reaise toast with index
            console.log("validDay bad")
            return false
        }
    });

    return true
}

function validBranch() {
    let result = true
    name = $("#name").val()
    city = $("#city").val()
    street = $("#street").val()
    buildingNumber = document.getElementById("building-number").value
    lon = $("#longitude").val()
    lat = $("#latitude").val()


    if (name.length < 2 || name.length > 50) {
        result = false;
        $("#name_error").removeClass("d-none")
    }
    else if (!($("#name_error").hasClass("d-none"))) {
        $("#name_error").addClass("d-none")
        console.log("add d-none to name_error")
    }

    if (city.length < 2) {
        result = false;
        $("#city_error").removeClass("d-none")
    }
    else if (!$("#city_error").hasClass("d-none")) {
        $("#city_error").addClass("d-none")
    }

    if (street.length < 2) {
        result = false;
        $("#street_error").removeClass("d-none")
    }
    else if (!$("#street_error").hasClass("d-none")) {
        $("#street_error").addClass("d-none")
    }

    if (buildingNumber == null || buildingNumber < 1 || buildingNumber > 10000) {
        result = false;
        $("#number_error").removeClass("d-none")
    }
    else if (!$("#number_error").hasClass("d-none")) {
        $("#number_error").addClass("d-none")
    }

    if (lon.length < 2) {
        result = false;
        $("#longitude_error").removeClass("d-none")
    }
    else if (!$("#longitude_error").hasClass("d-none")) {
        $("#longitude_error").addClass("d-none")
    }

    if (lat.length < 2) {
        result = false;
        $("#latitude_error").removeClass("d-none")
    }
    else if (!$("#latitude_error").hasClass("d-none")) {
        $("#latitude_error").addClass("d-none")
    }

    // figure a way to use the Asp.Net validation
    return result
}
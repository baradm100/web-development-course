$(function () {
    var openingHours = [];
    var name, city, street, buildingNumber, lon, lat, branchId, addressId
    var days = ["Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"]

    // Add new day to the branch opening day
    $("#add_day").click(function () {
        var option = initDays()

        // get the amount of days we already have
        var rowNum = $("tr").length - 1

        if ($("tr").length < 8) {
            $("#table_body").append('<tr id="tr_' + rowNum
                + '"><th scope="row"><select class="form-select day_selection" id="day_selection_' + rowNum
                + '" aria-label="Default select example">' + option
                + '</select><span class="text-danger d-none" id="day_error_' + rowNum
                + '">Error</span></th><td><div scope="row"><input id="open_' + rowNum
                + '" class="form-select" type="time"/><span class="text-danger d-none" id="open_error_' + rowNum
                + '">Error</span></div></td><td><div scope="row"><input id ="close_' + rowNum
                + '" class= "form-select" type="time"/><span class="text-danger d-none" id="close_error_' + rowNum
                + '">Error</span></div ></td></tr>')
        }
        else {
            console.log("too many days")
        }
    });

    // init the selcet days options 
    function initDays() {
        let lst = "";
        for (var i = 0; i < days.length; i++) {
            lst += ("<option value =" + i + ">" + days[i] + "</option>")
        }

        return lst
    };


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
                data: newBranch,
                success: function (response) {
                    if (!response.success) {
                        if ($("#create").siblings("#existWarning").length == 0)
                            $("#create").parent("div").append("<div class='alert alert-danger' id='existWarning'>this branch already exist</div>");
                    } else {
                        $("#create").siblings("#existWarning").remove();
                        window.location.href = "/branches";
                    }
                }
            }).done(function () {
                console.log("done")
            });

        }
        else {
            // clear opening hour
            openingHours = [];
        }
    });

    $("#save").click(function () {
        if (validHour() && validDay() && validBranch()) {
            var newBranch = {
                "__RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val(),
                "Id": branchId,
                'Name': name,
                'Address': {
                    'Id': addressId,
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
                url: "/Branches/Edit",
                dataType: 'json',
                data: newBranch,
                fail: function () {
                    console.log("fail");
                    alert("failed")
                },
                success: function () {
                    console.log("success");
                    location.href = "/Branches"
                }
            });
        }
        else {
            // clear opening hour
            openingHours = [];
        }
    });

    $("#deleteBranchBtn").click(function () {
        console.log(clicked);
        var BranchId = $(this).parent("div").Id;
        var token = $('input[name="__RequestVerificationToken"]').val();
        var url = "/Branches/DeleteConfirmed?id=" + BranchId + "&__RequestVerificationToken=" + token;
        window.location.href = url;
        return false;
    });

    $("#delete_day").click(function () {
        var lastRow = $("tr").length - 1
        if (lastRow > 1) {
            $("#tr_" + (lastRow - 1)).remove()
        }
    });

    function validHour() {
        console.log("validHour")
        var rowNumberIndex = $("tr").length - 2;

        while (rowNumberIndex >= 0) {
            if (!$("#open_error_" + rowNumberIndex).hasClass("d-none")) {
                $("#open_error_" + rowNumberIndex).addClass("d-none")
            }
            if (!$("#close_error_" + rowNumberIndex).hasClass("d-none")) {
                $("#close_error_" + rowNumberIndex).addClass("d-none")
            }

            var openHour = document.getElementById("open_" + rowNumberIndex).value
            var closeHour = document.getElementById("close_" + rowNumberIndex).value

            if (!openHour || !closeHour || openHour > closeHour) {
                console.log("all bad")
                $("#open_error_" + rowNumberIndex).removeClass("d-none")
                $("#close_error_" + rowNumberIndex).removeClass("d-none")

                // rasie some error
                return false
            }

            var item = {}
            var dayId = $("#day_selection_" + rowNumberIndex).attr("dbid")

            if (dayId != null) {
                item["Id"] = dayId
            }

            item["DayOfWeek"] = document.getElementById("day_selection_" + rowNumberIndex).value
            item["Open"] = openHour
            item["Close"] = closeHour
            item["BranchId"] = branchId
            openingHours.push(item)

            rowNumberIndex--
        }
        return true
    }

    function validDay() {
        console.log("validDay")

        var daysIndex = document.getElementsByClassName("day_selection")
        var hlper = [0, 0, 0, 0, 0, 0, 0];

        for (var i = 0; i < daysIndex.length; i++) {
            hlper[daysIndex[i].value] += 1
        }
        hlper.forEach(function (item, index) {
            if (item > 1) {
                //raise toast with index
                console.log("validDay bad " + index)
                alert("it is not possible to repeat the same day twice - " + days[index])
                return false
            }
        });

        return true
    }

    function validBranch() {
        let result = true
        name = $(".name").val()
        branchId = $(".name").attr("id")
        city = $(".city").val()
        addressId = $(".city").attr("id")
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

        return result
    }
});
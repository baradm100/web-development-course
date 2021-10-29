
$(function () {
    GetSummary()
    getUserInfo()
    validationHelper()

    //#region Listeners
    
    $(".delivery").click(function () {
        deliveryOption($(this).attr("id"))
    })

    function validationHelper() {
        'use strict'
        // Fetch all the forms we want to apply custom Bootstrap validation styles to
        var forms = document.querySelectorAll('.needs-validation')

        // Loop over them and prevent submission
        Array.prototype.slice.call(forms)
            .forEach(function (form) {
                form.addEventListener('input', function (event) {
                    if (!form.checkValidity()) {
                        event.preventDefault()
                        event.stopPropagation()
                    }

                    form.classList.add('was-validated')
                    allowPurchase()
                }, false)
            })
        
    }

    $(".orderSummaryClose").click(function () {
        var url = "/Home/Index"
        window.location.href = url;
        return false;
    })

    $("#checkoutModalClose").click(function () {
       cleanCheckoutModal()
    })

    //#endregion

    //#region Helpers

    function deliveryOption(id) {
        hasClass = false
        if ($("#" + id).hasClass("delivery-border")) {
            hasClass = true
        }

        $("#delivery_1").removeClass("delivery-border")
        $("#delivery_53").removeClass("delivery-border")

        if (!hasClass) {
            $("#" + id).addClass("delivery-border")
        }
        allowPurchase()
        updatePrice()
    }


    function updatePrice() {
        deliveryPrice = 0;
        if ($(".delivery-border")[0]) {
            let id = $($(".delivery-border")[0]).attr("id")
            id = id.split("_")
            if (id[1] > 1) {
                deliveryPrice = Number(id[1])
            }
            $("#delivery_price").text(deliveryPrice)
        }

        $("#total-price").text(Number((Number(deliveryPrice) + Number($("#subtotal-price").val())).toFixed(2)))
        $("#total-price").val(Number((Number(deliveryPrice) + Number($("#subtotal-price").val())).toFixed(2)))

    }


    function updateInfo(name, email, phone) {
        $("#floatingName").val(name)
        $("#floatingEmail").val(email)
        $("#floatingPhone").val(phone)
    }

    function allowPurchase() {
        allow= true

        $("form").each(function () {
            if (!(this).checkValidity()) {
                allow = false
            }
        });

        if (!$(".delivery-border")[0]) {
            allow = false
        }

        if (allow) {
            if (isDateValid()) {
                $("#place-order").removeAttr("disabled")

                if (!$("#floatingMMFeedback").hasClass("visually-hidden")) {
                    $("#floatingMMFeedback").addClass("visually-hidden")
                }

            }
            else {
                $("#floatingMMFeedback").removeClass("visually-hidden")
            }
        }
        else {
            if (!$("#place-order").attr("disabled")) {
                $("#place-order").prop('disabled', true);
            }
            if (!$("#floatingMMFeedback").hasClass("visually-hidden")) {
                $("#floatingMMFeedback").addClass("visually-hidden")
            }
        }
    }

    function isDateValid() {
        let today = new Date();
        month = $("#floatingMM").val()
        year = $("#floatingYY").val()

        let isValid = true

        if (((today.getFullYear() % 100) > year)) {
            isValid = false
        }
        if (((today.getMonth() + 1) > month) && ((today.getFullYear() % 100) == year)) {
            isValid = false
        }

        return isValid;
    }

    //#endregion

    //#region AJAX

    function getUserInfo() {
        $.ajax({
            url: "/Users/Info",
            type: 'GET',
            dataType: 'json',
            data: null,
            fail: function (xhr, textStatus, errorThrown) {
                Success = false;
                alert("Something went wrong in server")
            },
            success: function (response) {
                Success = true;
                if (response != null) {
                    updateInfo(response.data.name, response.data.email, response.data.phone)
                }
            },
            error: function (result) {
                console.log(result);
                return false
            },
            timeout: 5000,
        });
    }

    function GetSummary() {
        $.ajax({
            url: "/Orders/GetSummary",
            type: 'GET',
            dataType: 'json',
            data: null,
            fail: function (xhr, textStatus, errorThrown) {
                Success = false;
                alert("Something went wrong in server")
            },
            success: function (response) {
                Success = true;
                if (response != null) {
                    let subtotalPrice = Number(Number(response.data.totalPrice).toFixed(2))
                    $("#subtotal-price").text(subtotalPrice)
                    $("#subtotal-price").val(subtotalPrice)
                    updatePrice()
                }
            },
            error: function (result) {
                console.log(result);
                return false
            },
            timeout: 5000,
        });
    }

    function GetBranches() {
        $.ajax({
            url: "/Branches/GetBranches",
            type: 'GET',
            dataType: 'json',
            data: null,
            fail: function (xhr, textStatus, errorThrown) {
                Success = false;
                alert("Something went wrong in server")
            },
            success: function (response) {
                if (response.success) {
                    Success = true;
                    // TBD
                }
                else
                {
                    Success = false;
                    alert("Something went wrong in server")
                }
     
            },
            error: function (result) {
                console.log(result);
                return false
            },
            timeout: 5000,
        });
    }

    $("#place-order").click(function () {
        $.ajax({
            url: "/Orders/PlaceOrder",
            type: 'Post',
            dataType: 'json',
            data: {
                "totalPrice": $("#total-price").val(),
                "deliveryOption": $(".delivery-border").attr("id"),
                "phone" : $("#floatingPhone").val(),
                "address": {
                    "city": $("#floatingCity").val(),
                    "street": $("#floatingStreet").val(),
                    "buildingNumber": $("#floatingNumber").val()
                },
            },
            fail: function (xhr, textStatus, errorThrown) {
                Success = false;
                alert("Something went wrong in server " + textStatus)
                console.log("fail " + textStatus);
                cleanCheckoutModal()

            },
            success: function (response) {
                if (response.success) {
                    Success = true;
                    cleanCheckoutModal()
                    openOrderApprovment(response)
                }
                else {
                    alert("Something went wrong in server " + response.textStatus)
                    cleanCheckoutModal()
                }

            },
            error: function (result) {
                console.log("error");
                console.log(result);
                cleanCheckoutModal()
                return false
            },
            timeout: 5000,
        });
    })

    //#endregion

    //#region Close Modal
    
    function cleanCheckoutModal() {
        $('#checkoutModal').modal('hide');
        $("#floatingCity").val(""),
        $("#floatingStreet").val(""),
        $("#floatingNumber").val("")
        $("#floatingCreditNumber").val("")
        $("#floatingMM").val("")
        $("#floatingYY").val("")
        $("#floatingCVV").val("")
        $(".needs-validation").removeClass("was-validated")
        $(".delivery").removeClass("delivery-border")

        if (!$("#place-order").attr("disabled")) {
            $("#place-order").prop('disabled', true);
        }

        if (!$("#floatingMMFeedback").hasClass("visually-hidden")) {
            $("#floatingMMFeedback").addClass("visually-hidden")
        }

        $("#total-price").text(Number(Number($("#subtotal-price").val())).toFixed(2))
        $("#total-price").val(Number(Number($("#subtotal-price").val())).toFixed(2))
        $("#delivery_price").text("0")   

        GetSummary();

    }

    //#endregion

    //#region Open Modal
    function openOrderApprovment(response) {
        $("#orderNumer").text(response.data.orderId)
        $("#total_price_sum").text(response.data.price)
        $("#orderSummary").modal('show')
    }

    //#endregion
});
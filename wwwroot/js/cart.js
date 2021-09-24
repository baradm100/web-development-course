let currecny = ["$", "₪", "€","£"]

$(function () {
    GetSummary()
    initAmountListeners()
    initDeleteListener()
   

    function initAmountListeners() {
        $(".amount").change(function () {
            var index = $(this).attr("id")
            var modelId = $(this).attr("modelId")

            updateAmount(modelId, index)
        })
    }

    function GetSummary() {
        console.log("GetSummary")
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
                        $("#summary-price").text(Number(Number(response.data.midPrice).toFixed(2)) + " " + currecny[(response.data.currencyIndex)])
                        $("#summary-discount").text(Number(Number(response.data.saving).toFixed(2)) + " " + currecny[(response.data.currencyIndex)])
                        $("#summary-total").text(Number(Number(response.data.totalPrice).toFixed(2)) + " " + currecny[(response.data.currencyIndex)])
                    }
                },
                error: function (result) {
                    console.log(result);
                    return false
                },
                timeout: 5000,
        });
    }

    function updateAmount(id, index) {
        console.log("updateAmount")
        $.ajax({
            url: "/Orders/UpdateAmount",
            type: 'Post',
            dataType: 'json',
            data: {
                "id": id,
                "amount": $("#" + index).val()
            },
            fail: function (xhr, textStatus, errorThrown) {
                Success = false;
                alert("Something went wrong in server " + textStatus)
            },
            success: function (response) {
                Success = true;
                GetSummary()
                getItemFinalPrice(id, index)
            },
            error: function (result) {
                console.log(result);
                return false
            },
            timeout: 5000,
        });
    }


    function getItemFinalPrice(orderId, rowIndex) {
        console.log("orderid" + orderId)
        $.ajax({
            url: "/Orders/GetItemFinalPrice?orderId=" + orderId,
            type: 'GET',
            dataType: 'json',
            data: null,
            fail: function (xhr, textStatus, errorThrown) {
                Success = false;
                alert("Something went wrong in server")
            },
            success: function (response) {
                Success = true;
                $("#total-price_" + rowIndex).text(Number(Number(response.data.totalPrice).toFixed(2)) + " " + currecny[(response.data.currecnyIndex)])

            },
            error: function (result) {
                console.log(result);
                return false
            },
            timeout: 5000,
        });
    }


    function initDeleteListener() {
        $(".deleteButton").click(function () {
            var index = $(this).attr("id")
            var modelId = $(this).attr("modelId")
            deleteItem(modelId, index)
        })
    }

    function deleteItem(modelId, index) {
        $.ajax({
            url: "/Orders/DeleteByUser?orderId="+modelId,
            type: 'GET',
            dataType: 'json',
            data: null,
            fail: function (xhr, textStatus, errorThrown) {
                Success = false;
                alert("Something went wrong in server " + textStatus)
            },
            success: function (response) {
                Success = true;
                removeCard(index)
                GetSummary()
            },
            error: function (result) {
                console.log(result);
                return false
            },
            timeout: 5000,
        });
    }

    function removeCard(index) {
        $("#card_" + index).remove();
    }
});

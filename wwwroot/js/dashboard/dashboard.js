$(function () {
    var $dealsTableBody = $(".dealsTableBody");
    var formData = new FormData();
    var token = $('input[name="__RequestVerificationToken"]').val();
    formData.append("__RequestVerificationToken", token);

    $.ajax({
        url: "/orders/json/",
        type: 'GET',
        dataType: 'json',
        processData: false,
        contentType: false,
        data: formData,
        success: function (response) {
            if (response.success == true) {
                Success = true;
                for (let i = 0; i < response.orders.length; i++) {
                    var order = response.orders[i];
                    var closedDeal = order.isCart == true ? "false" : "true";
                    var orders = "<tr><th scope='row'>" + order.id + "</th><td>" + order.firstName + " " + order.lastName + "</td><td>" + order.date + "</td><td>"
                        + order.name + "</td><td>" + order.totalPrice + "</td><td>" + closedDeal + "</td></tr>"
                    $dealsTableBody.append(orders);
                };
            }
        },
        error: function (result) {
            console.log(result);
            return false;
        },
        timeout: 5000,
    });

});
$(function () {
    var $dealsTableBody = $(".dealsTableBody");
    var $usersTableBody = $(".usersBodyTable");
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
                var a = response.orders;
                a.sort((a, b) => parseInt(a.id) - parseInt(b.id));
                for (let i = 0; i < response.orders.length; i++) {
                    var order = a[i];
                    var closedDeal = order.isCart == true ? "false" : "true";
                    var orders = "<tr><th scope='row'>" + order.id + "</th><td>" + order.firstName + " " + order.lastName + "</td><td>" + order.date + "</td><td>"
                        + order.name + "</td><td>" + order.amount + "</td><td>" + order.totalPrice + "</td><td>" + closedDeal + "</td></tr>"
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

    var selectOption = (selected, id) => {
        var roles = { "Admin": false, "Editor": false, "Client": false }
        var select = "<select class='form-select-sm role' id='" + id + "'aria-label='Disabled' disabled>"
        select += "<option value=" + selected + "selected>" + selected + "</option>"
        roles[selected] = true;
        for (var i = 0; i < 3; i++) {
            var role = Object.keys(roles);
            if (!roles[role[i]])
                select += "<option value=" + role[i] + ">" + role[i] + "</option>"
        }
        return select
    }

    $.ajax({
        url: "/users/json/",
        type: 'GET',
        dataType: 'json',
        processData: false,
        contentType: false,
        data: formData,
        success: function (response) {
            if (response.success == true) {
                Success = true;
                var users = response.users;
                console.log(users);
                for (let i = 0; i < users.length; i++) {
                    var user = users[i];
                    var select = selectOption(user.userType, user.id);
                    var orders = "<tr class='tableRow'><th scope='row'>" + user.id + "</th><td>" + user.firstName + " " + user.lastName
                        + "</td><td>" + user.email + "</td><td>" + select + "</td>"
                        + "<td><a id='editUser" + user.id + "'class='mx-2' type='button'><i class='bi bi-pencil - square'></i></a> <a id='" + user.id +"'class='deleteUser" + user.id + "'type='button'><i class='bi bi-trash'></i></a></td></tr>"
                    $usersTableBody.append(orders);
                    $('#editUser' + user.id).bind("click", enableSelect);
                    $('.deleteUser' + user.id).bind("click", deleteUser);
                };
            }
        },
        error: function (result) {
            console.log(result);
            return false;
        },
        timeout: 5000,
    });

    function enableSelect() {
        console.log("here");
        var tr = $(this).parents(".tableRow");
        console.log(tr);
        var td = $(tr).children("td");
        var select = $(td).children("select");
        $(select).attr("disabled", false);
        $(select).bind("change", changeRole);
    }

    function changeRole() {
        var $this = $(this);
        var newRole = $(this).val();
        var id = $(this).attr("id");
        var form = new FormData();
        form.append("id", id);
        form.append("role", newRole);
        form.append("__RequestVerificationToken", token);
        $.ajax({
            url: "/users/changeRole/",
            type: 'POST',
            dataType: 'json',
            processData: false,
            contentType: false,
            data: form,
            success: function (response) {
                if (response.success == true) {
                    $this.attr("disabled", true);
                } else {
                    console.log(response.error);
                }
            },
            error: function (result) {
                console.log(result);
                return false;
            },
            timeout: 5000,
        });

    }

    function deleteUser() {
        var $row = $(this).parents(".tableRow");
        var id = $(this).attr("id");
        var form = new FormData();
        form.append("id", id);
        form.append("__RequestVerificationToken", token);
        $.ajax({
            url: "/users/delete/",
            type: 'POST',
            dataType: 'json',
            processData: false,
            contentType: false,
            data: form,
            success: function (response) {
                if (response.success == true) {
                    $row.remove();
                } else {
                    console.log(response.error);
                }
            },
            error: function (result) {
                console.log(result);
                return false;
            },
            timeout: 5000,
        });

    }

});
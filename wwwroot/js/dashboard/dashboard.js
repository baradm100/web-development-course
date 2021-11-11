const months = {
    1: "January",
    2: "February",
    3: "March",
    4: "April",
    5: "May",
    6: "June",
    7: "July",
    8: "August",
    9: "September",
    10: "October",
    11: "November",
    12: "December",
}

const sizes = {
    0: "XS",
    1: "S",
    2: "M",
    3: "L",
    4: "XL",
    5: "XXL",
}

$(function () {
    var $dealsTableBody = $(".dealsTableBody");
    var $usersTableBody = $(".usersBodyTable");
    var formData = new FormData();
    var token = $('input[name="__RequestVerificationToken"]').val();
    formData.append("__RequestVerificationToken", token);

    $("button#ProductDealSearch").on("click", function () {
        var $searchDiv = $(this).parents("#dealSearchDiv");
        var $product = $searchDiv.find("input#userProductSearch");
        var $category = $searchDiv.find(".userCategorySearch");
        var $username = $searchDiv.find("input#userNameSearch");
        var $orderid = $searchDiv.find("input#orderIdSearch");
        orders($product.val(), $category.val(), $username.val(), $orderid.val());
    })

    $("#usersSearchBtn").click(function () {
        var name = $("#nameSearch").val()
        var email = $("#userEmailSearch").val()
        users(name, email)
    })

    const categories = () => {
        $.ajax({
            url: "/Categories/json/",
            type: 'GET',
            dataType: 'json',
            data: null,
            fail: function (xhr, textStatus, errorThrown) {
                Success = false;
                alert("Something went wrong in server")
            },
            success: function (response) {
                Success = true;
                if (response.categories.length > 0) {
                    $('#categoriesDropDownList').html('');
                    const params = new URLSearchParams(window.location.search);
                    const categoryId = params.get("categoryId");
                    var options = '';
                    options += '<option value="Select">All Categories</option>';
                    for (var i = 0; i < response.categories.length; i++) {
                        if (categoryId == response.categories[i].id)
                            options += '<option selected value="' + response.categories[i].name + '">' + response.categories[i].name + '</option>';
                        else
                            options += '<option value="' + response.categories[i].name + '">' + response.categories[i].name + '</option>'
                    }
                    $('#categoriesDropDownList').append(options);
                }
            },
            error: function (result) {
                console.log(result);
                return false
            },
            timeout: 5000,
        });
    }

    categories()

    const orders = (product = "", category = "", username = "", orderid = "") => {
        var form = new FormData();
        var token = $('input[name="__RequestVerificationToken"]').val();
        $dealsTableBody.empty();
        $.ajax({
            url: "/orders/json/?product=" + product + "&category=" + category + "&username=" + username + "&orderid=" + orderid + "&__RequestVerificationToken=" + token,
            type: 'GET',
            dataType: 'json',
            processData: false,
            contentType: false,
            data: form,
            success: function (response) {
                if (response.success == true) {
                    Success = true;
                    var a = response.orders;
                    a.sort((a, b) => parseInt(a.id) - parseInt(b.id));
                    for (let i = 0; i < response.orders.length; i++) {
                        var order = a[i];
                        var closedDeal = order.isCart == true ? "false" : "true";
                        var orders = "<tr><th scope='row'>" + order.id + "</th><td>" + order.firstName + " " + order.lastName + "</td><td>" + order.date + "</td><td>"
                            + order.name + " " + order.color + " " + sizes[order.size] + "</td><td>" + order.amount + "</td><td>" + order.totalPrice + "</td><td>" + closedDeal + "</td></tr>"
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
    }

    orders();

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

    users()

    function users(name = "", email = "") {
        $.ajax({
            url: "/users/json/?name=" + name + "&email=" + email,
            type: 'GET',
            dataType: 'json',
            processData: false,
            contentType: false,
            data: formData,
            success: function (response) {
                $usersTableBody.empty()
                if (response.success) {
                    Success = true;
                    var users = response.users;
                    console.log(users);
                    for (let i = 0; i < users.length; i++) {
                        var user = users[i];
                        var select = selectOption(user.userType, user.id);
                        var orders = "<tr class='tableRow'><th scope='row'>" + user.id + "</th><td>" + user.firstName + " " + user.lastName
                            + "</td><td>" + user.email + "</td><td>" + select + "</td>"
                            + "<td><a id='editUser" + user.id + "'class='mx-2' type='button'><i class='bi bi-pencil - square'></i></a> <a id='" + user.id + "'class='deleteUser" + user.id + "'type='button'><i class='bi bi-trash'></i></a></td></tr>"
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

    }


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
                    alert(response.error);
                }
            },
            error: function (result) {
                console.log(result);
                return false;
            },
            timeout: 5000,
        });

    }
// *********** d3 ***********
    const purchasesByCategory = () => {
        const width = 450;
        const heigt = 400;
        const margin = { top: 50, bottom: 50, left: 50, right: 50 }

        const svg = d3.select(".d3-container-category-purchases").append('svg')
            .attr('heigt', heigt - margin.top - margin.bottom)
            .attr('width', "100%")
            .attr('viewBox', [0, 0, width, heigt]);

 

        $.ajax({
            url: "/products/summery/json/",
            type: 'GET',
            dataType: 'json',
            processData: false,
            contentType: false,
            data: formData,
            success: function (response) {
                if (response.success == true) {
                    console.log(response);
                    const max = Math.max.apply(Math, (response.orders).map(function (o) { return o.amount; }));
                    const x = d3.scaleBand().domain(d3.range(response.orders.length))
                        .range([margin.left, width - margin.right]).padding(0.2);
                    const y = d3.scaleLinear().domain([0, max]).range([heigt - margin.bottom, margin.top])
                    svg.append('g').attr('fill', 'royalblue').selectAll('rect').data(response.orders.sort((a, b) => d3.descending(a.amount, b.amount)))
                        .join('rect').attr('x', (d, i) => x(i)).attr('y', (d) => y(d.amount)).attr('height', d => y(0) - y(d.amount))
                        .attr('width', x.bandwidth());
                    function yAxis(g) {
                        g.attr("transform", `translate(${margin.left}, 0)`)
                            .call(d3.axisLeft(y).ticks(null, (response.orders).format))
                            .attr("font-size", '15px')
                    }

                    function xAxis(g) {
                        g.attr("transform", `translate(0,${heigt - margin.bottom})`)
                            .call(d3.axisBottom(x).tickFormat(i => (response.orders)[i].key))
                            .attr("font-size", '15px')
                    }
                    svg.append("g").call(xAxis);
                    svg.append("g").call(yAxis);
                    svg.node();

                }

            },
            error: function (result) {
                console.log(result);
                return false;
            },
            timeout: 5000,
        });
    }

    const availableStcok = () => {
        const width = 450;
        const heigt = 400;
        const margin = { top: 50, bottom: 50, left: 50, right: 50 }

        const svg = d3.select(".d3-container-available-stock-category").append('svg')
            .attr('heigt', heigt - margin.top - margin.bottom)
            .attr('width', "100%")
            .attr('viewBox', [0, 0, width, heigt]);


        $.ajax({
            url: "/products/availablestock/json/",
            type: 'GET',
            dataType: 'json',
            processData: false,
            contentType: false,
            data: formData,
            success: function (response) {
                if (response.success == true) {
                    console.log(response);
                    const max = Math.max.apply(Math, (response.products).map(function (o) { return o.amount; }));
                    const x = d3.scaleBand().domain(d3.range(response.products.length))
                        .range([margin.left, width - margin.right]).padding(0.1);
                    const y = d3.scaleLinear().domain([0, max]).range([heigt - margin.bottom, margin.top])
                    svg.append('g').attr('fill', 'royalblue').selectAll('rect').data(response.products.sort((a, b) => d3.descending(a.amount, b.amount)))
                        .join('rect').attr('x', (d, i) => x(i)).attr('y', (d) => y(d.amount)).attr('height', d => y(0) - y(d.amount))
                        .attr('width', x.bandwidth());
                    function yAxis(g) {
                        g.attr("transform", `translate(${margin.left}, 0)`)
                            .call(d3.axisLeft(y).ticks(null, (response.products).format))
                            .attr("font-size", '15px')
                    }

                    function xAxis(g) {
                        g.attr("transform", `translate(0,${heigt - margin.bottom})`)
                            .call(d3.axisBottom(x).tickFormat(i => (response.products)[i].key))
                            .attr("font-size", '15px')
                    }
                    svg.append("g").call(xAxis);
                    svg.append("g").call(yAxis);
                    svg.node();

                }

            },
            error: function (result) {
                console.log(result);
                return false;
            },
            timeout: 5000,
        });
    }

    const ordersByMonth = () => {
        const width = 450;
        const heigt = 400;
        const margin = { top: 50, bottom: 50, left: 50, right: 50 }

        const svg = d3.select(".d3-container-order-by-month").append('svg')
            .attr('heigt', heigt - margin.top - margin.bottom)
            .attr('width', "100%")
            .attr('viewBox', [0, 0, width, heigt]);


        $.ajax({
            url: "/orders/monthsummery/json/",
            type: 'GET',
            dataType: 'json',
            processData: false,
            contentType: false,
            data: formData,
            success: function (response) {
                if (response.success == true) {
                    console.log(response);
                    const max = Math.max.apply(Math, (response.orders).map(function (o) { return o.amount; }));
                    const x = d3.scaleBand().domain(d3.range(response.orders.length))
                        .range([margin.left, width - margin.right]).padding(0.1);
                    const y = d3.scaleLinear().domain([0, max]).range([heigt - margin.bottom, margin.top])
                    svg.append('g').attr('fill', 'royalblue').selectAll('rect').data(response.orders.sort((a, b) => d3.descending(a.amount, b.amount)))
                        .join('rect').attr('x', (d, i) => x(i)).attr('y', (d) => y(d.amount)).attr('height', d => y(0) - y(d.amount))
                        .attr('width', x.bandwidth());
                    function yAxis(g) {
                        g.attr("transform", `translate(${margin.left}, 0)`)
                            .call(d3.axisLeft(y).ticks(null, (response.orders).format))
                            .attr("font-size", '15px')
                    }

                    function xAxis(g) {
                        g.attr("transform", `translate(0,${heigt - margin.bottom})`)
                            .call(d3.axisBottom(x).tickFormat(i => months[(response.orders)[i].key]))
                            .attr("font-size", '15px')
                    }
                    svg.append("g").call(xAxis);
                    svg.append("g").call(yAxis);
                    svg.node();

                }

            },
            error: function (result) {
                console.log(result);
                return false;
            },
            timeout: 5000,
        });
    }

    

    purchasesByCategory();
    ordersByMonth();
    availableStcok();


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
                    alert(response.error);
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
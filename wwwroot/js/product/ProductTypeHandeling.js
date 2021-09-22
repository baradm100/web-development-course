$(function () {
    var productName = null;
    var isEdit = false;
    var startQuantity = 0;
    const PRODUCT_SIZES = {
        "xs": 0,
        "s": 1,
        "m": 2,
        "l": 3,
        "xl": 4,
        "xxl": 5,
    };

    //convert rgb string to hex
    const rgb2hex = (rgb) => `#${rgb.match(/^rgb\((\d+),\s*(\d+),\s*(\d+)\)$/).slice(1).map(n => parseInt(n, 10).toString(16).padStart(2, '0')).join('')}`

    $('.btnAddGoods').click(function () {
        productName = $(this).get(0).id;
        $("#addGoodsModal").modal("show");
        $.ajax({
            url: "/ProductTypes/GetColors/",
            type: 'GET',
            dataType: 'json',
            data: null,
            fail: function (xhr, textStatus, errorThrown) {
                Success = false;
                alert("Something went wrong in server")
            },
            success: function (response) {
                Success = true;
                if (response.success == true) {
                    if (response.colors.result.length > 0) {
                        $('#Color').html('');
                        var options = '';
                        for (var i = 0; i < response.colors.result.length; i++) {
                            if (response.colors.result[i].name == "White")
                                options += `<div class="form-check form-check-inline"><input class="form-check-input" style="background:#${response.colors.result[i].color};" type="radio" id="${response.colors.result[i].name}" value="${response.colors.result[i].id}"></div>`
                            else
                                options += `<div class="form-check form-check-inline"><input class="form-check-input" style="background:#${response.colors.result[i].color}; border-color:transparent" type="radio" id="${response.colors.result[i].name}" value="${response.colors.result[i].id}"></div>`
                        }
                        $('#Color').append(options);
                    }
                }
            },
            error: function (result) {
                console.log(result);
                return false
            },
            timeout: 5000,
        });
    });

    $(".editProductType").click(function () {
        var color = $(this).css("background-color");
        var size = $(this).attr("size");
        var quantity = $(this).attr("quantity");
        productName = $(this).attr("name");
        console.log(productName);
        console.log(color + " " + size + " " + quantity);
        $("#addGoodsModal").modal("show");
        $("#deleteGoodsModalBtn").removeClass("d-none");
        $("#Color").css("background", rgb2hex(color));
        $("#Color").attr("disabled", true);
        $("#Size").val(PRODUCT_SIZES[size]).change();
        $("#Size").attr("disabled", true);
        $("#Quantity").val(quantity);
        isEdit = true;
        startQuantity = parseInt(quantity);
        $(".modal-title").text("Edit Goods");
    });

    $("#deleteGoodsModalBtn").click(function () {
        $("#loadingSpinner").removeClass("d-none");
        $("#addGoodsForm").addClass("d-none");
        $("#errorIcon").addClass("d-none");
        $("#successIcon").addClass("d-none");
        var color = $('input:checked').val();
        var size = $("#Size").val();
        var quantity = $("#Quantity").val();
        var token = $('input[name="__RequestVerificationToken"]').val();
        var Success = false;
        $.ajax({
            url: "/ProductTypes/DeleteGoods/",
            type: 'POST',
            dataType: 'json',
            data: {
                __RequestVerificationToken: token,
                ProductName: productName,
                Size: size,
                Quantity: quantity,
                Color: color,
            },
            success: function (result) {
                if (result.success == true) {
                    Success = true;
                    $("#loadingSpinner").addClass("d-none");
                    $("#addGoodsForm").removeClass("d-none");
                    $("#successIcon").removeClass("d-none");
                    $("#addGoodsModalBtn").addClass("d-none");
                    $("#deleteGoodsModalBtn").addClass("d-none");
                } else {
                    Success = false;
                    $("#loadingSpinner").addClass("d-none");
                    $("#addGoodsForm").removeClass("d-none");
                    $("#errorIcon").removeClass("d-none");
                }
            },
            error: function (result) {
                console.log(result);
                return false
            },
            complete: function () {
                if (!Success) { $("#errorIcon").removeClass("d-none"); }
                $("#loadingSpinner").addClass("d-none");
                $("#addGoodsForm").removeClass("d-none");
                isEdit = false;
                startQuantity = 0;
            },
            timeout: 5000
        });
        return Success;
    });



    $("#addGoodsModalBtn").click(function () {
        var formData = new FormData();
        $("#loadingSpinner").removeClass("d-none");
        $("#addGoodsForm").addClass("d-none");
        $("#errorIcon").addClass("d-none");
        $("#successIcon").addClass("d-none");
        var color = $('input:checked').val();
        var size = $("#Size").val();
        var quantity = $("#Quantity").val();
        if (isEdit) {
            quantity = startQuantity + (quantity - startQuantity);
        }
        var token = $('input[name="__RequestVerificationToken"]').val();
        var Success = false;
        formData.append("__RequestVerificationToken", token);
        formData.append("Quantity", quantity);
        formData.append("ColorId", color);
        formData.append("productName", productName);
        formData.append("Size", size);
        $.ajax({
            url: "/ProductTypes/AddGoods/",
            type: 'POST',
            dataType: 'json',
            data: formData,
            cache: false,
            processData: false,
            contentType: false,
            success: function (result) {
                if (result.success == true) {
                    Success = true;
                    $("#loadingSpinner").addClass("d-none");
                    $("#addGoodsForm").removeClass("d-none");
                    $("#successIcon").removeClass("d-none");
                    $(this).val = "add more goods";
                } else {
                Success = false;
                $("#loadingSpinner").addClass("d-none");
                $("#addGoodsForm").removeClass("d-none");
                $("#errorIcon").removeClass("d-none");
                console.log(result);
                }
            },
            error: function (result) {
                console.log(result);
                return false
            },
            complete: function () {
                if (!Success) { $("#errorIcon").removeClass("d-none");}
                $("#loadingSpinner").addClass("d-none");
                $("#addGoodsForm").removeClass("d-none");
                $("#deleteGoodsModalBtn").addClass("d-none");
                isEdit = false;
                startQuantity = 0;
            },
            timeout: 5000
        });
        return Success;
    });

    $('#closeBtn').click(function () {
        $(".modal-title").text("Add Goods");
        $("#deleteGoodsModalBtn").addClass("d-none");
        $("#addGoodsModalBtn").removeClass("d-none");
        productName = null;
        startQuantity = 0;
        isEdit = false;
        location.reload();
    });
});
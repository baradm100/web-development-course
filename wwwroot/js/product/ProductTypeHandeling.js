$(function () {
    var productName = null;
    var isEdit = false;
    var startQuantity = 0;
    var productTypeQuantityValid = true;
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
                            options += `<input class="form-check-input" type="radio" id="${response.colors.result[i].name}" name="inlineRadioOptions" value="${response.colors.result[i].id}" style="background-color: #${response.colors.result[i].color}; border-radius: 18px;"></input>`
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

    const uploadProductType = (token, color, size, quantity, prodName) => {
        var formData = new FormData();
        formData.append("__RequestVerificationToken", token);
        formData.append("Quantity", quantity);
        formData.append("ColorId", color);
        formData.append("productName", prodName);
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
                    $(this).val = "add more goods";
                } else {
                    Success = false;
                    console.log(result);
                }
            },
            error: function (result) {
                console.log(result);
                return false
            },
            complete: function () {
                if (!Success) { $("#errorIcon").removeClass("d-none"); }
                else {
                    $("#successIcon").removeClass("d-none");
                }
                isEdit = false;
                startQuantity = 0;
            },
            timeout: 5000
        });
    }

    $("#addGoodsModalBtn").click(function () {
        if (productTypeQuantityValid == false) {
            $(this).parent("div").append("<div class='alert alert-danger' style='color: red' id='InvalidWarning'>There is an invalid Inputs</div>");
            return false;
        }
        $(this).siblings("#InvalidWarning").remove()
        $("#loadingSpinner").removeClass("d-none");
        $("#addGoodsForm").addClass("d-none");
        $("#errorIcon").addClass("d-none");
        $("#successIcon").addClass("d-none");
        var uploadSuccess = false;
        $(".modal-body").find(".goods").each(function (i, goods) {
            var color = $(goods).find('input:checked').val();
            var size = $(goods).find("#Size").val();
            var quantity = $(goods).find("#Quantity").val();
            var token = $('input[name="__RequestVerificationToken"]').val();
            uploadSuccess = uploadProductType(token, color, size, quantity, productName);
        });
        $("#loadingSpinner").addClass("d-none");
        $("#addGoodsForm").removeClass("d-none");
        $("#deleteGoodsModalBtn").addClass("d-none");

        return uploadSuccess;
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

    $("#addAnotherGood").on("click", function () {
        var $good = $("#addGoodsForm").clone();
        $good.find("input[type=radio]").each(function (i, x) {
            $(x).attr("name", $("input").length);
            $(x).attr("checked", false)
        });
        $good.find("#Quantity").val("0");
        $good.insertBefore("#AnotherGood");
    })

    const addWarningSmallerThanZero = (selector, warningId, alert) => {
        if ($(selector).val() < 0) {
            $(selector).parent("div").append("<div class='alert alert-danger' style='color: red' id=" + warningId + ">" + alert + "</div>");
            return false
        }
        else {
            $(selector).siblings("#" + warningId).remove()
            return true
        }
    };

    $('#Quantity').change(() => productTypeQuantityValid = addWarningSmallerThanZero('#Quantity', 'quantityAlert', 'Price cannot be lower than zero'));

});
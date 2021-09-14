$(function () {
    var productName = null;
    var isEdit = false;
    var startQuantity = 0;
    const productSizes = {
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
    });

    $(".editProductType").click(function () {
        var color = $(this).children("#color").css("background-color");
        var size = $(this).attr("size");
        var quantity = $(this).attr("quantity");
        productName = $(this).attr("name");
        console.log(productName);
        console.log(color + " " + size + " " + quantity);
        $("#addGoodsModal").modal("show");
        $("#Color").val(rgb2hex(color));
        $("#color").attr("disabled", true);
        $("#size").attr("disabled", true);
        $("#Size").val(productSizes[size]).change();
        $("#Quantity").val(quantity);
        isEdit = true;
        startQuantity = parseInt(quantity);
        $(".modal-title").val("Edit Goods");
        $("#deleteGoodsModalBtn").removeClass("d-none");
    });

    //todo: add delete

    $("#addGoodsModalBtn").click(function () {
        $("#loadingSpinner").removeClass("d-none");
        $("#addGoodsForm").addClass("d-none");
        $("#errorIcon").addClass("d-none");
        $("#successIcon").addClass("d-none");
        var color = $("#Color").val();
        var size = $("#Size").val();
        var quantity = $("#Quantity").val();
        if (isEdit) {
            quantity = startQuantity + (quantity - startQuantity);
        }
        console.log(color + " " + size + " " + quantity);
        var token = $('input[name="__RequestVerificationToken"]').val();
        var Success = false;
        $.ajax({
            url: "/ProductTypes/AddGoods/",
            type: 'POST',
            dataType: 'json',
            data: {
                __RequestVerificationToken: token,
                ProductName: productName,
                Size: size,
                Quantity: quantity,
                Color: color,
            },
            fail: function (xhr, textStatus, errorThrown) {
                Success = false;
                $("#loadingSpinner").addClass("d-none");
                $("#addGoodsForm").removeClass("d-none");
                $("#errorIcon").removeClass("d-none");
                console.log(result);
            },
            success: function (result) {
                Success = true;
                $("#loadingSpinner").addClass("d-none");
                $("#addGoodsForm").removeClass("d-none");
                $("#successIcon").removeClass("d-none");
                $(this).val = "add more goods";
                console.log(result);
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

    $('.btnDeleteProduct').click(function () {
        $(this).parent().css("background", "grey");
    });

    $('#closeBtn').click(function () {
        location.reload();
    })
});
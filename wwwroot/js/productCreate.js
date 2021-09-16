$(function () {
    $('#addGoods').click(function () {
        var fileUpload = $("#files").get(0);
        var files = fileUpload.files;
        console.log(fileUpload);
        // Create FormData object  
        var fileData = new FormData();

        // Looping over all files and add it to FormData object  
        for (var i = 0; i < files.length; i++) {
            fileData.append(files[i].name, files[i]);
        }
        var productName = $('.productName').val();
        var productPrice = $('.productPrice').val();
        var token = $('input[name="__RequestVerificationToken"]').val();
        $.ajax({
            url: '/Products/AddGoods',
            type: 'POST',
            data: {
                __RequestVerificationToken: token,
                Price: productPrice,
                Name: productName,
                files: fileData,
                DiscountPercentage: 0,
            },
            success: function (result) {
                console.log(result);
                alert('result');
            }
        });
    });

    $('.btns').click(function () {
        var size = $(this).attr("id");
        var token = $('input[name="__RequestVerificationToken"]').val();
        var color = $('input[name="color' + size + '"]').val();
        var quantity = $('input[name="quantity' + size + '"]').val();
        var a = $.ajax({
            dataType: "json",
            url: '/Products/GetLastProductId',
            success: function (result) {
                var productId = result.id;
                console.log(size + " " + color + " " + quantity + " " + productId);
                $.ajax({
                    url: '/ProductTypes/AddGoods',
                    type: 'POST',
                    data: {
                        __RequestVerificationToken: token,
                        ProductId: productId,
                        Size: size,
                        Color: color,
                        body: quantity
                    },
                    success: function (result) {
                        console.log(result);
                        return true;
                    },
                    error: function (result) {
                        console.log(result);
                        return false
                    }
                });
                return false;
            }
        });
    });
});
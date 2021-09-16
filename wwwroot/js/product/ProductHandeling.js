$(function () {
    var dataImages = null;
    var product_names = $(".card-title");


    const insertCategoriesToCards = (name) => {
        for (let i = 0; i < name.length; i++) {
            form = new FormData();
            var Name = name[i].textContent;
            form.append("name", Name);
            $.ajax({
                url: "/Categories/GetProductCategories/",
                type: 'POST',
                dataType: 'json',
                processData: false,
                contentType: false,
                data: form,
                fail: function (xhr, textStatus, errorThrown) {
                    Success = false;
                },
                success: function (response) {
                    Success = true;
                    var ProductName = response.name;
                    var Text = "Categories: ";
                    for (let i = 0; i < response.categories.length; i++){
                        Text += response.categories[i];
                        Text += ", ";
                    };
                    var a = "[category=" + "'" + ProductName + "'" + "]"
                    $(a).text(Text);
                },
                error: function (result) {
                    console.log(result);
                    return false;
                },
                timeout: 5000,
            });
        };
    };

    insertCategoriesToCards(product_names);

    $('#AddNewProductBtn').click(function () {
        $("#addProductModal").modal("show");
        $.ajax({
            url: "/Categories/GetCategories/",
            type: 'POST',
            dataType: 'json',
            data: null,
            fail: function (xhr, textStatus, errorThrown) {
                Success = false;
                alert("Something went wrong in server")
            },
            success: function (response) {
                Success = true;
                console.log(response.categories.result);
                if (response.categories.result.length > 0) {
                    $('#categoriesDropDownListPModel').html('');
                    var options = '';
                    options += '<option value="Select">---</option>';
                    for (var i = 0; i < response.categories.result.length; i++) {
                        console.log(response.categories.result[i]);
                        options += '<option value="' + response.categories.result[i].name + '">' + response.categories.result[i].name + '</option>';
                    }
                    $('#categoriesDropDownListPModel').append(options);
                }
            },
            error: function (result) {
                console.log(result);
                return false
            },
            timeout: 5000,
        });
    });

    $("#UploadImg").change(function () {
        var formData = new FormData();
        var totalFiles = $("#UploadImg").prop("files");
        for (var i = 0; i < totalFiles.length; i++) {
            var file = totalFiles[i];
            formData.append('file', file);
        };
        dataImages = formData;
    });

    const uploadImages = (formData, id) => {
        var Success = false
        var token = $('input[name="__RequestVerificationToken"]').val();
        formData.append("__RequestVerificationToken", token);
        formData.append("Id", id);
        formData.append("Files", dataImages);
        $.ajax({
            type: "POST",
            url: '/Products/AddProductImage/',
            data: formData,
            dataType: 'json',
            cache: false,
            processData: false,
            contentType: false,
            success: function (response) {
            if (response.success == true) {
                Success = true;
                $("#ProductSuccessIcon").removeClass("d-none");
            } else {
                Success = false;
                $("#ProductErrorIcon").removeClass("d-none");
                $("#ProductErrorIcon").Text("Image Uploading Problem");
            };
            },
            complete: function () {
                return Success;
            }
        });
    };

    $("#addProductModalBtn").click(function () {
        $("#ProductloadingSpinner").removeClass("d-none");
        $("#addProductForm").addClass("d-none");
        $("#ProductErrorIcon").addClass("d-none");
        $("#ProductSuccessIcon").addClass("d-none");
        var ProductName = $("#ProductName").val();
        var ProductPrice = $("#ProductPrice").val();
        var ProductCategories = $("#categoriesDropDownListPModel").val();
        console.log(ProductCategories);
        var ProductDiscount = $("#ProductDiscount").val();
        var token = $('input[name="__RequestVerificationToken"]').val();
        var Success = false;
        $.ajax({
            url: "/Products/AddProduct/",
            type: 'POST',
            dataType: 'json',
            data: {
                __RequestVerificationToken: token,
                Price: ProductPrice,
                Name: ProductName,
                DiscountPercentage: ProductDiscount,
                Categories: ProductCategories,
            },
            success: function (response) {
                if (response.success == true) {
                    var imagesUploading = uploadImages(dataImages, response.productId);
                    $("#ProductloadingSpinner").addClass("d-none");
                    $("#addProductForm").removeClass("d-none");
                    Success = true;
                    $("#ProductSuccessIcon").removeClass("d-none");
                } else {
                    Success = false;
                    $("#ProductloadingSpinner").addClass("d-none");
                    $("#addProductForm").removeClass("d-none");
                    $("#ProductErrorIcon").removeClass("d-none");
                }
            },
            error: function (result) {
                console.log(result);
                return false
            },
            complete: function () {
                if (!Success) { $("#ProductErrorIcon").removeClass("d-none"); }
                $("#ProductloadingSpinner").addClass("d-none");
                $("#addProductForm").removeClass("d-none");
                dataImages = null;
                return Success;
            },
            timeout: 10000
        });
        return Success;
    });

    $('#closeProductBtn').click(function () {
        location.reload();
    });

    $('.btnDeleteProduct').click(function () {
        var formData = new FormData();
        var Success = false
        var token = $('input[name="__RequestVerificationToken"]').val();
        var id = $(this).attr("id");
        formData.append("__RequestVerificationToken", token);
        formData.append("Id", id);
        $.ajax({
            type: "POST",
            url: '/Products/DeleteProduct/',
            data: formData,
            dataType: 'json',
            cache: false,
            processData: false,
            contentType: false,
            success: function (response) {
                if (response.success == true) {
                    Success = true;
                } else {
                    Success = false;
                };
            },
            complete: function () {
                if (Success)
                    location.reload();
                else {
                    alert("Delete Problem in server")
                }
                return Success;
            }
        });
    });
});
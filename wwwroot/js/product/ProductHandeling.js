$(function () {
    var dataImages = null;
    var product_names = $(".card-title");
    var productEditId = null;
    var Changed = false;
    var productPriceValid = true;
    var productDiscountValid = true;


    const insertCategoriesToCards = (name) => {
        for (let i = 0; i < name.length; i++) {
            form = new FormData();
            var Name = name[i].textContent;
            form.append("name", Name);
            $.ajax({
                url: "/Categories/ProductCategories/",
                type: 'Get',
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
                    for (let i = 0; i < response.categories.length; i++) {
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
        addCategoriesToModal("#categoriesDropDownListPModel");
    });

    $('.btnEditProduct').click(function () {
        productEditId = $(this).attr("id");
        var Success = addCategoriesToModal("#categoriesEditDropDownListPModel");
        var formData = new FormData();
        var ProductId = $(this).attr("id");
        var ProductName = $("#ProductEditName");
        var ProductPrice = $("#ProductEditPrice");
        var ProductDiscount = $("#ProductEditDiscount");
        var ProductCategories = $("#categoriesEditDropDownListPModel");
        var ProductImages = $("#imagesProduct");
        formData.append("Id", ProductId);
        $.ajax({
            url: "/Products/GetProduct/",
            type: 'POST',
            data: formData,
            cache: false,
            processData: false,
            contentType: false,
            dataType: 'json',
            fail: function (xhr, textStatus, errorThrown) {
                Success = false;
                alert("Something went wrong in server")
            },
            success: function (response) {
                if (response.success == true) {
                    Success = true;
                    ProductName.val(response.name);
                    ProductPrice.val(response.price);
                    ProductDiscount.val(response.discount);
                    ProductImages.html("");
                    for (let i = 0; i < response.categories.length; i++)
                        ProductCategories.find("[value='" + response.categories[i] + "']").attr('selected', true);
                    for (let i = 0; i < response.images.length; i++) {
                        ProductImages.append(`<button type="button" style="margin-buttom=2px;" id="${response.images[i].id}" class="DeleteImage btn btn-danger btn-sm">Delete Image ${response.images[i].name}</button>`)
                        ProductImages.append(`<img src="data:image/png;base64,${response.images[i].imageData}" id="${response.images[i].id}" class="img-fluid img-thumbnail" />`);
                    };
                } else {
                    Success = false;
                }
            },
            error: function (result) {
                console.log(result);
                return false
            },
            complete: function () {
                $("#editProductModal").modal("show");
                if (!Success) {
                    $("#ProductErrorIcon").removeClass("d-none");
                    $("#ProductErrorIcon").Text("Problem with getting product details");
                }
            },
            timeout: 5000,
        });
        $("#editProductModal").modal("show");
    });

    $("#imagesProduct").on("click", ".DeleteImage", function () {
        var ProductImages = $("#imagesProduct");
        var token = $('input[name="__RequestVerificationToken"]').val();
        var ImgId = $(this).attr("id");

        var formData = new FormData();
        formData.append("__RequestVerificationToken", token);
        formData.append("id", ImgId);
        $("#ProductEditloadingSpinner").removeClass("d-none");
        $.ajax({
            url: "/Products/DeleteProductImage/",
            type: 'POST',
            data: formData,
            cache: false,
            processData: false,
            contentType: false,
            dataType: 'json',
            fail: function (xhr, textStatus, errorThrown) {
                Success = false;
                alert("Something went wrong in server")
            },
            success: function (response) {
                if (response.success == true) {
                    Success = true;
                    ProductImages.children(`button#${ImgId}`).hide();
                    ProductImages.children(`img#${ImgId}`).hide();
                    $("#ProductEditloadingSpinner").addClass("d-none");
                    $("#edotProductForm").removeClass("d-none");
                    $("#ProductEditSuccessIcon").removeClass("d-none");
                    Changed = true;
                } else {
                    Success = false;
                    $("#ProductEditloadingSpinner").addClass("d-none");
                    $("#editProductForm").removeClass("d-none");
                    $("#ProductEditErrorIcon").removeClass("d-none");
                };
            },
            error: function (result) {
                console.log(result);
                return false
            },
            complete: function () {
                $("#ProductEditloadingSpinner").addClass("d-none");
            },
            timeout: 5000,
        });
    });

    const findActiveCheckbox = (div) => {
        var actives = [];
        $(div).find(':checkbox').each(function () {
            if (jQuery(this).attr('checked') == true)
                actives.add(jQuery(this).attr('value'));
        });
        return selected;
    }

    const findSelectedDropdown = (selector) => {
        var selected = [];
        var s = $(selector).find('option:selected')
        for (let i = 0; i < s.length; i++)
            selected.push($(s[i]).val());
        return selected;
    }

    $("#editProductModalBtn").click(function () {
        if (!productDiscountValid || !productPriceValid) {
            $(this).parent("div").append("<span style='color: red' id='formAlert'>Values are not valid!</span >")
            return false;
        }
        var formData = new FormData();
        $("#ProductEditloadingSpinner").removeClass("d-none");
        $("#editProductForm").addClass("d-none");
        $("#ProductEditErrorIcon").addClass("d-none");
        $("#ProductEditSuccessIcon").addClass("d-none");
        var ProductName = $("#ProductEditName").val();
        var ProductPrice = $("#ProductEditPrice").val();
        findSelectedDropdown("#categoriesEditDropDownListPModel").forEach(a => formData.append("categories", a));
        var ProductDiscount = $("#ProductEditDiscount").val();
        var token = $('input[name="__RequestVerificationToken"]').val();

        formData.append("__RequestVerificationToken", token);
        formData.append("id", productEditId);
        formData.append("Name", ProductName);
        formData.append("Price", ProductPrice);
        formData.append("DiscountPercentage", ProductDiscount);

        var Success = false;
        $.ajax({
            url: "/Products/EditProduct/",
            type: 'POST',
            dataType: 'json',
            data: formData,
            cache: false,
            processData: false,
            contentType: false,
            success: function (response) {
                if (response.success == true) {
                    $("#ProductEditloadingSpinner").addClass("d-none");
                    $("#editProductForm").removeClass("d-none");
                    $("#ProductEditSuccessIcon").removeClass("d-none");
                    Success = true;
                    uploadImages(dataImages, productEditId);
                    Changed = true;
                } else {
                    Success = false;
                    $("#ProductEditloadingSpinner").addClass("d-none");
                    $("#editProductForm").removeClass("d-none");
                    $("#ProductEditErrorIcon").text(response.errorDetails);
                    $("#ProductEditErrorIcon").removeClass("d-none");
                }
            },
            error: function (result) {
                console.log(result);
                return false
            },
            complete: function () {
                if (!Success) { $("#ProductEditErrorIcon").removeClass("d-none"); }
                $("#ProductEditloadingSpinner").addClass("d-none");
                $("#editProductForm").removeClass("d-none");
                dataImages = null;
                return Success;
            },
            timeout: 10000
        });
        var Success = addCategoriesToModal("#categoriesDropDownListPModel");
        return Success;
    });

    $(".EditUploadNewImages").children("#EditImageUpload").on("change", function () {
        var formData = new FormData();
        var totalFiles = $("#EditImageUpload").prop("files");
        for (var i = 0; i < totalFiles.length; i++) {
            var file = totalFiles[i];
            formData.append('file', file);
        };
        dataImages = formData;
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

    const addCategoriesToModal = (dropDownId) => {
        $.ajax({
            url: "/Categories/json/",
            type: 'Get',
            dataType: 'json',
            data: null,
            fail: function (xhr, textStatus, errorThrown) {
                Success = false;
                alert("Something went wrong in server")
            },
            success: function (response) {
                Success = true;
                if (response.categories.length > 0) {
                    $(dropDownId).html('');
                    var options = '';
                    options += '<option value="Select">---</option>';
                    for (var i = 0; i < response.categories.length; i++) {
                        options += '<option value="' + response.categories[i].name + '">' + response.categories[i].name + '</option>';
                    }
                    $(dropDownId).append(options);
                }
            },
            error: function (result) {
                console.log(result);
                return false
            },
            timeout: 5000,
        });
    }


    $("#addProductModalBtn").click(function () {
        if (!productDiscountValid || !productPriceValid) {
            $(this).parent("div").append("<span style='color: red' id='formAlert'>Values are not valid!</span >")
            return false;
        }
        $("#ProductloadingSpinner").removeClass("d-none");
        $("#addProductForm").addClass("d-none");
        $("#ProductErrorIcon").addClass("d-none");
        $("#ProductSuccessIcon").addClass("d-none");
        var ProductName = $("#ProductName").val();
        var ProductPrice = $("#ProductPrice").val();
        var ProductCategories = $("#categoriesDropDownListPModel").val();
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
                    $("#ProductloadingSpinner").addClass("d-none");
                    $("#addProductForm").removeClass("d-none");
                    uploadImages(dataImages, response.productId);
                    Success = true;
                    $("#ProductSuccessIcon").removeClass("d-none");
                } else {
                    Success = false;
                    $("#ProductloadingSpinner").addClass("d-none");
                    $("#addProductForm").removeClass("d-none");
                    $("#ProductErrorIcon").text(response.errorDetails);
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
        var Success = addCategoriesToModal("#categoriesDropDownListPModel");
        return Success;
    });

    $('#closeProductBtn').click(function () {
        location.reload();
    });

    $('#closeEditProductBtn').click(function () {
        $("#ProductEditName").val("");
        $("#ProductEditPrice").val("");
        $("#ProductEditDiscount").val("");
        $("#categoriesEditDropDownListPModel").val("");
        $("#imagesCheckBox").html("");
        if (Changed)
            location.reload();
    });

    const addWarningSmallerThanZero = (selector, warningId, alert) => {
        if ($(selector).val() < 0) {
            $(selector).parent("div").append("<span style='color: red' id=" + warningId + ">"+ alert + "</span >");
            return false
        }
        else {
            $(selector).siblings("#"+warningId).remove()
            return true
        }
    };

    $('#ProductPrice').change(() => productPriceValid = addWarningSmallerThanZero('#ProductPrice', 'priceAlert', 'Price cannot be lower than zero'));
    $('#ProductDiscount').change(() => productDiscountValid = addWarningSmallerThanZero('#ProductDiscount', 'discountAlert', 'Discount cannot be lower than zero'));

    $('#ProductEditPrice').change(() => productPriceValid = addWarningSmallerThanZero('#ProductEditPrice', 'priceAlert', 'Price cannot be lower than zero'));
    $('#ProductEditDiscount').change(() => productDiscountValid = addWarningSmallerThanZero('#ProductEditDiscount', 'discountAlert', 'Discount cannot be lower than zero'));


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
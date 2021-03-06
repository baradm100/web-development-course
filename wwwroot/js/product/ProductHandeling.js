$(function () {
    var dataImages = null;
    var product_names = $(".card-title");
    var productEditId = null;
    var Changed = false;
    var productPriceValid = true;
    var productDiscountValid = true;
    var productNameValid = false;
    const $CategorieTable = $(".editCategoriesTableBody");

    const insertCategoriesToCards = (name) => {
        for (let i = 0; i < name.length; i++) {
            form = new FormData();
            var Name = name[i].textContent;
            form.append("name", Name);
            $.ajax({
                url: "/Categories/ProductCategories/",
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
                    if (response.success == true) {
                        for (let i = 0; i < response.categories.length; i++) {
                            Text += response.categories[i];
                            Text += ", ";
                        };
                        var a = "[category=" + "'" + ProductName + "'" + "]"
                        $(a).text(Text);
                    }
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

    function deleteCategory()  {
        var form = new FormData();
        var token = $('input[name="__RequestVerificationToken"]').val();
        var id = $(this).parents("tr").attr("id");
        form.set("__RequestVerificationToken", token);
        form.set("id", id);
        $.ajax({
            url: "/categories/delete/",
            type: 'POST',
            dataType: 'json',
            processData: false,
            contentType: false,
            data: form,
            success: function (response) {
                if (response.success == true) {
                    $CategorieTable.children("tr#" + id).remove()
                }
            },
            error: function (result) {
                console.log(result);
                return false;
            },
            timeout: 5000,
        });
    }

    const buttons = (id) => "<button class='btn px-0 mx-0' id='deleteCategory" + id + "'><i class='bi bi-trash'></i></button>";

    const getCategoriesTree = () => {
        var form = new FormData();
        var token = $('input[name="__RequestVerificationToken"]').val();
        form.set("__RequestVerificationToken", token);
        $.ajax({
            url: "/categoriesTree/json/",
            type: 'GET',
            dataType: 'json',
            processData: false,
            contentType: false,
            data: form,
            success: function (response) {
                if (response.success == true) {
                    Success = true;
                    var a = response.categories;
                    a.sort((a, b) => parseInt(a.id) - parseInt(b.id));
                    for (let i = 0; i < response.categories.length; i++) {
                        var category = a[i];
                        var categories = "<tr id='" + category.id + "'><th>" + category.name + "</th><td>" + category.parent + "</td>"
                            + "<td>" + buttons(category.id) + "</td></tr>"
                        $CategorieTable.append(categories);
                        $("button#deleteCategory" + category.id).bind("click", deleteCategory);
                    }
                }
            },
            error: function (result) {
                console.log(result);
                return false;
            },
            timeout: 5000,
        });
    }

    $(".editCategoriesModal-close").click(function () {
        window.location.reload();
    })

    $('#EdtiCategoriesBtn').click(function () {
        $CategorieTable.empty();
        $("#editCategoriesModal").modal("show");
        getCategoriesTree();
    });

    $('#AddNewProductBtn').click(function () {
        $("#addProductModal").modal("show");
        addCategoriesToModal("#categoriesDropDownListPModel");
    });

    $('.btnEditProduct').click(function () {
        productNameValid = true;
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
        if (!productEditValid())
            return false;
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
                if (response.success == true) {
                    if (response.categories.length > 0) {
                        $(dropDownId).html('');
                        var options = '';
                        options += '<option value="Select">---</option>';
                        for (var i = 0; i < response.categories.length; i++) {
                            options += '<option value="' + response.categories[i].name + '">' + response.categories[i].name + '</option>';
                        }
                        $(dropDownId).append(options);
                    }
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
        if (!productValid())
            return false;
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
                    Changed = true;
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

    $(".closeBtn").click(function () {
        document.location.reload(true);
    });

    $('#closeEditProductBtn').click(function () {
        $("#ProductEditName").val("");
        $("#ProductEditPrice").val("");
        $("#ProductEditDiscount").val("");
        $("#categoriesEditDropDownListPModel").val("");
        $("#imagesCheckBox").html("");
        productNameValid = false;
        document.location.reload(true);
    });

    const productValid = () => {
        // price validation
        if ($('#ProductPrice').val() < 0) {
            if ($('#ProductPrice').siblings("#priceWarning").length == 0)
                $('#ProductPrice').parent("div").append("<span class='alert alert-danger p-1 m-2' id='priceWarning'>Price can't be negative</span >");
            return false;
        }
        else {
            $('#ProductPrice').siblings("#priceWarning").remove();
        }

        // discount validation
        if ($('#ProductDiscount').val() < 0) {
            if ($('#ProductDiscount').siblings("#discaountMinWarning").length == 0)
                $('#ProductDiscount').parent("div").append("<span class='alert alert-danger p-1 m-2' id='discaountMinWarning'>Discount can't be negative</span >");
            return false;
        }
        else {
            $('#ProductDiscount').siblings("#discaountMinWarning").remove();
        }
        if ($('#ProductDiscount').val() > 100) {
            if ($('#ProductDiscount').siblings("#discaountMaxWarning").length == 0)
                $('#ProductDiscount').parent("div").append("<span class='alert alert-danger p-1 m-2' id='discaountMaxWarning'>Discount can't be bigger than 100</span >");
            return false;
        }
        else {
            $('#ProductDiscount').siblings("#discaountMaxWarning").remove();
        }

        // name validation
        if ($("#ProductName").val() == null || $("#ProductName").val().length == 0) {
            if ($('#ProductName').siblings("#nameWarning").length == 0)
                $('#ProductName').parent("div").append("<span class='alert alert-danger p-1 m-2' id='nameWarning'>you must enter a name</span >");
            return false;
        }
        else {
            $('#ProductName').siblings("#nameWarning").remove();
        }

        // image validation
        if (dataImages == null) {
            if ($("#UploadImg").siblings("#DataFormAlert").length == 0)
                $("#UploadImg").parent("div").append("<span class='alert alert-danger p-1 m-2' id='DataFormAlert'>Must Add at least one image</span>");
            return false;
        } else {
            $(this).siblings("#DataFormAlert").remove();
        }

        return true;
    }

    const productEditValid = () => {
        // price validation
        if ($('#ProductEditPrice').val() < 0) {
            if ($('#ProductEditPrice').siblings("#priceWarning").length == 0)
                $('#ProductEditPrice').parent("div").append("<span class='alert alert-danger p-1 m-2' id='priceEditWarning'>Price can't be negative</span >");
            return false;
        }
        else {
            $('#ProductEditPrice').siblings("#priceEditWarning").remove();
        }

        // discount validation
        if ($('#ProductEditDiscount').val() < 0) {
            if ($('#ProductEditDiscount').siblings("#discaountEditMinWarning").length == 0)
                $('#ProductEditDiscount').parent("div").append("<span class='alert alert-danger p-1 m-2' id='discaountEditMinWarning'>Discount can't be negative</span >");
            return false;
        }
        else {
            $('#ProductEditDiscount').siblings("#discaountEditMinWarning").remove();
        }
        if ($('#ProductEditDiscount').val() > 100) {
            if ($('#ProductEditDiscount').siblings("#discaountEditMaxWarning").length == 0)
                $('#ProductEditDiscount').parent("div").append("<span class='alert alert-danger p-1 m-2' id='discaountEditMaxWarning'>Discount can't be bigger than 100</span >");
            return false;
        }
        else {
            $('#ProductEditDiscount').siblings("#discaountEditMaxWarning").remove();
        }

        // name validation
        if ($("#ProductEditName").val() == null || $("#ProductEditName").val().length == 0) {
            if ($('#ProductEditName').siblings("#nameEditWarning").length == 0)
                $('#ProductEditName').parent("div").append("<span class='alert alert-danger p-1 m-2' id='nameEditWarning'>you must enter a name</span >");
            return false;
        }
        else {
            $('#ProductEditName').siblings("#nameEditWarning").remove();
        }
        return true;
    }

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

    $(".ProductSearchBtn").click(function () {
        var SearchValue = $(".ProductSearch").val();
        var token = $('input[name="__RequestVerificationToken"]').val();
        var url = "/Products/EditorIndexSearch?product=" + SearchValue + "&__RequestVerificationToken=" + token;
        window.location.href = url;
        return false;
    });
});
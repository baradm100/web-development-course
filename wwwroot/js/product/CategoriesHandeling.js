$(function () {
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
                        options += '<option selected value="' + response.categories[i].id + '">' + response.categories[i].name + '</option>';
                    else
                        options += '<option value="' + response.categories[i].id + '">' + response.categories[i].name + '</option>'
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

    $("#categoriesDropDownList").change(function () {
        var categoryId = $("#categoriesDropDownList option:selected").val();
        var token = $('input[name="__RequestVerificationToken"]').val();
        var url = "/Products/EditorIndex?categoryId=" + categoryId + "&__RequestVerificationToken=" + token;
        window.location.href = url;
    })

    $("#AddNewCategoryBtn").click(function () {
        categories();
        $("#addCategoryModal").modal("show");
    })

    $("#addCategoryBtn").click(function () {
        $("#CategoryloadingSpinner").removeClass("d-none");
        $("#addCategoryBtn").addClass("d-none");
        $("#CategoryErrorIcon").addClass("d-none");
        $("#CategorySuccessIcon").addClass("d-none");
        var category = $("#CategoryName").val();
        var parentCategory = $(".parentCategory").val();
        console.log(parentCategory);
        var token = $('input[name="__RequestVerificationToken"]').val();
        var Success = false;
        $.ajax({
            url: "/Categories/",
            type: 'POST',
            dataType: 'json',
            data: {
                __RequestVerificationToken: token,
                Name: category,
                Parent: parentCategory,
            },
            success: function (result) {
                if (result.success) {
                    Success = true;
                    $("#CategoryloadingSpinner").addClass("d-none");
                    $("#addCategoryForm").removeClass("d-none");
                    $("#CategorySuccessIcon").removeClass("d-none");
                } else {
                    Success = false;
                    $("#CategoryloadingSpinner").addClass("d-none");
                    $("#addCategoryForm").removeClass("d-none");
                    $("#CategoryErrorIcon").removeClass("d-none");
                    $("#CategoryErrorIcon").text(result.textStatus);
                }
            },
            error: function (result) {
                console.log(result);
                return false
            },
            complete: function () {
                if (!Success) { $("#CategoryErrorIcon").removeClass("d-none"); }
                $("#CategoryloadingSpinner").addClass("d-none");
                $("#addCategoryForm").removeClass("d-none");
                $("#CategoryError").addClass("d-none");
            },
            timeout: 5000
        });
        return Success;
    });

    $('#closeCategoryBtn').click(function () {
        location.reload();
    });

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
                    $('.parentCategory').html('');
                    var options = '';
                    options += '<option value="Select">--</option>';
                    for (var i = 0; i < response.categories.length; i++) {
                            options += '<option value="' + response.categories[i].name + '">' + response.categories[i].name + '</option>'
                    }
                    $('.parentCategory').append(options);
                }
            },
            error: function (result) {
                console.log(result);
                return false
            },
            timeout: 5000,
        });
    }

});
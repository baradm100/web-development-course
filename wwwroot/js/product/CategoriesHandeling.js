$(function () {
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
               $('#categoriesDropDownList').html('');
               var options = '';
                options += '<option value="Select">All</option>';
                for (var i = 0; i < response.categories.result.length; i++) {
                    console.log(response.categories.result[i]);
                    options += '<option value="' + response.categories.result[i].id + '">' + response.categories.result[i].name + '</option>';
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
        var selected = $("#categoriesDropDownList option:selected").text();
        var cards = $("div.card");
        for (let i = 0; i < cards.length; i++) {
            var card = $(cards[i]);
            if ($(card).find(".category").text().includes(selected) || selected == "All")
                $(card).parent(".col").removeClass("d-none");
            else
                $(card).parent(".col").addClass("d-none");
        }
        
    })

    $("#AddNewCategoryBtn").click(function () {
        $("#addCategoryModal").modal("show");
    })

    $("#addCategoryBtn").click(function () {
        $("#CategoryloadingSpinner").removeClass("d-none");
        $("#addCategoryBtn").addClass("d-none");
        $("#CategoryErrorIcon").addClass("d-none");
        $("#CategorySuccessIcon").addClass("d-none");
        var category = $("#CategoryName").val();
        var token = $('input[name="__RequestVerificationToken"]').val();
        var Success = false;
        $.ajax({
            url: "/Categories/AddCategory/",
            type: 'POST',
            dataType: 'json',
            data: {
                __RequestVerificationToken: token,
                Name: category,
            },
            success: function (result) {
                if (result.success) {
                    Success = true;
                    console.log(result);
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

});
$(function () {
    var maxPrice = 10000;
    var $dropDownCategories = $("#categoriesDropDownAdvancedSearch");
    var $price = $("#priceRangeAdvancedSearch");
    var $name = $("#productNameAdvancedSearch");
    var $productSearchBtn = $("#searchBtnAdvancedSearch");
    var $branchSearchBtn = $("#branchBtnAdvancedSearch");
    var $branchName = $("#branchNameAdvancedSearch");


    // when page is ready append the categories:
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
                $dropDownCategories.html('');
                var options = '';
                options += '<option selected value="Select">All Categories</option>';
                for (var i = 0; i < response.categories.length; i++) {
                    options += '<option value="' + response.categories[i].id + '">' + response.categories[i].name + '</option>'
                }
                $dropDownCategories.append(options);
            }
        },
        error: function (result) {
            console.log(result);
            return false
        },
        timeout: 10000,
    });

    // when page is ready configure price attribute:
    $.ajax({
        url: "/products/maxPrice/json",
        type: 'GET',
        dataType: 'json',
        data: null,
        fail: function (xhr, textStatus, errorThrown) {
            Success = false;
            alert("Something went wrong in server")
        },
        success: function (response) {
            if (response.success == true) {
                Success = true;
                maxPrice = response.max
                $price.attr("max", maxPrice);
            }
        },
        error: function (result) {
            console.log(result);
            return false
        },
        complete: function () {
            $("#dynamicPrice").text(maxPrice);
            $price.val(maxPrice);
        },
        timeout: 10000,
    });

    const productAdvancedSearch = (categoryId, maximumPrice, name) => {
        var url = "/Products/AdvancedSearch?productName=" + name + "&maximumPrice=" + maximumPrice + "&categoryId=" + categoryId;
        window.location.href = url;
    };

    const branchAdvancedSearch = (name) => {
        var url = "/Branches?name=" + name;
        window.location.href = url;
    };

    const findSelectedDropdown = ($selector) => {
        return $selector.find('option:selected')
    }

    //events
    $productSearchBtn.on("click", function (event) {
        productAdvancedSearch(findSelectedDropdown($dropDownCategories), $price.val(), $name.val())
    });

    $branchSearchBtn.on("click", function (event) {
        branchAdvancedSearch($branchName.val());
    });

    $price.on("change", (event) => $("#dynamicPrice").text($price.val()));
});
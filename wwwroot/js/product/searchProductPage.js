$(function () {
    var $dropDownCategories = $("#categoriesDropDownList");
    var $price = $("#maxPriceSearch");
    var $name = $("#productNameSearch");
    var $productSearchBtn = $("#btnSearch");
    var $sortDropDown = $("#sortDropDown");
    var priceValid = true;

    $price.change(function () {
        if ($price.val() <= 0) {
            $price.addClass("alert alert-danger")
            priceValid = false;
        } else {
            $price.removeClass("alert alert-danger");
            priceValid = true;
        }
        if ($price.val() == "") {
            $price.removeClass("alert alert-danger");
            priceValid = true;
        }
    })

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

    const productAdvancedSearch = (categoryId, maximumPrice, name, sort) => {
        if (priceValid == false)
            return;
        var coin = getCookieValue("currency");
        var url = ""
        var sortValue = sort ? sort : "";
        if (maximumPrice <= 0)
            url = "/Products/AdvancedSearch?productName=" + name + "&maximumPrice=" + "&categoryId=" + categoryId + "&sort=" + sort;
        else {
            url = "/Products/AdvancedSearch?productName=" + name + "&maximumPrice=" + maximumPrice / coin + "&categoryId=" + categoryId + "&sort=" + sort;
        }
        window.location.href = url;
    };

    $productSearchBtn.on("click", function (event) {
        productAdvancedSearch(findSelectedDropdown($dropDownCategories), $price.val(), $name.val(), findSelectedDropdown($sortDropDown))
    });

    const findSelectedDropdown = ($selector) => {
        return $selector.find('option:selected').val();
    }

    const getCookieValue = (name) => (
        document.cookie.match('(^|;)\\s*' + name + '\\s*=\\s*([^;]+)')?.pop() || ''
    )
})
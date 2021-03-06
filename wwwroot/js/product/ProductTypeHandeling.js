$(function () {
    var editType = false;
    var productName = null;
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
        if (DuplicateSize() == true) {
            if ($(this).siblings("#SizeWarning").length == 0)
                $(this).parent("div").append("<div class='alert alert-danger' style='color: red' id='SizeWarning'>There is an Duplicate Sizes</div>");
            return false;
        } else {
            $(this).parent("#SizeWarning").remove()
        }
        editType = false;
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
                            if(i == 0)
                                options += `<input checked class="form-check-input" type="radio" id="${response.colors.result[i].name}" name="inlineRadioOptions" value="${response.colors.result[i].id}" style="background-color: #${response.colors.result[i].color}; border-radius: 18px;"></input>`
                            else
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

    const getProductsInColor = (Color, prodName) => {
        d = {
            color: rgb2hex(Color),
            name: prodName,
        }
        return $.ajax({
            url: "/Products/json",
            type: 'GET',
            dataType: 'json',
            data: d,
            success: function (result) {
                if (result.success == true) {
                    return result.types
                } else {
                    Success = false;
                }
            },
            timeout: 5000
        });
    }

    $(".editProductType").click(async function () {
        editType = true;
        var $btn = $(this);
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
                            if (("#" + response.colors.result[i].color).toLowerCase() == rgb2hex($btn.css("background-color")))
                                options += `<input disabled checked class="form-check-input checked" type="radio" id="${response.colors.result[i].name}" name="inlineRadioOptions" value="${response.colors.result[i].id}" style="background-color: #${response.colors.result[i].color}; border-radius: 18px;"></input>`
                            else
                                options += `<input disabled class="form-check-input" type="radio" id="${response.colors.result[i].name}" name="inlineRadioOptions" value="${response.colors.result[i].id}" style="background-color: #${response.colors.result[i].color}; border-radius: 18px;"></input>`
                        }
                        $('#Color').append(options);
                    }
                }
            },
            error: function (result) {
                console.log(result);
                return false
            },
            complete: async function () {
                var color = $btn.css("background-color");
                productName = $btn.attr("name");
                var types = await getProductsInColor(color, productName);
                $("#addGoodsModal").modal("show");
                for (let i = 0; i < types.types.length; i++) {
                    if (i != 0) {
                        var $good = $("#addGoodsForm").clone();
                        $good.find("input[type=radio]").each(function (i, x) {
                            $(x).attr("name", $("input").length);
                        });
                        $good.find("#Color").attr("disabled", true);
                        $good.find("#Quantity").val(types.types[i].quantity);
                        $good.find("#Size").val(types.types[i].size).change();
                        $good.insertBefore("#AnotherGood");
                    } else {
                        $("#Color").attr("disabled", true);
                        $("#Size").val(types.types[i].size).change();
                        $("#Size").attr("disabled", true);
                        $("#Quantity").val(types.types[i].quantity);
                    }
                };
                $(".modal-title").text("Edit Goods");
                $("button#addGoodsModalBtn").text("Save");
            },
            timeout: 5000,
        })
    });

    const uploadProductType = (token, color, size, quantity, prodName) => {
        var formData = new FormData();
        formData.append("__RequestVerificationToken", token);
        formData.append("Quantity", quantity);
        formData.append("ColorId", color);
        formData.append("productName", prodName);
        formData.append("Size", size);
        return $.ajax({
            url: "/ProductTypes/AddGoods/",
            type: 'POST',
            dataType: 'json',
            data: formData,
            cache: false,
            processData: false,
            contentType: false,
            success: function (result) {
                if (result.success == true) {
                    return true;
                } else {
                    return false;
                }
            },
            error: function (result) {
                console.log(result);
                return false
            },
            timeout: 5000
        });
    }

    const DuplicateSize = () => {
        var validated = {};
        for (var i = 0; i < $("select#Size").length; i++) {
            var $size = $($("select#Size")[i]);
            if (validated[$size.val()] != null) {
                // check if color already exist in it
                if (validated[$size.val()].includes($size.parents("#addGoodsForm").find("input:checked").val()))
                    return true;
                else
                    validated[$size.val()].push($size.parents("#addGoodsForm").find("input:checked").val());
            }
            validated[$size.val()] = [$size.parents("#addGoodsForm").find("input:checked").val()];
        }
        return false;
    };

    const QuantityNegativeOrFloatValue = () => {
        for (var i = 0; i < $("input#Quantity").length; i++) {
            if ($($("input#Quantity")[i]).val() < 0 || !Number.isInteger(Number($($("input#Quantity")[i]).val())))
                return true;
        }
        return false;
    };

    $("#addGoodsModalBtn").click(function () {
        if (DuplicateSize() == true) {
            if($(this).siblings("#SizeWarning").length == 0)
                $(this).parent("div").append("<div class='alert alert-danger' style='color: red' id='SizeWarning'>There is an Duplicate Sizes</div>");
            return false;
        } else {
            $(this).parent("#SizeWarning").remove()
        }
        if (QuantityNegativeOrFloatValue() == true) {
            if($(this).siblings("#InvalidWarning").length == 0)
                $(this).parent("div").append("<div class='alert alert-danger' style='color: red' id='InvalidWarning'>There is an invalid Inputs</div>");
            return false;
        }
        else {
            $(this).parent("#SizeWarning").remove()
        }
        $(this).siblings("#InvalidWarning").remove()
        $("#loadingSpinner").removeClass("d-none");
        $("#addGoodsForm").addClass("d-none");
        $("#errorIcon").addClass("d-none");
        $("#successIcon").addClass("d-none");
        $(".modal-body").find(".goods").each(async function (i, goods) {
            var color = $(goods).find('input:checked').val();
            if (!color) {
                color = $(goods).find("#Color").css("background-color");
            }
            var size = $(goods).find("#Size").val();
            var quantity = $(goods).find("#Quantity").val();
            var token = $('input[name="__RequestVerificationToken"]').val();
            var uploadSuccess = await uploadProductType(token, color, size, quantity, productName);
            if (await uploadSuccess == false) {
                $("#errorIcon").removeClass("d-none");
            }
            $("#successIcon").removeClass("d-none");
            $("#loadingSpinner").addClass("d-none");
            $("#addGoodsForm").removeClass("d-none");
            $("#deleteGoodsModalBtn").addClass("d-none");
        });
    });

    $('#closeBtn').click(function () {
        editType = false;
        $(".modal-title").text("Add Goods");
        $("#deleteGoodsModalBtn").addClass("d-none");
        $("#addGoodsModalBtn").removeClass("d-none");
        productName = null;
        location.reload();
    });

    const addAnotherGood = () => {
        var $good = $("#addGoodsForm").clone();
        $good.find("input[type=radio]").each(function (i, x) {
            $(x).attr("name", $("input").length);
        });
        $good.find("#Size").attr("disabled", false);
        $good.find("#Quantity").val("0");
        $good.insertBefore("#AnotherGood");
    }

    $("#addAnotherGood").on("click", function () {
        if (editType) {
            if ($("div.goods").length < 6) {
                addAnotherGood();
            } else {
                $(this).attr("disabled", true);
                $(this).parent("div").append("<div><span class='alert alert-danger p-1 m-2' id='maximumSizes' > there is no more available sizes</span>");
            }
        } else {
            addAnotherGood();
        }
    })
});
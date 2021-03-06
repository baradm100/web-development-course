const getCookieValue = (name, defaultValue = 1) =>
  document.cookie.match("(^|;)\\s*" + name + "\\s*=\\s*([^;]+)")?.pop() ||
  defaultValue;

const sign = {
  1: "$",
  2: "₪",
  3: "€",
  4: "£",
};

function addItems(response) {
  var currency = getCookieValue("currency");
  var cSign = sign[getCookieValue("currencySign")];
  $("#table-body").empty();

  for (let index = 0; index < response.length; index++) {
    $("#table-body").append(
      '<tr><th scope="row">' +
        (Number(index) + 1) +
        "</th><td>" +
        response[index].productName +
        "</td><td>" +
        response[index].amount +
        "</td><td>" +
        (Number(response[index].totalPrice) * currency).toFixed(2) +
        cSign +
        "</td></tr>"
    );
  }
}

function updateCheckoutModal() {
  getCart();
  GetSummary();
}

function getCart() {
  $.ajax({
    url: "/Orders/GetCart",
    type: "GET",
    dataType: "json",
    data: null,
    fail: function (xhr, textStatus, errorThrown) {
      Success = false;
      alert("Something went wrong in server");
    },
    success: function (response) {
      if (response != null) {
        addItems(response);
      } else {
        Success = false;
        alert("Something went wrong in server");
      }
    },
    error: function (result) {
      console.log(result);
      return false;
    },
    timeout: 5000,
  });
}

function GetSummary() {
  $.ajax({
    url: "/Orders/GetSummary",
    type: "GET",
    dataType: "json",
    data: null,
    fail: function (xhr, textStatus, errorThrown) {
      Success = false;
      alert("Something went wrong in server");
    },
    success: function (response) {
      Success = true;
      if (response != null) {
        let cSign = sign[getCookieValue("currencySign")];
        let subtotalPrice = Number(Number(response.data.totalPrice).toFixed(2));
        $("#subtotal-price").text(subtotalPrice + cSign);
        $("#subtotal-price").val(subtotalPrice);
        updatePrice();
      }
    },
    error: function (result) {
      console.log(result);
      return false;
    },
    timeout: 5000,
  });
}

function updatePrice() {
  var currency = getCookieValue("currency");
  var cSign = sign[getCookieValue("currencySign")];
  deliveryPrice = 0;
  if ($(".delivery-border")[0]) {
    let id = $($(".delivery-border")[0]).attr("id");
    id = id.split("_");
    if (id[1] > 1) {
      deliveryPrice = Number(id[1]) * currency;
    }
    $("#delivery_price").text(deliveryPrice.toFixed(2));
  } else {
    $("#delivery_price").text("0");
  }

  $("#total-price").text(
    Number(
      (Number(deliveryPrice) + Number($("#subtotal-price").val())).toFixed(2)
    ) + cSign
  );
  $("#total-price").val(
    Number(
      (Number(deliveryPrice) + Number($("#subtotal-price").val())).toFixed(2)
    )
  );
}

$(function () {
  GetSummary();
  getUserInfo();
  validationHelper();
  GetBranches();
  getCart();

  //#region Listeners

  $("#next_delivery").click(function () {
    navigetePane(
      $("#delivery-tab"),
      $("#delivery_methods"),
      $("#details-tab"),
      $("#details_input")
    );
  });

  $("#next_addr").click(function () {
    navigetePane(
      $("#details-tab"),
      $("#details_input"),
      $("#payment-tab"),
      $("#payment_input")
    );
  });

  $("#checkout_partial").click(function () {
    GetSummary();
  });

  function validationHelper() {
    "use strict";
    // Fetch all the forms we want to apply custom Bootstrap validation styles to
    var forms = document.querySelectorAll(".needs-validation");

    // Loop over them and prevent submission
    Array.prototype.slice.call(forms).forEach(function (form) {
      form.addEventListener(
        "input",
        function (event) {
          if (!form.checkValidity()) {
            event.preventDefault();
            event.stopPropagation();
          }

          form.classList.add("was-validated");
          allowPurchase();
        },
        false
      );
    });
  }

  $(".orderSummaryClose").click(function () {
    cleanorderSummary();
    var url = "/Home/Index";
    window.location.href = url;

    return false;
  });

  $("#checkoutModalClose").click(function () {
    cleanCheckoutModal();
  });

  //#endregion

  //#region Helpers

  function navigetePane($srcTab, $srcPane, $dstTab, $dstPane) {
    $srcPane.removeClass("show");
    $srcPane.removeClass("active");
    $srcTab.removeClass("active");
    $srcTab.prop("aria-selected", false);
    $dstTab.addClass("active");
    $dstTab.prop("aria-selected", true);
    $dstPane.addClass("show");
    $dstPane.addClass("active");
  }

  function initStores(branches) {
    let index = 1;
    branches.forEach(function (branch) {
      $("#branches_list").append(
        '<li><a class="dropdown-item" id="branch_' +
          index +
          '" >' +
          branch.name +
          "</a></li>"
      );
      $("#branch_" + index).click(function () {
        $("#selected_branch").text($(this).text());
        $("#selected_branch").val($(this).text());
      });
      index++;
    });
  }

  $("#delivery_1").click(function () {
    if ($("#selected_branch").val() != "") {
      $("#delivery_1").addClass("delivery-border");
      $("#delivery_53").removeClass("delivery-border");
    }
    deliveryOption();
  });

  $("#delivery_53").click(function () {
    if ($("#delivery_53").hasClass("delivery-border")) {
      $("#delivery_53").removeClass("delivery-border");
    } else {
      $("#delivery_1").removeClass("delivery-border");
      $("#delivery_53").addClass("delivery-border");
      $("#selected_branch").text("");
      $("#selected_branch").val("");
    }
    deliveryOption();
  });

  function deliveryOption() {
    allowPurchase();
    updatePrice();
  }

  function updateInfo(name, email, phone) {
    $("#floatingName").val(name);
    $("#floatingEmail").val(email);
    $("#floatingPhone").val(phone);
  }

  function allowPurchase() {
    allow = true;

    $("form").each(function () {
      if (!this.checkValidity()) {
        allow = false;
      }
    });

    if (!$(".delivery-border")[0]) {
      allow = false;
    }

    if (allow) {
      if (isDateValid()) {
        $("#place-order").removeAttr("disabled");

        if (!$("#floatingMMFeedback").hasClass("visually-hidden")) {
          $("#floatingMMFeedback").addClass("visually-hidden");
        }
      } else {
        $("#floatingMMFeedback").removeClass("visually-hidden");
      }
    } else {
      if (!$("#place-order").attr("disabled")) {
        $("#place-order").prop("disabled", true);
      }
      if (!$("#floatingMMFeedback").hasClass("visually-hidden")) {
        $("#floatingMMFeedback").addClass("visually-hidden");
      }
    }
  }

  function isDateValid() {
    let today = new Date();
    month = $("#floatingMM").val();
    year = $("#floatingYY").val();

    let isValid = true;

    if (today.getFullYear() % 100 > year) {
      isValid = false;
    }
    if (today.getMonth() + 1 > month && today.getFullYear() % 100 == year) {
      isValid = false;
    }

    return isValid;
  }

  //#endregion

  //#region AJAX

  function getUserInfo() {
    $.ajax({
      url: "/Users/Info",
      type: "GET",
      dataType: "json",
      data: null,
      fail: function (xhr, textStatus, errorThrown) {
        Success = false;
        alert("Something went wrong in server");
      },
      success: function (response) {
        Success = true;
        if (response != null) {
          updateInfo(
            response.data.name,
            response.data.email,
            response.data.phone
          );
        }
      },
      error: function (result) {
        console.log(result);
        return false;
      },
      timeout: 5000,
    });
  }

  function GetBranches() {
    $.ajax({
      url: "/Branches/GetData",
      type: "GET",
      dataType: "json",
      data: null,
      fail: function (xhr, textStatus, errorThrown) {
        Success = false;
        alert("Something went wrong in server");
      },
      success: function (response) {
        if (response != null) {
          Success = true;
          initStores(response);
        } else {
          Success = false;
          alert("Something went wrong in server");
        }
      },
      error: function (result) {
        console.log(result);
        return false;
      },
      timeout: 5000,
    });
  }

  $("#place-order").click(function () {
    $.ajax({
      url: "/Orders/PlaceOrder",
      type: "Post",
      dataType: "json",
      data: {
        totalPrice: $("#total-price").val(),
        deliveryOption: $(".delivery-border").attr("id"),
        branchName: $("#selected_branch").val(),
        phone: $("#floatingPhone").val(),
        address: {
          city: $("#floatingCity").val(),
          street: $("#floatingStreet").val(),
          buildingNumber: $("#floatingNumber").val(),
        },
      },
      fail: function (xhr, textStatus, errorThrown) {
        Success = false;
        alert("Something went wrong in server " + textStatus);
        console.log("fail " + textStatus);
        cleanCheckoutModal();
      },
      success: function (response) {
        if (response.success) {
          Success = true;
          cleanCheckoutModal();
          openOrderApprovment(response);
        } else {
          alert("Something went wrong in server " + response.textStatus);
          cleanCheckoutModal();
        }
      },
      error: function (result) {
        console.log("error");
        console.log(result);
        cleanCheckoutModal();
        return false;
      },
      timeout: 5000,
    });
  });

  //#endregion

  //#region Close Modal

  function cleanCheckoutModal() {
    $("#checkoutModal").modal("hide");
    $("#floatingCity").val(""),
      $("#floatingStreet").val(""),
      $("#floatingNumber").val("");
    $("#floatingCreditNumber").val("");
    $("#floatingMM").val("");
    $("#floatingYY").val("");
    $("#floatingCVV").val("");
    $(".needs-validation").removeClass("was-validated");
    $(".delivery").removeClass("delivery-border");
    $("#selected_branch").val("");
    $("#selected_branch").text("");

    if (!$("#place-order").attr("disabled")) {
      $("#place-order").prop("disabled", true);
    }

    if (!$("#floatingMMFeedback").hasClass("visually-hidden")) {
      $("#floatingMMFeedback").addClass("visually-hidden");
    }

    $("#total-price").text(
      Number(Number($("#subtotal-price").val())).toFixed(2)
    );
    $("#total-price").val(
      Number(Number($("#subtotal-price").val())).toFixed(2)
    );
    $("#delivery_price").text("0");

    GetSummary();
  }

  function cleanorderSummary() {
    $("#store_info").addClass("d-none");
    $("#store_name").text("");
    $("#delivery_guy").addClass("d-none");
    $("#phone_number").text("");
  }

  //#endregion

  //#region Open Modal

  function openOrderApprovment(response) {
    $("#orderNumer").text(response.data.orderId);
    $("#total_price_sum").text(response.data.price + " " + currencySign);

    if (response.data.branchName != null) {
        $("#store_info").removeClass("d-none");
        $("#store_name").removeClass("d-none");
        $("#store_name").text("" + response.data.branchName);
    } else {
        $("#delivery_guy").removeClass("d-none");
        $("#phone_number").removeClass("d-none");
        $("#phone_number").text("" + response.data.phone);
    }
     $("#orderSummary").modal("show");

  }

  //#endregion
});

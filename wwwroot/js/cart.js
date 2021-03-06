let currecny = ["$", "₪", "€", "£"];
let sizes = ["XS", "S", "M", "L", "XL", "XXL"];

$(function () {
  disableMiniCart();
  GetSummary();
  initAmountListeners();
  initDeleteListener();
  initEditListener();

  window.addEventListener("locationchange", function () {
    $(".mini-cart").prop("disabled", false);
  });

  function disableMiniCart() {
    $(".mini-cart").prop("disabled", true);
  }

  function initAmountListeners() {
    $(".amount").change(function () {
      var index = $(this).attr("id");
      var modelId = $(this).attr("modelId");

      updateAmount(modelId, index);
    });
  }

  function initDeleteListener() {
    $(".deleteButton").click(function () {
      var index = $(this).attr("id");
      var modelId = $(this).attr("modelId");
      deleteItem(modelId, index);
    });
  }

  function initEditListener() {
    $(".editBtn").click(function () {
      var index = $(this).attr("id");
      var modelId = $(this).attr("modelId");
      editItem(modelId, index);
    });
  }

  function GetSummary() {
    updateCart(false);
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
          $("#summary-price").text(
            Number(Number(response.data.midPrice).toFixed(2)) +
              " " +
              response.data.sign
          );
          $("#summary-discount").text(
            Number(Number(response.data.saving).toFixed(2)) +
              " " +
              response.data.sign
          );
          $("#summary-total").text(
            Number(Number(response.data.totalPrice).toFixed(2)) +
              " " +
              response.data.sign
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

  function updateAmount(id, index) {
    $.ajax({
      url: "/Orders/UpdateAmount",
      type: "Post",
      dataType: "json",
      data: {
        id: id,
        amount: $("#" + index).val(),
      },
      fail: function (xhr, textStatus, errorThrown) {
        Success = false;
        alert("Something went wrong in server " + textStatus);
      },
      success: function (response) {
        Success = true;
        getItemFinalPrice(id, index);
        updateCheckoutModal();
      },
      error: function (result) {
        console.log(result);
        return false;
      },
      timeout: 5000,
    });
  }

  function getItemFinalPrice(orderId, rowIndex) {
    $.ajax({
      url: "/Orders/GetItemFinalPrice?orderId=" + orderId,
      type: "GET",
      dataType: "json",
      data: null,
      fail: function (xhr, textStatus, errorThrown) {
        Success = false;
        alert("Something went wrong in server");
      },
      success: function (response) {
        Success = true;
        $("#total-price_" + rowIndex).text(
          Number(Number(response.data.totalPrice).toFixed(2)) +
            " " +
            response.data.sign
        );
        GetSummary();
      },
      error: function (result) {
        console.log(result);
        return false;
      },
      timeout: 5000,
    });
  }

  function editItem(modelId, index) {
    alert("to be wriiten later");
    // to be wriiten later, need to pop modal
    GetOrderItemData(modelId, index);
  }

  function GetOrderItemData(modelId, index) {
    $.ajax({
      url: "/Orders/GetOrderItemData?orderItemId=" + modelId,
      type: "GET",
      dataType: "json",
      data: null,
      fail: function (xhr, textStatus, errorThrown) {
        Success = false;
        alert("Something went wrong in server " + textStatus);
      },
      success: function (response) {
        Success = true;
        // UPDATE The Item Data  {color = color, amount = amount, size = size}
        $("#size_" + index).text(sizes[response.data.size]);
        $("#color_" + index).css("background-color", response.data.color);
        $(".amount#" + index).val(response.data.amount);
        getItemFinalPrice(modelId, index);
      },
      error: function (result) {
        console.log(result);
        return false;
      },
      timeout: 5000,
    });
  }

  function deleteItem(modelId, index) {
    $.ajax({
      url: "/Orders/DeleteByUser?orderItemId=" + modelId,
      type: "GET",
      dataType: "json",
      data: null,
      fail: function (xhr, textStatus, errorThrown) {
        Success = false;
        alert("Something went wrong in server " + textStatus);
      },
      success: function (response) {
        Success = true;
        if (response.isLastItem) {
          location.reload();
        } else {
          removeCard(index);
          GetSummary();
          updateCheckoutModal();
        }
      },
      error: function (result) {
        console.log(result);
        return false;
      },
      timeout: 5000,
    });
  }

  function removeCard(index) {
    $("#card_" + index).remove();
  }
});

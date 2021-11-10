$(function () {
  // Handle updating color
  $("#productForm input[name=colorSelection]").change(function (e) {
    const sizesBySelectedColor = SizesCountByColorIds[e.target.value];
    const sizesInputs = $("#productForm input[name=Size]");

    for (const sizeInput of sizesInputs) {
      // Enable only the sizes we have in stock
      sizeInput.disabled = sizesBySelectedColor[sizeInput.value] <= 0;
    }

    // Reset other depended fields (size, quantity and add to cart)
    const checkedSize = $("#productForm input[name=Size]:checked")[0];
    if (checkedSize) {
      checkedSize.checked = false;
    }

    $("#Quantity").prop("disabled", true);
    $("#addToCart").prop("disabled", true);
  });

  // Handle updating size
  $("#productForm input[name=Size]").change(function (e) {
    const color = $("#productForm input[name=colorSelection]:checked").val();
    const size = e.target.value;
    let maxQuantity = SizesCountByColorIds[color][size];
    const quantitySelection = $("#Quantity").empty()[0];

    maxQuantity = Math.min(Number(maxQuantity), 10)
        
    for (let i = 1; i <= maxQuantity; i++) {
      // Adding all the available quantity, will set `1` as selected
      quantitySelection.append(new Option(i, i, i === 1, i === 1));
    }

    // Enable the quantity selection and add to cart button
    quantitySelection.disabled = false;
    $("#addToCart").prop("disabled", false);
  });

    $("#back_btn").click(function () {
        var last_url = document.referrer;
        window.location.replace(last_url);
    });

  const indicateLoading = () => {
    $("#loadingSpinner").removeClass("d-none");
    $("#addToCart").addClass("d-none");
    $("#successNotification").addClass("d-none");
    $("#failNotification").addClass("d-none");
  };

  const clearLoading = () => {
    $("#loadingSpinner").addClass("d-none");
    $("#addToCart").removeClass("d-none");
  };

  $("#productForm").submit(function (event) {
    event.preventDefault();

    if (!isLoggedIn) {
      // The user is not logged in, showing the modal
      const notLoggedModal = new bootstrap.Modal(
        document.getElementById("notLoggedModal")
      );
      notLoggedModal.show();
      return;
    }

    const form = new FormData();
    form.set("productId", ProductId);
    form.set(
      "colorId",
      +$("#productForm input[name=colorSelection]:checked").val()
    );
    form.set("size", $("#productForm input[name=Size]:checked").val());
    form.set("quantity", +$("#Quantity").val());
    form.set(
      "__RequestVerificationToken",
      $('input[name="__RequestVerificationToken"]').val()
    );

    indicateLoading();

    $.ajax({
      url: "/Orders/AddToCart/",
      type: "POST",
      data: form,
      cache: false,
      processData: false,
      contentType: false,
      dataType: "json",
      fail: function (xhr) {
        let errorMessage = "Unknown error, please try again";
        if (xhr.responseJSON && xhr.responseJSON.errorMessage) {
          errorMessage = xhr.responseJSON.errorMessage;
        }

        $("#failNotification").text(errorMessage);
        $("#failNotification").removeClass("d-none");
      },
      success: function (response) {
        $("#successNotification").removeClass("d-none");
        updateCart();
      },
      error: function (xhr) {
        let errorMessage = "Unknown error, please try again";
        if (xhr.responseJSON && xhr.responseJSON.errorMessage) {
          errorMessage = xhr.responseJSON.errorMessage;
        }

        $("#failNotification").text(errorMessage);
        $("#failNotification").removeClass("d-none");
        return false;
      },
      complete: function () {
        clearLoading();
      },
      timeout: 5000,
    });
  });
});

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
    $("#productForm input[name=Size]:checked")[0].checked = false;
    $("#Quantity")[0].disabled = true;
    $("#addToCart")[0].disabled = true;
  });

  // Handle updating size
  $("#productForm input[name=Size]").change(function (e) {
    const color = $("#productForm input[name=colorSelection]:checked")[0].value;
    const size = e.target.value;
    const maxQuantity = SizesCountByColorIds[color][size];
    const quantitySelection = $("#Quantity").empty()[0];

    for (let i = 1; i <= maxQuantity; i++) {
      // Adding all the available quantity, will set `1` as selected
      quantitySelection.append(new Option(i, i, i === 1));
    }

    // Enable the quantity selection and add to cart button
    quantitySelection.disabled = false;
    $("#addToCart")[0].disabled = false;
  });
});

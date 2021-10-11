function openCart() {
  const cartDropdown = new bootstrap.Dropdown(
    document.getElementById("quickCartDropdown")
  );
  cartDropdown.show();
}

function createCarouse(item) {
  const carouseDiv = document.createElement("div");
  const carouselId = "carouselQuickCartIndicators-" + item.id;
  carouseDiv.id = carouselId;
  carouseDiv.classList.add("carousel", "slide", "col");
  carouseDiv.dataset.bsRide = "carousel";
  const carouseIndicators = document.createElement("div");
  carouseIndicators.classList.add("carousel-indicators");
  const carouseInner = document.createElement("div");
  carouseInner.classList.add("carousel-inner");

  for (let i = 0; i < item.productImages.length; i++) {
    const button = document.createElement("button");
    button.type = "button";
    button.dataset["bsTarget"] = "#" + carouselId;
    button.dataset["bsSlideTo"] = i;

    const imgDiv = document.createElement("div");
    imgDiv.classList.add("carousel-item");
    const img = document.createElement("img");
    img.classList.add("d-block", "w-100", "h-100");
    img.width = 100;
    img.height = 250;
    img.src = `data:image/png;base64,${item.productImages[i]}`;
    imgDiv.appendChild(img);

    if (i === 0) {
      button.classList.add("active");
      imgDiv.classList.add("active");
    }

    carouseIndicators.appendChild(button);
    carouseInner.appendChild(imgDiv);
  }

  carouseDiv.appendChild(carouseIndicators);
  carouseDiv.appendChild(carouseInner);
  carouseDiv.innerHTML += `
  <button class="carousel-control-prev" type="button" data-bs-target="#${carouselId}"
  data-bs-slide="prev">
  <span class="carousel-control-prev-icon" aria-hidden="true"></span>
  <span class="visually-hidden">Previous</span>
</button>
<button class="carousel-control-next" type="button" data-bs-target="#${carouselId}"
  data-bs-slide="next">
  <span class="carousel-control-next-icon" aria-hidden="true"></span>
  <span class="visually-hidden">Next</span>
</button>`;

  return carouseDiv;
}

function createDetails(item) {
  const detailsDiv = document.createElement("div");
  detailsDiv.classList.add("col", "cartItemDetails");
  detailsDiv.innerHTML += `
  <span>
    <a href="/Products/Details/${item.productId}" class="link-primary">
    ${item.productName}
    </a>
  </span>
  <br />
  <span class="text-secondary">
  ${item.color} / ${item.size}
  </span>
  <br />
  <span class="text-secondary">
  ${item.totalPrice}$
  </span>
    `;
  const editItemDiv = document.createElement("div");
  const selectAmount = document.createElement("select");
  selectAmount.classList.add("form-select");
  editItemDiv.appendChild(selectAmount);

  for (let i = 1; i <= item.productQuantity; i++) {
    const option = document.createElement("option");
    option.value = i;
    option.text = i;
    selectAmount.add(option);
  }

  editItemDiv.innerHTML += `
  <button type="button" class="btn btn-outline-danger deleteButton" onClick="deleteItemFromCart(${item.id})">
    <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor"
    class="bi bi-trash-fill" viewBox="0 0 16 16">
        <path
        d="M2.5 1a1 1 0 0 0-1 1v1a1 1 0 0 0 1 1H3v9a2 2 0 0 0 2 2h6a2 2 0 0 0 2-2V4h.5a1 1 0 0 0 1-1V2a1 1 0 0 0-1-1H10a1 1 0 0 0-1-1H7a1 1 0 0 0-1 1H2.5zm3 4a.5.5 0 0 1 .5.5v7a.5.5 0 0 1-1 0v-7a.5.5 0 0 1 .5-.5zM8 5a.5.5 0 0 1 .5.5v7a.5.5 0 0 1-1 0v-7A.5.5 0 0 1 8 5zm3 .5v7a.5.5 0 0 1-1 0v-7a.5.5 0 0 1 1 0z">
        </path>
    </svg>
    <span class="d-none d-sm-inline d-md-inline d-lg-none">Delete From Cart</span>
  </button>
    `;
  detailsDiv.appendChild(editItemDiv);

  return detailsDiv;
}

function createItemElm(item) {
  const mainLi = document.createElement("li");
  mainLi.classList.add("row", "cartItem");

  mainLi.appendChild(createCarouse(item));
  mainLi.appendChild(createDetails(item));

  return mainLi;
}

function renderCart(items) {
  const cartElm = $("#quickCartItemsList");
  cartElm.empty();

  const lastItemId = items.at(-1)?.id;

  for (const item of items) {
    cartElm.append(createItemElm(item));
    const select = cartElm.find("select").last();

    select.val(item.amount);
    select.change(updateItemQuantity(item.id));

    if (item.id !== lastItemId) {
      cartElm.append(`
        <li>
            <hr class="dropdown-divider">
        </li>
    `);
    }
  }

  $("#quickCartTotal").text(items.length);
}

function updateCart(shouldOpen = true) {
  $.ajax({
    type: "GET",
    url: "/Orders/GetCart",
    fail: function (xhr, textStatus, errorThrown) {
      console.error("fail", arguments);
    },
    success: function (response) {
      console.log("success", arguments);
      renderCart(response);
    },
    error: function (result) {
      console.log("error", arguments);
      return false;
    },
    complete: function () {
      if (shouldOpen) {
        openCart();
      }
    },
    timeout: 5000,
  });
}

function showCartLoading() {
  const loadingDiv = document.getElementById("quickCartLoading");
  const quickCartItemsList = document.getElementById("quickCartItemsList");
  loadingDiv.classList.add("d-flex");
  loadingDiv.classList.remove("d-none");

  quickCartItemsList.classList.add("d-none");
  quickCartItemsList.classList.remove("d-flex");
}

function showCartWasLoaded() {
  const loadingDiv = document.getElementById("quickCartLoading");
  const quickCartItemsList = document.getElementById("quickCartItemsList");
  loadingDiv.classList.add("d-none");
  loadingDiv.classList.remove("d-flex");

  quickCartItemsList.classList.remove("d-none");
}

function deleteItemFromCart(itemId) {
  showCartLoading();

  const form = new FormData();
  form.set(
    "__RequestVerificationToken",
    $('input[name="__RequestVerificationToken"]').val()
  );

  $.ajax({
    type: "DELETE",
    url: "/Orders/DeleteOrderItem/" + itemId,
    data: form,
    cache: false,
    processData: false,
    contentType: false,
    dataType: "json",
    fail: function (xhr, textStatus, errorThrown) {
      console.log("fail", arguments);
    },
    success: function (response) {
      updateCart();
    },
    error: function (result) {
      console.log("error", arguments);
      return false;
    },
    complete: function () {
      openCart();
      showCartWasLoaded();
    },
    timeout: 5000,
  });
}

function updateItemQuantity(itemId) {
  return (e) => {
    const updatedValue = e.target.value;
    const form = new FormData();
    form.set("id", itemId);
    form.set("amount", updatedValue);
    showCartLoading();

    $.ajax({
      type: "POST",
      url: "/Orders/UpdateAmount/" + itemId,
      data: form,
      cache: false,
      processData: false,
      contentType: false,
      dataType: "json",
      fail: function (xhr, textStatus, errorThrown) {
        console.log("fail", arguments);
      },
      success: function (response) {
        console.log("success", arguments);
        updateCart();
      },
      error: function (result) {
        console.log("error", arguments);
        return false;
      },
      complete: function () {
        openCart();
        showCartWasLoaded();
      },
      timeout: 5000,
    });
  };
}
$(function () {
  const DEFAULT_CURRENCY = 1; // USD

  const deleteCookie = (name) => {
    document.cookie = `${name}=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/`;
  };
  
  const setCookie = (name, value, exdays = 100) => {
    const expires = new Date();
    expires.setTime(expires.getTime() + exdays * 24 * 60 * 60 * 1000);

    // Making sure the cookie will be shared a cors the pages
    document.cookie = `${name}=${value}; expires=${expires.toUTCString()}; path=/`;
  };

  var currencisSign = {
    USD: 1,
    ILS: 2,
    EUR: 3,
    GBP: 4,
  };
  $currencyButton = $(".currencyBtn");
  var currentCurrency = document.cookie["currency"];

  $currencyButton.click(function () {
    var selectedCurrency = $(this).children("i").attr("id");
    if (currentCurrency == selectedCurrency) return true;
    $.ajax({
      url: "/currency/json?currency=" + selectedCurrency,
      type: "GET",
      dataType: "json",
      success: function (result) {
        deleteCookie("currency");
        deleteCookie("currencySign");
        if (result.success) {
          setCookie("currencySign", currencisSign[selectedCurrency]);
          setCookie("currency", result.value);
          return true;
        }
        setCookie("currencySign", currencisSign[DEFAULT_CURRENCY]);
        setCookie("currency", DEFAULT_CURRENCY);
      },
      error: function (result) {
        console.log(result);
        deleteCookie("currency");
        deleteCookie("currencySign");
        setCookie("currencySign", currencisSign[DEFAULT_CURRENCY]);
        setCookie("currency", DEFAULT_CURRENCY);

        return false;
      },
      complete: function () {
        document.location.reload();
      },
      timeout: 5000,
    });
  });
});

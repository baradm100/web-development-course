$(function () {
    var currencisSign = {
        "USD": 1,
        "ILS": 2,
        "EUR": 3,
        "GBP": 4,
    }
    $currencyButton = $(".currencyBtn");
    var currentCurrency = document.cookie["currency"];

    $currencyButton.click(function () {
        var selectedCurrency = $(this).children("i").attr("id");
        if (currentCurrency == selectedCurrency)
            return true
        $.ajax({
            url: "/currency/json?currency=" + selectedCurrency,
            type: 'GET',
            dataType: 'json',
            success: function (result) {
                if (result.success) {
                    document.cookie = "currencySign=" + currencisSign[selectedCurrency];
                    document.cookie = "currency=" + result.value;
                    return true;
                }
                document.cookie = "currency=" + 1 + ",currencySign=" + currencisSign["USD"];
            },
            error: function (result) {
                console.log(result);
                document.cookie = "currency=" + 1 + ",currencySign=" + currencisSign["USD"];
                return false;
            },
            complete: function () {
                document.location.reload();
            },
            timeout: 5000
        });
    })
});
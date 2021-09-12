$(function () {
    $('.btnAddGoods').click(function () {
        $(this).parent().css("background", "yellow");
        });
    $('.btnDeleteProduct').click(function () {
        $(this).parent().css("background", "grey");
    });
});
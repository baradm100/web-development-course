$(function () {
    $(".deleteBranchBtn").click(function () {
        var BranchId = $(this).parent("div").attr("id");
        var token = $('input[name="__RequestVerificationToken"]').val();
        var url = "/Branches/Delete?id=" + BranchId + "&__RequestVerificationToken=" + token;
        window.location.href = url;
        return false;
    });

});
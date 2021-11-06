$(function () {
    $("#register").click(function () {
        $("#confirm-error").addClass("d-none")

        if (validPassword()) {
            createUser()
            return true;
        }
        else {
            $("#confirm-error").removeClass("d-none")
            return false;
        }
    })


    function validPassword() {

        return ($("#Confirm-Password").val() === $("#password").val())
    }

    function createUser() {
        console.log("create user")
    }



});
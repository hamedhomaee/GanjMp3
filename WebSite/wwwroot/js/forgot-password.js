var valid;

$(document).ready(function() {
    var email = $("#Model_Email");

    $("#forgot_password_submit").on("click", function(event) {
        event.preventDefault();

        valid = true;
        warn(!email.val().match(/^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/gm), "آدرس ایمیل اشتباه است", email);

        if (valid) {
            $.ajax({
                method: "POST",
                url: window.location.origin + "/forgotpassword",
                data: $("form").serialize()
            }).fail(function(res) {
                alert("خطا! لطفا دوباره تلاش کنید.");
            }).done(function(res) {
                if (res == "success") {
                    window.location.href = window.location.origin + "?password=reset";
                } else {
                    alert("ایمیل اشتباه است، دوباره امتحان کنید.");
                }
            });
        }
    });

    resetInput(email);
});

function warn(condition, warning, element) {
    if (condition) {
        valid = false;
        element.css("background-color", "rgba(196, 85, 73, .3)");
        element.val(warning);
    }
}

function resetInput(element) {
    element.on("focus", function() {
        if (element.css("background-color") != "rgb(255, 255, 255)") {
            element.val("");
            element.css("background-color", "white");
        }
    });
}
var valid;

$(document).ready(function() {
    $("#reset-password-submit").on("click", function(event) {
        event.preventDefault();

        var password = $("#ViewModel_Password");
        var confirmPassword = $("#ViewModel_ConfirmPassword");

        valid = true;
        warn(!password.val().match(/^[a-zA-Z0-9]{8,30}$/gm), "کلمه عبور بین ۸ تا ۳۰ کاراکتر الزامی است", password, toTextElement);
        warn((confirmPassword.val().localeCompare(password.val()) != 0), "کلمه عبور یکسان نیست", confirmPassword, toTextElement);

        if (valid) {
            $.ajax({
                method: "POST",
                url: window.location.origin + "/resetpassword",
                data: $("#reset-password-form").serialize()
            }).fail(function(res) {
                alert("خطا! لطفا دوباره تلاش کنید.");
            }).done(function(res) {
                if (res === "success") {
                    window.location.href = window.location.origin + "/?pass=true";
                } else {
                    window.location.href = window.location.origin + "/?error=true";
                }
            });
        }

        resetInput(password, true);
        resetInput(confirmPassword, true);
    });
});

function warn(condition, warning, element, callback) {
    if (condition) {
        valid = false;
        if (callback != null) {
            callback(element);
        }
        element.css("background-color", "rgba(196, 85, 73, .3)");
        element.val(warning);
    }
}

function toTextElement(element) {
    element.attr("type", "text");
}

function resetInput(element, isPass) {
    element.on("focus", function() {
        if (element.css("background-color") != "rgb(255, 255, 255)") {
            element.val("");
            if (isPass) {
                element.attr("type", "password");
            }
            element.css("background-color", "white");
        }
    });
}
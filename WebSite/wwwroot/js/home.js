var valid;
var unavailableFile = false;

$(document).ready(function() {
    if (window.location.href.search("auth=failed") > 20) {
        alert("لطفا وارد حساب کاربری خود شوید!");
    }

    if (window.location.href.search("pass=true") > 20) {
        alert("کلمه عبور با موفقیت تغییر یافت.");
    }

    if (window.location.href.search("message=sent") > 20) {
        alert("پیغام شما با موفقیت ارسال شد.");
    }

    if (window.location.href.search("password=reset") > 20) {
        alert("برای بازیابی کلمه عبور به ایمیل خود مراجعه کنید.");
    }

    var username = $("#ViewModel_SignUp_UserName");
    var email = $("#ViewModel_SignUp_Email");
    var confirmEmail = $("#ViewModel_SignUp_ConfirmEmail");
    var password = $("#ViewModel_SignUp_Password");
    var confirmPassword = $("#ViewModel_SignUp_ConfirmPassword");

    var signInName = $("#ViewModel_SignIn_NameOrEmail");
    var signInPassword = $("#ViewModel_SignIn_Password");

    $("#signup-button").on("click", function(event) {
        event.preventDefault();

        valid = true;
        warn(!username.val().match(/^[a-zA-Z0-9]{3,20}$/gm), "نام کاربری بین ۳ تا ۲۰ کاراکتر الزامی است", username, null);
        warn(!email.val().match(/^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/gm), "آدرس ایمیل اشتباه است", email, null);
        warn((confirmEmail.val().localeCompare(email.val()) != 0), "آدرس ایمیل یکسان نیست", confirmEmail, null);
        warn(!password.val().match(/^[a-zA-Z0-9]{8,30}$/gm), "کلمه عبور بین ۸ تا ۳۰ کاراکتر الزامی است", password, toTextElement);
        warn((confirmPassword.val().localeCompare(password.val()) != 0), "کلمه عبور یکسان نیست", confirmPassword, toTextElement);

        if (valid) {
            $.ajax({
                method: "POST",
                url: window.location.origin + "/signup",
                data: $("#signup-form").serialize()
            }).fail(function(res) {
                alert("خطا! لطفا دوباره تلاش کنید.");
            }).done(function(res) {
                window.location.href = window.location.origin + res;
            });
        }
    });

    $("#signin-button").on("click", function(event) {
        event.preventDefault();

        valid = true;
        warn(!(signInName.val().match(/^[a-zA-Z0-9]{3,20}$/gm) || signInName.val().match(/^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/gm)), "نام کاربری یا ایمیل اشتباه است", signInName, null);
        warn(!signInPassword.val().match(/^[a-zA-Z0-9]{8,30}$/gm), "کلمه عبور اشتباه وارد شده", signInPassword, toTextElement);

        if (valid) {
            $.ajax({
                method: "POST",
                url: window.location.origin + "/signin",
                data: $("#signin-form").serialize()
            }).fail(function(res) {
                alert("خطا! لطفا دوباره تلاش کنید.");
            }).done(function(res) {
                window.location.href = window.location.origin + res;
            });
        }
    })

    resetInput(username, false);
    resetInput(email, false);
    resetInput(confirmEmail, false);
    resetInput(password, true);
    resetInput(confirmPassword, true);
    resetInput(signInName, false);
    resetInput(signInPassword, true);

    $("#generate-audio").on("click", function(event) {
        event.preventDefault();

        unavailableFile = false;

        $("#loader").show();
        $("#loader-text").show();

        var matches = $("#episodes").val().match(/\b[1-9][0-9]{0,2}\b/gm);

        var episodes = $("#episodes").val().split(" ");

        var unavailableFiles = ["2", "6", "23", "28", "31", "32", "33", "34", "35", "36", "37", "40", "41", "42", "43", "44", "45", "46", "47", "92", "160", "167", "180", "185", "190", "250", "263", "278", "302", "304", "309", "310", "311", "312", "363", "446", "447", "452", "453", "454", "455", "464", "465", "474", "475", "476", "477", "478", "479", "480", "481", "499", "502", "559", "596", "620", "635", "907", "908", "909", "910", "911", "912", "913", "914", "915", "916", "917", "918", "919", "920", "921", "922", "923", "924", "925", "926", "927", "928", "929", "930", "931", "932", "933", "934", "935", "936", "937", "938", "939", "940", "941", "942", "943", "944", "945", "946", "947", "948", "949", "950", "951", "952", "953", "954", "955", "956", "957", "958", "959", "960", "961", "962", "963", "964", "965", "966", "967", "968", "969", "970", "971", "972", "973", "974", "975", "976", "977", "978", "979", "980", "981", "982", "983", "984", "985", "986", "987", "988", "989", "990", "991", "992", "993", "994", "995", "996", "997", "998", "999"];

        unavailableFiles.forEach(item => {
            if (episodes.includes(item)) {
                $("#loader").hide();
                $("#loader-text").hide();
                alert("متاسفانه برنامه شماره " + item + " موجود نیست.");
                unavailableFile = true;
                return { error: true };
            }
        });

        if (unavailableFile) {
            return { error: true };
        }

        var userFile = $('#user-audiofile').prop('files')[0];

        if (matches != null && matches.length == episodes.length) {
            var formData = new FormData();

            for (var i = 0; i < episodes.length; i++) {
                formData.append("audiofiles[" + i + "]", episodes[i]);
            }

            formData.append("usermp3file", userFile);

            formData.append($(this).parent().next().attr("name"), $(this).parent().next().val());

            var duration;
            if (episodes.length < 5) {
                duration = 5000;
            } else if (episodes.length < 10) {
                duration = 10000;
            } else if (episodes.length < 20) {
                duration = 25000;
            }

            $.ajax({
                url: window.location.origin + "/generateaudio",
                method: "POST",
                data: formData,
                processData: false,
                contentType: false
            }).done(function(res) {
                if (res == "success") {
                    setTimeout(function() {
                        $("#loader").hide();
                        $("#loader-text").hide();
                        downloadFile("../Mowlana.mp3");
                    }, duration);
                }
            }).fail(function() {
                $("#loader").hide();
                $("#loader-text").hide();
                alert("خطا! لطفا دوباره امتحان کنید.");
            });
        } else {
            $("#loader").hide();
            $("#loader-text").hide();
            alert("شماره برنامه‌ها اشتباه وارد شده‌ است. برای مثال:\n23 434 611 5");
        }
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

function downloadFile(filePath) {
    var link = document.createElement('a');
    link.href = filePath;
    link.download = filePath.substr(filePath.lastIndexOf('/') + 1);
    link.click();
}
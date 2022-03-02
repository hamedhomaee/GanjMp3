var onModal = false;
var deleteNote = 0;

$(document).ready(function() {
    $("#enter-text-button").on("click", function() {
        $("#enter-text").append(`
            <textarea name="content" class="s1-width-100 s1-resize-vertical content"></textarea>
            <div id="public" class="s1-width-100 s1-display-flex-row s1-justify-self-start public-maker s1-align-items-center">
                <label>این متن را به طور عمومی منتشر کن (این گزینه بعدا نیز قابل تغییر است)</label>
                <input id="is-public" type="checkbox" value="true" />
            </div>
            <div id="button-holder" class="m2-width-20 s1-width-50 s1-display-flex-row s1-justify-content-space-between">
                <button id="save" class="s1-width-40 text-button good-sign">
                    ذخیره
                </button>
                <button id="cancel" class="s1-width-40 text-button">
                    انصراف
                </button>
            </div>
        `);

        $("#content").focus();

        $(this).hide();
    });

    $("#enter-text").on("click", "#cancel", function(event) {
        $(this).parent().prev().remove();
        $("textarea[name='content']").remove();
        $("#button-holder").remove();
        $("#enter-text-button").show();
    });

    $("#enter-text").on("click", "#save", function(event) {
        $.ajax({
            url: "https://ganjmp3webapi.azurewebsites.net/addnote",
            method: "POST",
            contentType: "application/json",
            data: JSON.stringify({
                ownerid: $("input[name='user-id']").val(),
                content: $("textarea[name='content']").val(),
                ispublic: $("#is-public").is(":checked")
            })
        }).done(function() {
            window.location.href = window.location.origin + "/portal";
        }).fail(function() {
            alert("خطا! لطفا دوباره امتحان کنید.");
        });
    });

    $(document.body).on("click", ".delete", function() {
        deleteNote = $(this).parent().next().val();
        $("#modal-background").removeClass("s1-display-none");
    });

    $(document.body).on("click", "#delete-cancel", function() {
        $("#modal-background").addClass("s1-display-none");
    });

    $(document.body).on("click", "#modal-background", function() {
        if (!onModal) {
            $("#modal-background").addClass("s1-display-none");
        }
    });

    $(document.body).on("mouseover", "#modal", function() {
        onModal = true;
    });

    $(document.body).on("mouseleave", "#modal", function() {
        onModal = false;
    });

    $(document.body).on("click", ".edit", function() {
        var userText = $(this).parent().prev().html().replaceAll("<br>", "\n");

        $(this).parent().prev().replaceWith(`
            <textarea autofocus onfocus="this.selectionStart = this.selectionEnd = this.value.length;" class="s1-width-100 s1-resize-vertical content">` + userText + `</textarea>
            <div class="s1-width-100 s1-display-flex-row s1-justify-self-start public-maker s1-align-items-center">
                <label>این متن را به طور عمومی منتشر کن (این گزینه بعدا نیز قابل تغییر است)</label>
                <input type="checkbox" value="true" />
            </div>
        `);

        $(this).parent().replaceWith(`
            <div
                class="m2-width-20 s1-width-50 s1-align-self-center s1-display-flex-row s1-justify-content-space-between edit-buttons-holder">
                <button class="s1-width-40 text-button update">
                    ذخیره
                </button>
                <button class="s1-width-40 text-button reject">
                    انصراف
                </button>
            </div>
        `);
    });

    $(document.body).on("click", ".reject", function() {
        var userText = $(this).parent().prev().prev().html().replaceAll("\n", "<br>");

        $(this).parent().prev().remove();

        $(this).parent().prev().replaceWith(`
            <div class="s1-text-align-justify s1-width-100">` + userText + `</div>
        `);

        $(this).parent().html(`
            <button class="s1-width-40 text-button edit">
                ویرایش
            </button>
            <button class="s1-width-40 text-button delete">
                حذف
            </button>
        `);
    });

    $(document.body).on("click", ".update", function() {
        $.ajax({
            url: "https://ganjmp3webapi.azurewebsites.net/updatenote",
            method: "PUT",
            contentType: "application/json",
            data: JSON.stringify({
                id: $(this).parent().next().val(),
                content: $(this).parent().prev().prev().val(),
                ispublic: $(this).parent().prev().children("input").is(":checked")
            })
        }).done(function() {
            window.location.href = window.location.origin + "/portal";
        }).fail(function() {
            alert("خطا! لطفا دوباره امتحان کنید.");
        });
    });

    $(document.body).on("click", "#delete-confirm", function() {
        $.ajax({
            url: "https://ganjmp3webapi.azurewebsites.net/deletenote?id=" + deleteNote,
            method: "DELETE",
        }).done(function() {
            window.location.href = window.location.origin + "/portal";
        }).fail(function() {
            alert("خطا! لطفا دوباره امتحان کنید.");
        });
    });
});
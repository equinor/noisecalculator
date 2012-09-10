
function showValidationError(jqXHR, $formDiv) {
    var errorDiv = $("<div>").replaceWith(jqXHR.responseText).hide();

    //$("#editForm").append(errorDiv);
    $formDiv.append(errorDiv);

    $("#closeErrorDialog").click(function () {
        errorDiv.dialog("close");
    });

    errorDiv.dialog({
        modal: true,
        resizable: false,
        show: { effect: 'fade', duration: 500 },
        width: 600,
        title: $("#validationDialogDialogTitle").text(),
        position: [200, 80],
        cache: false
    });
}

function setCommonEvents() {
    $("#langNo").live("click", function () {
        $.cookie("_culture", "nb-NO", { expires: 365, path: '/' });
        window.location.reload();
    });

    $("#langEn").live("click", function () {
        $.cookie("_culture", "en-US", { expires: 365, path: '/' });
        window.location.reload();
    });
}

setCommonEvents();
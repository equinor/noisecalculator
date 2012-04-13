/* File Created: april 12, 2012 */

setAllEvents();

function setAllEvents() {
    $("#mainContent").delegate("#addNew", "click", function (event) {
        event.preventDefault();
        showDialogNew();
    });

    $("#searchForm").delegate("#searchButton", "click", function (event) {
        event.preventDefault();
        $("#searchForm").submit();
    });

    $("#dialogDiv").delegate("#submitButton", "click", function (event) {
        event.preventDefault();
        submitForm();
    });

    $("#dialogDiv").delegate("#closeButton", "click", function (event) {
        event.preventDefault();
        hideDialog();
    });
}

function submitForm() {
    $.ajax({
        type: $("#adminForm").attr("method"),
        url: $("#adminForm").attr("action"),
        data: $("#adminForm").serialize(),
        success: function (result) {
            var lol = "lol";
            //            var $taskDiv = $("<div>").append(result);
            //            if ($('#' + $taskDiv.find(".task").attr("id")).length > 0) {
            //                replaceTaskInTaskList(result);
            //            } else {
            //                addResultToTaskList($taskDiv);
            //            }
        },
        error: function (jqXHR) {
            alert("error!");
            //showValidationError(jqXHR);
        }
    });
}

function showDialogNew() {
    $("#dialogDiv").load($("#urlCreate").val(), function () {
        $("#dialogDiv").dialog({
            title: "Add New",
            modal: true,
            resizable: false,
            width: 'auto',
            position: [250, 80]
        });
    });
}

function hideDialog() {
    $("#dialogDiv").dialog('close');
}
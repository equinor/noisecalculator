$(document).ready(function () {
    $("#addTask").click(function () {
        $("#taskPopup")
            .empty()
            .load(addTaskUrl, function () {
                bindTaskDialogEvents();
            })
            .dialog({
                modal: true,
                resizable: false,
                hide: { effect: 'fade', duration: 1000 },
                width: 'auto',
                position: [250, 80]
            });
    });

    updateTotalPercentage();
});

function bindTaskDialogEvents() {
    $("#useTask").click(function (event) {
        event.preventDefault();

        var taskId = $('#taskSelect option:selected').val();
        $.ajax({
            type: "GET",
            url: getCreateTaskFormUrl + "/" + taskId,
            dataType: "html",
            cache: false,
            success: function (result) {
                $('#taskForm').empty();
                $('#taskForm').html(result);

                switch ($("#roleType").val()) {
                    case "Helideck":
                        bindHelideckEvents();
                        break;
                    default:
                        bindRegularEvents();
                }
            }
        });
    });
}


function bindRegularEvents() {

    /* Set disabled state of task time */
    if ($('#percentRadio').is(':checked')) {
        enablePercentageInput();
    } else {
        enableWorkTimeInput();
    }
    
    /* Set disabled state of noise level measured */
    if($("#noiseMeasuredNo").is(":checked")) {
        disableNoiseMeasuredInput();
    } else {
        enableNoiseMeasuredInput();
    }

    $("#noiseMeasuredYes").click(enableNoiseMeasuredInput);
    $("#noiseMeasuredNo").click(disableNoiseMeasuredInput);
    $("#percentRadio").click(enablePercentageInput);
    $("#workTimeRadio").click(enableWorkTimeInput);
    $("#taskFormCloseButton").click(closeTaskPopup);

    /* --------------- */
    if ($("#roleType").val() == "Rotation") {
        $("#hours").keyup(function (event) {
            $("#hoursAssistant").text($("#hours").val());
        });

        $("#minutes").keyup(function () {
            $("#minutesAssistant").text($("#minutes").val());
        });

        $("#percentage").keyup(function () {
            $("#percentageAssistant").text($("#percentage").val());
        });
    }

    $('#submitButton').click(function (event) {
        event.preventDefault();

        var myEditForm = $("#editForm");
        var formData = {
            TaskId: $("#taskId").val(),
            NoiseLevelMeassured: $("#noiseLevelMeassured").val(),
            Hours: $("#hours").val(),
            Minutes: $("#minutes").val(),
            Percentage: $("#percentage").val()
        };

        $.ajax({
            url: myEditForm.attr('action'),
            type: myEditForm.attr('method'),
            data: JSON.stringify(formData),
            contentType: "application/json",
            dataType: "html",
            success: function (result) {
                var $taskDiv = $("<div>").append(result);

                if ($('#' + $taskDiv.find(".task").attr("id")).length > 0) {
                    replaceTaskInTaskList(result);
                } else {
                    addResultToTaskList($taskDiv);
                }
            },
            error: function (jqXHR) {
                showValidationError(jqXHR);
            }
        });
    });
}

function bindHelideckEvents() {
    $("#taskFormCloseButton").click(closeTaskPopup);

    $('#submitButton').click(function (event) {
        event.preventDefault();

        var myEditForm = $('#editForm');
        var formData = {
            TaskId: $("#TaskId").val(),
            HelicopterId: $("#HelicopterId").val(),
            NoiseProtectionId: $("#NoiseProtectionId").val(),
            WorkIntervalId: $("#WorkIntervalId").val()
        };

        $.ajax({
            url: myEditForm.attr('action'),
            type: myEditForm.attr('method'),
            data: JSON.stringify(formData),
            contentType: "application/json",
            dataType: "html",
            success: function (result) {
                var $taskDiv = $("<div>").append(result);

                if ($('#' + $taskDiv.find(".task").attr("id")).length > 0) {
                    replaceTaskInTaskList(result);
                } else {
                    addResultToTaskList($taskDiv);
                }
            },
            error: function (jqXHR) {
                showValidationError(jqXHR);
            }
        });
    });
}

function addResultToTaskList($taskDiv) {
    $("#taskList").append($taskDiv);
    $taskDiv.fadeIn('slow');
    
    closeTaskPopup();
    updateTotalPercentage();
}

function replaceTaskInTaskList(result) {
    var idOfResultDiv = $(result).attr("id");
    $("#" + idOfResultDiv).replaceWith(result);
    
    closeTaskPopup();
    updateTotalPercentage();
}


function setAllEvents() {
    var $mainContainer = $("#taskList");
    
    // Click remove task
    $mainContainer.find(".emulatedRemoveButton").live("click", function () {
        var $item = $(this).closest(".task");
        removeTask($item);
    });
    
    // Click edit task
    $mainContainer.find(".emulatedEditButton").live("click", function () {
        var $item = $(this).closest(".task");
        editTask($item);
    });
}

setAllEvents();


function updateTotalPercentage() {
    $.ajax({
        type: "GET",
        url: getTotalPercentageUrl,
        dataType: "json",
        cache: false,
        success: function (result) {
            $("#totalDailyPercentage").text(result.Percentage + "%");
            $("#statusText").text(result.StatusText);
            $("#totalDailyPercentageDiv").removeClass().addClass(result.CssClass);
        },
        error: function (result) {
            alert(result);
        }
    });
}


function removeTask(taskDiv) {
    var selectedTaskId = taskDiv.attr("id");

    $.ajax({
        type: "POST",
        url: removeTaskUrl + "/" + selectedTaskId,
        dataType: "json",
        cache: false,
        success: function () {
            taskDiv.fadeOut("normal", function () {
                taskDiv.remove();
                updateTotalPercentage();
            });
        },
        error: function (jqXHR) {
            alert("Unable to remove: " + jqXHR.responseText);
        }
    });
}

function editTask(taskDiv) {
    var selectedTaskId = taskDiv.attr("id");
    
    /* Edit Dialog Task */
    $.ajax({
        type: "GET",
        url: getEditTaskFormUrl + "/" + selectedTaskId,
        dataType: "html",
        cache: false,
        success: function (result) {
            var $taskPopup = $('#taskPopup');
            $taskPopup.empty();
            $taskPopup.html(result);

            switch ($("#roleType").val()) {
                case "Helideck":
                    bindHelideckEvents();
                    break;
                case "Rotation":
                    alert("Rotation task!");
                    break;
                default:
                    bindRegularEvents();
            }

            $taskPopup.dialog({
                modal: true,
                resizable: false,
                hide: { effect: 'fade', duration: 1000 },
                width: 'auto',
                position: [250, 80]
            });
        }
    });
}


function closeTaskPopup() {
    $("#taskPopup").dialog('close');
}

function enableNoiseMeasuredInput() {
    $("#noiseLevelGuidline").attr("disabled", true);
    $("#noiseLevelMeassuredSpan").attr("disabled", false);
    $("#noiseLevelMeassured").attr("disabled", false).focus();
}

function disableNoiseMeasuredInput() {
    $("#noiseLevelMeassured").attr("disabled", true).val("");
    $("#noiseLevelGuidline").attr("disabled", false);
    $("#noiseLevelMeassuredSpan").attr("disabled", true);
}

function enablePercentageInput() {
    $("#percentageSpan").attr("disabled", false);
    $("#percentage").attr("disabled", false).focus();

    $("#hours").attr("disabled", true).val("");
    $("#minutes").attr("disabled", true).val("");
    $("#hoursAssistant").text("");
    $("#minutesAssistant").text("");
    $("#workTimeSpan").attr("disabled", true);
}

function enableWorkTimeInput() {
    $("#workTimeSpan").attr("disabled", false);
    $("#hours").attr("disabled", false).focus();
    $("#minutes").attr("disabled", false);

    $("#percentage").attr("disabled", true).val("");
    $("#percentageAssistant").text("");
    $("#percentageSpan").attr("disabled", true);
}

function showValidationError(jqXHR) {
    var errorDiv = $("<div>").replaceWith(jqXHR.responseText).hide();

    $("#editForm").append(errorDiv);

    $("#closeErrorDialog").click(function () {
        errorDiv.dialog("close");
    });

    errorDiv.dialog({
        modal: true,
        resizable: false,
        show: { effect: 'fade', duration: 500 },
        width: 600,
        title: $("#validationDialogTitle").text(),
        position: [200, 80],
        cache: false
    });
}
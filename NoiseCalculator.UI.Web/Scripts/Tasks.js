// Initialize event handlers
setAllEvents();

$(document).ready(function () {
    $("#addTask").click(function () {
        openTaskDialog();
    });
        
    $("#removeAllTasks").click(function () {
        $("#removeAllConfirmDialog").dialog({
                    title: $("#removeAllConfirmDialog").attr("title"),
                    modal: true,
                    resizable: false,
                    width: 'auto',
                    position: [250, 80]
                });
                
                $("#confirmRemoveAll").click(function () {
                    removeAllTasks();
                });

                $("#cancelRemoveAll").click(function () {
                    $("#removeAllConfirmDialog").dialog('close');
                });
        
            });

    /* Date picker in report info */
    $("#date").datepicker({
        showOn: "both",
        dateFormat: 'dd.mm.yy',
        buttonImage: "Content/calendar.png",
        buttonImageOnly: true
    });


    $("#printAsPdfPost").click(function () {
        // Convert date to "US" format to allow DateTime model binding.
        var dateStringSplitted = $("#date").val().split('.');
        var formatedDateString = dateStringSplitted[1] + "/" + dateStringSplitted[0] + "/" + dateStringSplitted[2];

        var theUrl = $("#pdfReportUrl").val() + "?plant=" + $("#plant").val() + "&group=" + $("#group").val() + "&date=" + formatedDateString + "&comment=" + $("#comment").val();
        window.location = theUrl;
        $("#reportInfo").dialog('close');
    });

    $("#printAsPdf").click(function () {
        $("#reportInfo").dialog({
            modal: true,
            title: $("#reportInfo").attr("title"),
            resizable: false,
            width: 'auto',
            position: [250, 80]
        });
    });

    updateTotalPercentage();
});

function setAllEvents() {
    var $mainContainer = $("#taskList");

    // Click remove task
    $mainContainer.find(".taskListRemove").live("click", function() {
        var $item = $(this).closest(".task");

        $("#deleteConfirmDialog")
            .empty()
            .load(getRemoveTaskConfirmationUrl + "/" + $item.attr("id"), function() {

                $("#confirmRemove").click(function() {
                    removeTask($item);
                    closeRemoveConfirmDialog();
                });

                $("#cancelRemove").click(function() {
                    closeRemoveConfirmDialog();
                });

                $(this).dialog({
                    title: $("#dialogTitleConfirmRemove").val(),
                    modal: true,
                    resizable: false,
                    width: 'auto',
                    position: [500, 140]
                });
            });
    });

    // Click edit task
    $mainContainer.find(".taskListEdit").live("click", function() {
        var $item = $(this).closest(".task");
        editTask($item);
    });

    $("#langNo").live("click", function () {
        $.cookie("_culture", "nb-NO", { expires: 365, path: '/' });
        window.location.reload(); // reload
    });
    
    $("#langEn").live("click", function () {
        $.cookie("_culture", "en-US", { expires: 365, path: '/' });
        window.location.reload(); // reload
    });
}

function openTaskDialog() {
    $("#taskDialog")
        .empty()
        .load(addTaskUrl, function () {
            bindTaskDialogEvents();
            $(this).dialog({
                modal: true,
                title: $("#hiddenTaskDialogTitle").val(),
                resizable: false,
                hide: { effect: 'fade', duration: 300 },
                width: 'auto',
                position: [250, 80]
            });
        });
}


function bindTaskDialogEvents() {
    $("#taskSelect").dblclick(function (event) {
        event.preventDefault();
        getCreateTaskForm();
    });
    
    $("#useTask").click(function (event) {
        event.preventDefault();
        getCreateTaskForm();
    });
}


function getCreateTaskForm() {
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
                case "Rotation":
                    bindRotationEvents();
                    break;
                default:
                    bindRegularEvents();
            }
        }
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
    $("#noiseLevelMeassured").click(enableNoiseMeasuredInput);
    $("#noiseMeasuredNo").click(disableNoiseMeasuredInput);

    $("#percentRadio").click(enablePercentageInput);
    $("#percentage").click(enablePercentageInput);

    $("#workTimeRadio").click(enableWorkTimeInput);
    $("#hours").click(enableWorkTimeInput);
    $("#minutes").click(enableWorkTimeInput);
    
    $("#taskFormCloseButton").click(closeTaskDialog);
    $('#submitButton').click(function (event) {
        event.preventDefault();
        submitRegularForm();
    });

    /* For Rotation tasks, work time / percentage should be mirrored as assistant time */
    if($("#roleType").val() == "AreaNoise") {
        $("#noiseLevelMeassuredSpan").hide();
        $("#noiseMeasuredYesLabel").hide();
        $("#noiseMeasuredYes").hide();
    }
}

function bindHelideckEvents() {
    $("#taskFormCloseButton").click(closeTaskDialog);

    $('#submitButton').click(function (event) {
        event.preventDefault();
        submitHelideckForm();
    });
}


/* MODIFIED FOR ROTATION */
function enableNoiseMeasuredInputRotationOperator() {
    if ($("#noiseMeasuredYesOperator").attr("checked") == undefined) {
        $("#noiseMeasuredYesOperator").attr("checked", "checked");
        $("#noiseLevelMeassuredOperator").focus();
    }

    $("#noiseLevelMeassuredOperator").focus();
    $("#noiseLevelGuidlineOperator").attr("disabled", true);
}

function disableNoiseMeasuredInputRotationOperator() {
    $("#noiseLevelGuidlineOperator").removeAttr("disabled");
    $("#noiseLevelMeassuredOperator").val("");
}

function enableNoiseMeasuredInputRotationAssistant() {
    if ($("#noiseMeasuredYesAssistant").attr("checked") == undefined) {
        $("#noiseMeasuredYesAssistant").attr("checked", "checked");
        $("#noiseLevelMeassuredAssistant").focus();
    }

    $("#noiseLevelMeassuredAssistant").focus();
    $("#noiseLevelGuidlineAssistant").attr("disabled", true);
}

function disableNoiseMeasuredInputRotationAssistant() {
    $("#noiseLevelGuidlineAssistant").removeAttr("disabled");
    $("#noiseLevelMeassuredAssistant").val("");
}
/* ------- MODIFIED FOR ROTATION ------- */


function bindRotationEvents() {
    //
    /* Set disabled state of task time */
    if ($('#percentRadio').is(':checked')) {
        enablePercentageInput();
    } else {
        enableWorkTimeInput();
    }

    // ----------------------------------------------
    // Refactor - Unscrew this, DRY - Create common code with the "regular task" use case,
    disableNoiseMeasuredInputRotationOperator();
    disableNoiseMeasuredInputRotationAssistant();
    
    $("#noiseMeasuredYesOperator").click(enableNoiseMeasuredInputRotationOperator);
    $("#noiseLevelMeassuredOperator").click(enableNoiseMeasuredInputRotationOperator);
    $("#noiseMeasuredNoOperator").click(disableNoiseMeasuredInputRotationOperator);

    $("#noiseMeasuredYesAssistant").click(enableNoiseMeasuredInputRotationAssistant);
    $("#noiseLevelMeassuredAssistant").click(enableNoiseMeasuredInputRotationAssistant);
    $("#noiseMeasuredNoAssistant").click(disableNoiseMeasuredInputRotationAssistant);

// ----------------------------------------------

    $("#percentRadio").click(enablePercentageInput);
    $("#percentage").click(enablePercentageInput);

    $("#workTimeRadio").click(enableWorkTimeInput);
    $("#hours").click(enableWorkTimeInput);
    $("#minutes").click(enableWorkTimeInput);

    $("#taskFormCloseButton").click(closeTaskDialog);
    $('#submitButton').click(function (event) {
        event.preventDefault();
        submitRotationForm();
    });
}

function addResultToTaskList($taskDiv) {
    $("#taskList").append($taskDiv);
    $taskDiv.fadeIn('slow');
    
    closeTaskDialog();
    updateTotalPercentage();
}

function replaceTaskInTaskList(result) {
    var idOfResultDiv = $(result).attr("id");
    $("#" + idOfResultDiv).replaceWith(result);
    
    closeTaskDialog();
    updateTotalPercentage();
}

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

            if (result.Percentage > 0) {
                $("#removeAllContainer").show();
            } else {
                $("#removeAllContainer").hide();
            }

            // Dynamic footnotes
            $("#dynamicFootnotes").empty();
            $.each(result.DynamicFootnotes, function () {
                $("#dynamicFootnotes").append("<li>" + this + "</li>");
            });
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

function removeAllTasks() {
    $.ajax({
        type: "POST",
        url: removeAllUrl,
        dataType: "json",
        cache: false,
        success: function () {
            window.location = window.location;
        },
        error: function (jqXHR) {
            alert("Unable to remove all: " + jqXHR.responseText);
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
            var $taskDialog = $('#taskDialog');
            $taskDialog.empty();
            $taskDialog.html(result);

            switch ($("#roleType").val()) {
                case "Helideck":
                    bindHelideckEvents();
                    break;
                default:
                    bindRegularEvents();
            }

            $taskDialog.dialog({
                modal: true,
                title: $("#hiddenEditTitle").val(),
                resizable: false,
                hide: { effect: 'fade', duration: 300 },
                width: 'auto',
                position: [250, 80]
            });
        }
    });
}


function closeTaskDialog() {
    $("#taskDialog").dialog('close');
}

function closeRemoveConfirmDialog() {
    $("#deleteConfirmDialog").dialog('close');
}

function enableNoiseMeasuredInput() {
    if ($("#noiseMeasuredYes").attr("checked") == undefined) {
        $("#noiseMeasuredYes").attr("checked", "checked");
        $("#noiseLevelMeassured").focus();
    } 

    $("#noiseLevelMeassured").focus();
    $("#noiseLevelGuidline").attr("disabled", true);
}

function disableNoiseMeasuredInput() {
    $("#noiseLevelGuidline").removeAttr("disabled");
    $("#noiseLevelMeassured").val("");
}

function enablePercentageInput() {
    if ($("#percentRadio").attr("checked") == undefined) {
        $("#percentRadio").attr("checked", "checked");
    }
    
    $("#percentage").focus();

    $("#hours").val("");
    $("#minutes").val("");
    $("#timeOperatorSpan").hide();
    $("#timeAssistantSpan").hide();
}

function enableWorkTimeInput() {
    if ($("#workTimeRadio").attr("checked") == undefined) {
        $("#workTimeRadio").attr("checked", "checked");
    } else if($("#minutes").is(":focus") == false) {
        $("#hours").focus();
    }

    $("#percentage").val("");
}

function submitRegularForm() {
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
}

function submitRotationForm() {
    var myEditForm = $("#editForm");
    var formData = {
        RotationId: $("#rotationId").val(),
        Hours: $("#hours").val(),
        Minutes: $("#minutes").val(),
        Percentage: $("#percentage").val(),
        OperatorNoiseLevelMeasured: $("#noiseLevelMeassuredOperator").val(),
        AssistantNoiseLevelMeasured: $("#noiseLevelMeassuredAssistant").val()
    };

    $.ajax({
        url: myEditForm.attr('action'),
        type: myEditForm.attr('method'),
        data: JSON.stringify(formData),
        contentType: "application/json",
        dataType: "html",
        success: function (result) {
            var $taskDiv = $("<div>").append(result);
            addResultToTaskList($taskDiv);
        },
        error: function (jqXHR) {
            showValidationError(jqXHR);
        }
    });
}


function submitHelideckForm() {
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
        title: $("#validationDialogDialogTitle").text(),
        position: [200, 80],
        cache: false
    });
}
// Initialize event handlers
setAllEvents();

function openHelpDialog() {
    $("#helpDialog").dialog({
        modal: true,
        title: $("#helpButton").text(),
        resizable: false,
        hide: { effect: 'fade', duration: 300 },
        show: { effect: 'fade', duration: 300 },
        width: 'auto',
        position: [300, 100]
    });
}

$(document).ready(function () {
    $("#addTask").click(function () {
        openTaskDialog();
    });

    $("#helpButton").click(function() {
        openHelpDialog();
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
        $("#comment").maxlength({ max: 75, showFeedback: true, feedbackTarget: '#maxlengthFeedback', feedbackText: '{c} ({m} max)' });
        
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
                position: [200, 10]
            });
        });
}


function bindTaskDialogEvents() {
    $("#taskDefSelect").change(function (event) {
        event.preventDefault();
        getTaskList();
    });

    $("#taskSelect").dblclick(function (event) {
        event.preventDefault();
        getCreateTaskForm();
    });
    
    $("#useTask").click(function (event) {
        event.preventDefault();
        getCreateTaskForm();
    });
}

function getTaskList() {
    var taskDefSelected = $("#taskDefSelect option:selected").val().split('-');
    
    $("#taskSelect").empty();
    $.ajax({
        type: "POST",
        url: getTasksUrl,
        dataType: "json",
        cache: false,
        data: { id: $("#taskDefSelect").val() },
        success: function(tasks) {
            // states contains the JSON formatted list
            // of states passed from the controller
            $.each(tasks.Tasks, function (i, task) {
                $("#taskSelect").append('<option value="'
                    + task.Value + '">'
                    + task.Text + '</option>');
            });
        },
        error: function(ex) {
            alert('Failed to retrieve tasks.' + ex);
        }
    });
}

function getCreateTaskForm() {
    var taskSelectValueSplitted = $("#taskSelect option:selected").val().split('-');
    var taskId = taskSelectValueSplitted[0];
    var roleType = taskSelectValueSplitted[1];

    $.ajax({
        type: "GET",
        url: getCreateTaskFormUrl(roleType) + "/" + taskId,
        dataType: "html",
        cache: false,
        success: function (result) {
            $('#taskForm').empty();
            $('#taskForm').html(result);
            
            switch (roleType) {
                case "Helideck":
                    bindHelideckEvents();
                    break;
                case "Helipassenger":
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

function getCreateTaskFormUrl(roleType) {
    switch (roleType) {
        case "Helideck":
            return createHelideckTaskFormUrl;
        case "Helipassenger":
            return createHelideckTaskFormUrl;
        case "Rotation":
            return createRotationTaskFormUrl;
        default:
            return createRegularTaskFormUrl;
    }
}

function bindRegularEvents() {
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
    
    // Meassured noise level and more is not applicable for non-noisy work in areas with noise
    if ($("#roleType").val() == "AreaNoise") {
        $("#backgroundNoiseLabel").hide();
        $("#backgroundNoise").hide();
        $("#backgroundNoiseReasonLabel").hide();
        $("#backgroundNoiseDba").hide();
        $("#regularNoiseAndButtonPressed").hide();
        $("#regularNoiseAndButtonPressedValues").hide();
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

            var numberOfResults = $("#taskList").find(".task").length;
            
            if (numberOfResults > 0) {
                $("#removeAllContainer").show();
            } else {
                $("#removeAllContainer").hide();
            }

            // Dynamic footnotes
            $("#footerTexts").empty();
            $.each(result.Footnotes, function () {
                $("#footerTexts").append("<li>" + this + "</li>");
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
    var roleType = $(taskDiv).find(".roleType").text();
    
    /* Edit Dialog Task */
    $.ajax({
        type: "GET",
        url: getEditTaskFormUrl(roleType) + "/" + selectedTaskId,
        dataType: "html",
        cache: false,
        success: function (result) {
            var $taskDialog = $('#taskDialog');
            $taskDialog.empty();
            $taskDialog.html(result);

            switch (roleType) {
                case "Helideck":
                    bindHelideckEvents();
                    break;
                case "Helipassenger":
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

function getEditTaskFormUrl(roleType) {
    switch (roleType) {
        case "Helideck":
            return editHelideckTaskFormUrl;
        case "Helipassenger":
            return editHelideckTaskFormUrl;
        default:
            return editRegularTaskFormUrl;
    }
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
        ButtonPressed: $("#buttonPressed").val(),
        BackgroundNoise: $("#backgroundNoise").val(),
        NoiseProtectionId: $('#noiseProtectionId').val(),
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
            showValidationError(jqXHR, myEditForm);
        }
    });
}

function submitRotationForm() {
    var myEditForm = $("#editForm");
    var formData = {
        RotationId: $("#rotationId").val(),
        Hours: $("#hours").val(),
        Minutes: $("#minutes").val(),
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
            showValidationError(jqXHR, myEditForm);
        }
    });
}


function submitHelideckForm() {
    var myEditForm = $('#editForm');
    var formData = {
        TaskId: $("#TaskId").val(),
        HelicopterId: $("#HelicopterId").val(),
        NoiseProtectionId: $("#NoiseProtectionId").val()
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
            showValidationError(jqXHR, myEditForm);
        }
    });
}

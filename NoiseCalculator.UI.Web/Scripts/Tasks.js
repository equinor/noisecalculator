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

    $(".emulatedRemoveButton").click(function (event) {
        event.preventDefault();
        var taskDiv = $(this).closest(".task");
        removeTask(taskDiv);
    });

    updateTotalPercentage();
});

function bindTaskDialogEvents() {
    $("#useTask").click(function (event) {
        event.preventDefault();

        var taskId = $('#taskSelect option:selected').val();
        $.ajax({
            type: "GET",
            url: getTaskFormUrl + "/" + taskId,
            dataType: "html",
            cache: false,
            success: function (result) {
                $('#taskForm').empty();
                $('#taskForm').html(result);
                /*bindHelideckEvents();*/
                bindRegularEvents();
            }
        });
    });
}


function closeTaskPopup() {
    $("#taskPopup").dialog('close');
}


/*noiseMeasuredNo
noiseMeasuredYes
workTimeRadio
percentRadio*/

/* IsNoiseMeassured: "",
IsWorkSpecifiedAsTime: "",
IsWorkSpecifiedAsPercentage: "",*/

function bindRegularEvents() {
    $("#taskFormCloseButton").click(closeTaskPopup);

    $('#addButton').click(function (event) {
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
                addResultToTaskList(result);
            }
        });
    });
}


function bindHelideckEvents() {
    $("#taskFormCloseButton").click(closeTaskPopup);

    $('#addButton').click(function (event) {
        event.preventDefault();

        var myEditForm = $('#editForm');
        var formData = {
            TaskId: $("#TaskId").val(),
            HelicopterId: $("#HelicopterId").val(),
            NoiseProtectionId: $("#NoiseProtectionId").val(),
            WorkIntervalId: $("#WorkIntervalId").val()
        };

        if (formData.HelicopterId == 0 || formData.NoiseProtectionId == 0 || formData.WorkIntervalId == 0) {
            /* This message should be generated as a div when the form itself is generated. */
            alert("Helicopter, noise protection and work interval must be selected to add the task.");
            return;
        }

        $.ajax({
            url: myEditForm.attr('action'),
            type: myEditForm.attr('method'),
            data: JSON.stringify(formData),
            contentType: "application/json",
            dataType: "html",
            success: function (result) {
                addResultToTaskList(result);
            }
            /*error: function (jqXHR) {
            alert(jqXHR.statusText);
            alert(jqXHR.responseText);
            },*/
        });
    });
}

function addResultToTaskList(newTaskDiv) {
    
    /* Add task from result, with effect */
    var taskDiv = $("<div>").replaceWith(newTaskDiv).hide();
    $("#taskList").append(taskDiv);
    taskDiv.fadeIn('slow');
    closeTaskPopup();

    /* Add remove button event */
    taskDiv.children(".emulatedRemoveButton").click(function () {
        removeTask(taskDiv);
    });

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
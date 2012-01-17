$(document).ready(function () {
    $("#addTask").click(function () {
        $("#taskPopup")
            .empty()
            .load(addTaskUrl, function () {
                addTaskFormEvents();
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
    });

    updateTotalPercentage();
});

function addTaskFormEvents() {
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
                addEventsToHelideckForm();
            }
        });
    });
}

function closeTaskPopup() {
    $("#taskPopup").dialog('close');
}

function addEventsToHelideckForm() {
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

        $.ajax({
            url: myEditForm.attr('action'),
            type: myEditForm.attr('method'),
            data: JSON.stringify(formData),
            contentType: "application/json",
            dataType: "html",
            /*error: function (jqXHR) {
            alert(jqXHR.statusText);
            alert(jqXHR.responseText);
            },*/
            success: function (result) {
                var div = $("<div>").replaceWith(result).hide();
                $("#taskList").append(div);
                div.fadeIn('slow');
                closeTaskPopup();

                /* Add Remove event handler */
                var selectedTaskId = div.attr("id");
                var removeDiv = div.children(".emulatedRemoveButton");
                
                removeDiv.click(function () {
                    alert("Remove clicked for Div with ID: " + selectedTaskId);
                });

                updateTotalPercentage();
            }
        });
    });
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
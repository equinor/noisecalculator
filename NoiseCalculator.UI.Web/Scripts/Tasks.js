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
                width: 'auto',
                position: [250, 80]
            });

    });
});

function addTaskFormEvents() {
    $("#useTask").click(function (event) {
        event.preventDefault();

        var selectedTask = $('#taskSelect option:selected').val();
        $.ajax({
            type: "POST",
            url: getTaskFormUrl,
            data: "{'selectedTask': '" + selectedTask + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "html",
            success: function (result) {
                $('#taskForm').empty();
                $('#taskForm').html(result);
                addEventsToHelideckForm();
            }
        });
    });
}


function addEventsToHelideckForm() {
    $("#taskFormCloseButton").click(function () {
        var popupDiv = $("#taskPopup");
        popupDiv.dialog('close');
    });

    $('#addButton').click(function (event) {
        event.preventDefault();

        var myEditForm = $('#editForm');
        var formData = {
            TaskId: $("#TaskId").val(),
            HelicopterIdSelected: $("#HelicopterIdSelected").val(),
            NoiseProtectionIdSelected: $("#NoiseProtectionIdSelected").val(),
            WorkIntervalIdSelected: $("#WorkIntervalIdSelected").val()
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
            }
        });
    });
}
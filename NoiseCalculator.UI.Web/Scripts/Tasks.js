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
                /*width: 450,*/
                /*height: 540,*/
                position: [250, 80]
            });

    });
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
            }
        });
    });
}
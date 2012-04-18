/* File Created: april 12, 2012 */

setAllEvents();

function setAllEvents() {
    $("#mainContent").delegate("#addNew", "click", function(event) {
        event.preventDefault();
        showDialogNew();
    });

//    $("#searchForm").delegate("#searchButton", "click", function(event) {
//        event.preventDefault();
//        $("#searchForm").submit();
//    });

    $("#dialogDiv").delegate("#submitButton", "click", function(event) {
        event.preventDefault();
        submitForm();
    });

    $("#dialogDiv").delegate("#closeButton", "click", function(event) {
        event.preventDefault();
        hideDialog();
    });

    $("#mainContent").delegate(".editDefinition", "click", function (event) {
        event.preventDefault();
        showDialogEdit(this);
    });

    $("#mainContent").delegate(".removeDefinition", "click", function (event) {
        event.preventDefault();
        getConfirmDeleteDialog(this);
    });

    $("#deleteConfirmDialog").delegate("#confirmDeleteButton", "click", function(event) {
        event.preventDefault();
        deleteDefinition();
    });

    $("#deleteConfirmDialog").delegate("#cancelDeleteButton", "click", function(event) {
        event.preventDefault();
        hideDeleteConfirmation();
    });

    $("#dialogDiv").delegate("#adminForm", "submit", function (event) {
        event.preventDefault();
        submitForm();
    });
} // setAllEvents()


function getConfirmDeleteDialog(removeDefinitionButton) {
    $("#deleteConfirmDialog")
            .empty()
            .load($("#urlDeleteConformation").val() + "/" + $(removeDefinitionButton).closest(".definition").attr("id"), function () {
                $(this).dialog({
                    title: $("#dialogTitleConfirmDelete").val(),
                    modal: true,
                    resizable: false,
                    width: 'auto',
                    position: [250, 80]
                });
            });
}

function deleteDefinition() {
    // Get item by creating a ID selector based on the idToDelete value
    var $item = $('#' + $("#idToDelete").val());

    $.ajax({
        type: "POST",
        url: $("#urlDeleteDefinition").val() + "/" + $item.attr("id"),
        dataType: "json",
        cache: false,
        success: function () {
            $item.remove();
            hideDeleteConfirmation();
        },
        error: function (jqXHR) {
            alert("Unable to remove: " + jqXHR.responseText);
        }
    });
}

function submitForm() {
    $.ajax({
        type: $("#adminForm").attr("method"),
        url: $("#adminForm").attr("action"),
        data: $("#adminForm").serialize(),
        success: function (result) {
            if ($('#' + $(result).attr('id')).length > 0) {
                replaceExistingItemWithSameId(result);
            } else {
                addResultToList(result);
            }
        },
        error: function (jqXHR) {
            alert("error!");
            //showValidationError(jqXHR);
        }
    });
}

function addResultToList(result) {
    $("#definitionList tbody").prepend(result);
    hideDialog();
}

function replaceExistingItemWithSameId(result) {
    var idOfResultDiv = $(result).attr("id");
    $("#" + idOfResultDiv).replaceWith(result);
    hideDialog();
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

function showDialogEdit(editDefinitionButton) {
    $.ajax({
        type: "GET",
        url: $("#urlEdit").val() + "/" + $(editDefinitionButton).closest(".definition").attr("id"),
        dataType: "html",
        cache: false,
        success: function (result) {
            var $dialogDiv = $('#dialogDiv');
            $dialogDiv.empty();
            $dialogDiv.html(result);

            $dialogDiv.dialog({
                modal: true,
                title: 'Edit Definition',
                resizable: false,
                hide: { effect: 'fade', duration: 300 },
                width: 'auto',
                position: [250, 80]
            });
        }
    });
}

function hideDialog() {
    $("#dialogDiv").dialog('close');
}

function hideDeleteConfirmation() {
    $("#deleteConfirmDialog").dialog('close');
}
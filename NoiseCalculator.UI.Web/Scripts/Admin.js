/* File Created: april 12, 2012 */

setAllEvents();

function setAllEvents() {
    $("#mainContent").delegate("#addNew", "click", function(event) {
        event.preventDefault();
        showDialogNew();
    });

    $("#dialogDiv").delegate("#submitButton", "click", function(event) {
        event.preventDefault();
        submitForm();
    });

    $("#dialogDiv").submit(function(event) {
        event.preventDefault();
        submitForm();
    });  

    $("#dialogDiv").delegate("#closeButton", "click", function(event) {
        event.preventDefault();
        hideDialog();
    });

    $("#mainContent").delegate(".definition .editButton", "click", function (event) {
        event.preventDefault();
        showDialogEdit(this);
    });
    $("#mainContent").delegate("tr.definition.editable", "dblclick", function (event) {
        event.preventDefault();
        showDialogEdit(this);
    });

    $("#mainContent").delegate(".definition .removeButton", "click", function (event) {
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


    // Translation events
    $("#dialogDiv").delegate("#addNewTranslation", "click", function (event) {
        event.preventDefault();
        showTranslationDialogNew();
    });

    $(".translation .editButton").live('click', function (event) {
        event.preventDefault();
        showTranslationDialogEdit(this);
    });

    $(".translation .removeButton").live('click', function (event) {
        event.preventDefault();
        getConfirmDeleteTranslationDialog(this);
    });
    
    $("#submitTranslationButton").live("click", function (event) {
        event.preventDefault();
        submitTranslationForm();
    });

    $("#closeTranslationButton").live("click", function (event) {
        event.preventDefault();
        hideTranslationDialog();
    });
    // End Translation events

    $("#helpButton").click(function () {
        openHelpDialog();
    });
    
} // setAllEvents()

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

    var id;
    var matches = $item.attr("id").match( /[\d]+$/ );
    if(matches !== null && matches.length > 0) {
        id = matches[0];
    } else {
        id = $item.attr("id");
    }
     // <--- First matched items should be the id at end of string
//    var id = $item.attr("id").match( /[\d]+$/ )[0]; // <--- First matched items should be the id at end of string

    $.ajax({
        type: "POST",
        url: $("#urlDeleteAction").val() + "/" + id,
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
                hideDialog();
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
    $("#itemList tbody").prepend(result);
    hideDialog();
}

function replaceExistingItemWithSameId(result) {
    var idOfResultDiv = $(result).attr("id");
    $("#" + idOfResultDiv).replaceWith(result);
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

// TEST TEST
function showDialogNewRotation() {
    $("#dialogDiv").load($("#urlCreateRotation").val(), function () {
        $("#dialogDiv").dialog({
            title: "Add New",
            modal: true,
            resizable: false,
            width: 'auto',
            position: [250, 80]
        });
    });
}
// TEST TEST

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

function hideTranslationDialog() {
    $("#translationDialogDiv").dialog('close');
}

function showTranslationDialogNew() {
    $("#translationDialogDiv").load($("#urlCreateTranslation").val(), function () {
        $("#translationDialogDiv").dialog({
            title: "Add Translation",
            modal: true,
            resizable: false,
            width: 'auto',
            position: [300, 100]
        });
    });
}

function showTranslationDialogEdit(editTranslationButton) {
    var lengthOfIdPrefix = "trans".length;
    var id = $(editTranslationButton).closest(".translation").attr("id");
    var parsedId = id.substr(lengthOfIdPrefix, id.length - lengthOfIdPrefix);
    
    // Oppdater url element til å benytter translation Edit URL
    $("#translationDialogDiv").load($("#urlEditTranslation").val() + '/' + parsedId, function () {
        $("#translationDialogDiv").dialog({
            title: "Edit Translation",
            modal: true,
            resizable: false,
            hide: { effect: 'fade', duration: 300 },
            width: 'auto',
            position: [250, 80]
        });
    });
}

function submitTranslationForm() {
    $.ajax({
        type: $("#adminTranslationForm").attr('method'),
        url: $("#adminTranslationForm").attr('action'),
        data: $("#adminTranslationForm").serialize(),
        success: function (result) {
            if ($('#' + $(result).attr('id')).length > 0) {
                replaceExistingItemWithSameId(result);
                hideTranslationDialog();
            } else {
                addTranslationResultToList(result);
                hideTranslationDialog();
            }
        },
        error: function (result) {
            var feil = "Feilmelding!";
        }
    });
}

function addTranslationResultToList(result) {
    $("#translations tbody").prepend(result);
    hideTranslationDialog();
}

function getConfirmDeleteTranslationDialog(removeTranslationButton) {
    var lengthOfIdPrefix = "trans".length;
    var id = $(removeTranslationButton).closest(".translation").attr("id");
    var parsedId = id.substr(lengthOfIdPrefix, id.length - lengthOfIdPrefix);
    
    $("#deleteConfirmDialog")
            .empty()
            .load($("#urlDeleteTranslationConformation").val() + "/" + parsedId, function () {
                $(this).dialog({
                    title: $("#dialogTitleConfirmDelete").val(),
                    modal: true,
                    resizable: false,
                    width: 'auto',
                    position: [250, 80]
                });
            });
}

function deleteTranslation() {
    // Get item by creating a ID selector based on the idToDelete value
    var $item = $('#' + $("#idToDelete").val());

    $.ajax({
        type: "POST",
        url: $("#urlDeleteAction").val() + "/" + $item.attr("id"),
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
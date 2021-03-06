﻿/* File Created: april 12, 2012 */

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
    
    $(".translation").live('dblclick', function (event) {
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
            showValidationError(jqXHR, $("#adminForm"));
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
            title: $("#adminForm").attr("title"),
            modal: true,
            resizable: false,
            width: 'auto',
            position: [100, 80]
        });
    });
}

function getRoleTypeFromRow(eventNode) {
    var roleType = $(eventNode).closest(".definition").find(".role").html();
    if (roleType === 'Rotation') {
        return $("#urlEditRotation").val();
    }
    else {
        return $("#urlEditGeneric").val();
    }
}

function showDialogEdit(eventNode) {
    
    var urlEdit;

    if($("#urlEditGeneric").length > 0) { //  only task view has this element
        urlEdit = getRoleTypeFromRow(eventNode);
    } else {
        urlEdit = $("#urlEdit").val();
    }
    
    $.ajax({
        type: "GET",
        url: urlEdit + "/" + $(eventNode).closest(".definition").attr("id"),
        dataType: "html",
        cache: false,
        success: function (result) {
            var $dialogDiv = $('#dialogDiv');
            $dialogDiv.empty();
            $dialogDiv.html(result);

            $dialogDiv.dialog({
                modal: true,
                title: $("#adminForm").attr("title"),
                resizable: false,
                hide: { effect: 'fade', duration: 300 },
                width: 'auto',
                position: [100, 80]
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
            title: $("#adminForm").attr("title"),
            modal: true,
            resizable: false,
            width: 'auto',
            position: [150, 120]
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
            title: $("#adminForm").attr("title"),
            modal: true,
            resizable: false,
            hide: { effect: 'fade', duration: 300 },
            width: 'auto',
            position: [150, 120]
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
        error: function (jqXHR) {
            showValidationError(jqXHR, $("#adminTranslationForm"));
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

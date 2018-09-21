"use strict";

$(document).ready(function () {
    // Get 'apiServerUri' from config file
    var apiServerUri = configSettings.apiServerUri;

    var KeyTypes = { "OkCancel": 0 };
    Object.freeze(KeyTypes);

    var key = {
        data: "",
        sessionId: "",
        key: ""
    };

    var receipt = "";

    // Setup and start SignalR connection
    var connection = new signalR.HubConnectionBuilder().withUrl("/notify").build();
    connection.start().catch(function (err) {
        return console.error(err.toString());
    });

    // Receive SignalR messages
    connection.on("ReceiveMessage", function (message) {
        if (message) {
            key.sessionId = message.sessionId;

            // Open notification modal popup on the page and based on the message type received from API, display content in the modal
            $("#notificationModal").modal("show");
            switch (message.type) {
                case "display":
                    $("#txtContent0").html(message.text[0]);
                    $("#txtContent1").html(message.text[1]);
                    $("#btnAccept").hide();
                    $("#btnClose").hide();
                    break;
                case "receipt": // Receipt is not displayed in a model. So we assign it to 'receipt' string to be displayed on the page
                    if (message.text && message.text.length > 0) {
                        for (var i = 0; i < message.text.length; i++) {
                            receipt = receipt + message.text[i] + `\n`;
                        }
                    }
                    break;
                case "status":
                    $("#txtContent0").html("GOT STATUS");
                    $("#txtContent1").html("PRESS CLOSE");
                    $("#btnAccept").hide();
                    $("#btnClose").hide();
                    assignStatusResponse(message.response);
                    break;
                case "logon":
                    $("#txtContent0").html("LOGON DONE");
                    $("#txtContent1").html("PRESS CLOSE");
                    $("#btnAccept").hide();
                    $("#btnClose").hide();
                    assignLogonResponse(message.response);
                    break;
                default:
                    $("#txtContent0").html(message.text[0]);
                    $("#txtContent1").html(message.text[1]);
                    $("#btnAccept").hide();
                    $("#btnClose").hide();
                    break;
            }

            // Based on the button type received in the message object, display the button on the modal with appropriate name
            if (message.type != "receipt") { // Receipt is not displayed in a model. So we don't check for button type               
                if (message.okButton) {
                    $("#btnClose").show().html("Ok");
                }
                if (message.cancelButton) {
                    $("#btnClose").show().html("Close");
                }
            }
        }
    });

    // Assign status response to be displayed on the page. 
    function assignStatusResponse(statusResponse) {
        $("#divStatus").show(function () {
            $("#lblResponseCode").html(statusResponse.responseCode);
            $("#lblResponseText").html(statusResponse.responseText);
            $("#lblPinPadVersion").html(statusResponse.pinPadVersion);
            $("#lblPinPadSerialNumber").html(statusResponse.pinPadSerialNumber);
        });
    }

    // Assign logon response to be displayed on the page. 
    function assignLogonResponse(logonResponse) {
        $("#divStatus").show(function () {
            $("#lblResponseCode").html(logonResponse.responseCode);
            $("#lblResponseText").html(logonResponse.responseText);

            // If receipt content is obtained in the response, display the receipt on the page
            if (receipt) {
                $("#divReceipt").show(function () {
                    $("#txtReceipt").html(receipt);
                });
            }
        });
    }


    // Below section contains button click event handling in the modal popups. When user clicks the button, the 'key' object is populated and a method called 'OnPostSendKeyAsync' in the 'Index.cshtml.cs' is called 

    $('#btnClose').click(function () {
        if ($("#btnClose").html() === "Ok") {
            key.key = KeyTypes.OkCancel;
            sendKey();
        }
        $("#notificationModal").modal("hide");
    });

    function sendKey() {
        $.ajax({
            url: configSettings.apiServerUri + "Index?handler=SendKey",
            type: 'post',
            beforeSend: function (xhr) {
                xhr.setRequestHeader("XSRF-TOKEN",
                    $('input:hidden[name="__RequestVerificationToken"]').val());
            },
            data: JSON.stringify(key),
            contentType: "application/json; charset=utf-8",
            success: function (response, textStatus, xhr) {
            },
            error: function (xhr, textStatus, errorThrown) {
                $("#btnAccept").hide();
                $("#btnClose").hide();
                $("#notificationModal").hide();
                console.error(errorThrown);
            }
        });
    }
});
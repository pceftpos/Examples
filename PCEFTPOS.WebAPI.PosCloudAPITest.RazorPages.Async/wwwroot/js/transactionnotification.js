"use strict";

$(document).ready(function () {
    // Get 'apiServerUri' from config file
    var apiServerUri = configSettings.apiServerUri;

    var KeyTypes = { "OkCancel": 0, "YesAccept": 1, "NoDecline": 2, "Auth": 3 };
    Object.freeze(KeyTypes);

    var key = {
        data: "",
        sessionId: "",
        key: ""
    };

    var receipt = "";

    var regexDecimal = /^(0|0?[1-9]\d*)\.\d\d$/;

    // Validation for amount entered by the user
    $("#inputAmount").keyup(function () {
        if (!regexDecimal.test($("#inputAmount").val())) {
            $("#errorAmt").html("Please enter a value with exactly two decimal points.");
            $("#btnSubmit").attr("disabled", true);
        }
        else {
            $("#errorAmt").html("");
            $("#btnSubmit").attr("disabled", false);
        }
    });

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
                case "transaction":
                    $("#txtContent0").html("TXN FINISHED");
                    $("#txtContent1").html("PRESS CLOSE");
                    $("#btnAccept").hide();
                    $("#btnClose").hide();
                    assignTransactionResponse(message.response);
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
                if (message.authButton) {
                    $("#txtAuth").show();
                    $("#btnAccept").show().html("Auth");
                }
                if (message.yesButton) {
                    $("#btnAccept").show().html("Yes");
                }

                if (message.noButton) {
                    $("#btnClose").show().html("No");
                }
                if (message.cancelButton && message.type != "transaction") {
                    $("#btnClose").show().html("Cancel");
                }
                if (message.cancelButton && message.type == "transaction") {
                    $("#btnClose").show().html("Close");
                }
                if (message.okButton) {
                    $("#btnClose").show().html("Ok");
                }
            }
        }
    });

    // Assign transaction response to be displayed on the page. 
    function assignTransactionResponse(txnResponse) {
        $("#divTransaction").show(function () {
            $("#lblResCode").html(txnResponse.responseCode);
            $("#lblResText").html(txnResponse.responseText);
            $("#lblRef").html(txnResponse.txnRef);
            $("#lblTranType").html(txnResponse.txnType);
            $("#lblAmount").html((txnResponse.amtPurchase / 100).toFixed(2));
            $("#lblCash").html((txnResponse.amtCash / 100).toFixed(2));
            $("#lblTip").html((txnResponse.amtTip / 100).toFixed(2));
            if (txnResponse.balanceReceived) {
                $("#lblAvBal").html((txnResponse.availableBalance / 100).toFixed(2));
            }

            $("#lblAccType").html(txnResponse.cardAccountType);
            $("#lblCardName").html(txnResponse.cardName);
            $("#lblCardType").html(txnResponse.cardType);
            $("#lblCAID").html(txnResponse.caid);
            $("#lblCATID").html(txnResponse.catid);
            $("#lblDate").html(txnResponse.date);
            $("#lblExpDate").html("");
            $("#lblMerchant").html(txnResponse.merchant);
            $("#lblPan").html(txnResponse.pan);
            $("#lblRRN").html(txnResponse.rrn);
            $("#lblTrack2").html(txnResponse.track2);

            // If receipt content is obtained in the response, display the receipt on the page
            if (receipt) {
                $("#divReceipt").show(function () {
                    $("#txtReceipt").html(receipt);
                });
            }
        });
    }


    // Below section contains button click event handling in the modal popups. When user clicks the button, the 'key' object is populated and a method called 'OnPostSendKeyAsync' in the 'Index.cshtml.cs' is called 

    $('#btnAccept').click(function () {
        if ($("#btnAccept").html() === "Auth") {
            key.data = $("#txtAuth").val();
            $("#txtAuth").val("").hide();
            key.key = KeyTypes.Auth;
        }
        if ($("#btnAccept").html() === "Yes") {
            key.key = KeyTypes.YesAccept;
        }
        $("#notificationModal").modal("hide");
        sendKey();
    });

    $('#btnClose').click(function () {
        if ($("#btnClose").html() === "No") {
            key.key = KeyTypes.NoDecline;
            sendKey();
        }
        if ($("#btnClose").html() === "Ok" || $("#btnClose").html() === "Cancel") {
            key.key = KeyTypes.OkCancel;
            sendKey();
        }
        $("#notificationModal").modal("hide");
    });

    function sendKey() {
        $.ajax({
            url: apiServerUri + "Index?handler=SendKey",
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
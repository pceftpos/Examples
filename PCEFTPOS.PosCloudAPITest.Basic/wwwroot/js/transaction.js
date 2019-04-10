"use strict";

$(document).ready(function () {

    // Set appSettings and request objects
    var appSettings = {
        tokenServer: "https://pceftpos-authenticationservice-sandbox.azurewebsites.net/v1/",
        cloudAPIUri: "https://localhost:<YOUR PORT NUMBER>/v1",
        posName: "<YOUR POS NAME>",
        posVersion: "<YOUR POS VERSION>",
        posId: "<UNIQUE UUID THAT IDENTIFIES YOUR POS>",
        merchant: "00",
        application: "00",
        pinpadUsername: "<YOUR PINPAD USERNAME>",
        pinpadPassword: "<YOUR PINPAD PASSWORD>",
        pinpadPairCode: "<YOUR PINPAD PAIRING CODE>"
    }
    var tokenRequest = {
        posId: appSettings.posId,
        posName: appSettings.posName,
        posVersion: appSettings.posVersion,
        username: appSettings.pinpadUsername,
        password: appSettings.pinpadPassword,
        pairCode: appSettings.pinpadPairCode
    };
    
    var token = ""; // The authentication token
    var sessionUUID = ""; // The generated sessionUUID value is a global variable to make 'GET' transaction request with the same UUID if a post doesn't respond with a '200' status

    // Function to generate UUID which is the 'sessionUUID' used in the requests
    function generateUUID() {
        return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
            var r = Math.random() * 16 | 0, v = c == 'x' ? r : (r & 0x3 | 0x8);
            return v.toString(16);
        });
    }

    // Regex validation for an input field so that it has a number with exactly two decimal points
    var regexDecimal = /^(0|0?[1-9]\d*)\.\d\d$/;
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

    // Function to generate 'txnRef', which is sent as a part of 'eFTTransactionRequest' object
    function generateAlphaNumStr(len) {
        var rdmString = "";
        for (; rdmString.length < len; rdmString += Math.random().toString(36).substr(2));
        return rdmString.substr(0, len);
    }

    $('#btnSubmit').click(function () {
        $("#responseModal").modal("show");
        $("#btnClose").hide();
        $("#txtContent").html("PLEASE WAIT...");

        // Get authentication token
        $.ajax({
            url: appSettings.tokenServer + "tokens/cloudpos",
            type: 'post',
            data: JSON.stringify(tokenRequest),
            contentType: "application/json; charset=utf-8",
            success: function (response, textStatus, xhr) {
                token = response.token;
                sessionUUID = generateUUID();
                getTransactionResponse(token, sessionUUID);
            },
            error: function (xhr, textStatus, errorThrown) {
                console.error(errorThrown);
            }
        });
    });

    // Function to 'POST' a transaction request and wait for the response
    function getTransactionResponse(token, sessionUUID) {
        var eFTTransactionRequest = {
        request: {
            txnType: "P",
            txnRef: (generateAlphaNumStr(16)).toUpperCase(),
            amtPurchase: parseInt($("#inputAmount").val() * 100),
            merchant: appSettings.merchant,
            application: appSettings.application,
            basket: // Create a sample json 'basket' object which is sent as part of transaction request 
            {
                id: generateUUID(),
                amt: 18700,
                tax: 1760,
                dis: 650,
                sur: 374,
                items: [
                    {
                        id: "t39kq002",
                        sku: "k24086723",
                        qty: 2,
                        amt: 2145,
                        tax: 200,
                        dis: 50,
                        name: "XData USB Drive"
                    },
                    {
                        id: "t39kq003",
                        sku: "s23475697",
                        qty: 1,
                        amt: 8910,
                        tax: 810,
                        dis: 50,
                        name: "MSoft OSuite",
                        srl: "ms7843k346j23" 
                    },
                    {
                        id: "t39kq004",
                        sku: "m47060855",
                        qty: 5,
                        amt: 1100,
                        tax: 110,
                        dis: 110,
                        name: "A4 Notepad"
                    }
                ]
            } 
        }
    };
        
        $.ajax({
            url: appSettings.cloudAPIUri + "sessions/" + sessionUUID + "/transaction",
            type: 'post',
            beforeSend: function (xhr) {
                xhr.setRequestHeader('Authorization', 'Bearer ' + token);
            },
            data: JSON.stringify(eFTTransactionRequest),
            contentType: "application/json; charset=utf-8",
            success: function (response, textStatus, xhr) {
                if (xhr.status == 200) {
                    $("#txtContent").html(response.response.responseText);
                    $("#btnClose").show();
                }
            },
            error: function (xhr, textStatus, errorThrown) {
                if (xhr.status == 400) {
                    $("#txtContent").html("INVALID REQUEST. THE TRANSACTION HAS NOT COMPLETED SUCCESSFULLY");
                    $("#btnClose").show();
                }
                else if (xhr.status == 404) {
                    $("#txtContent").html("REQUESTED RESOURCE IS UNAVAILABLE. THE TRANSACTION WAS UNSUCCESSFUL");
                    $("#btnClose").show();
                }
                else if (xhr.status == 408 || (xhr.status >= 500 && xhr.status <= 599)) {
                    enterErrorRecovery(sessionUUID);
                }
                else {
                    $("#txtContent").html("AN ERROR OCCURED. PLEASE TRY AGAIN LATER");
                    $("#btnClose").show();
                }
            }
        });
    }

    // If the above request doesn't return a response with status '200', we enter into an error recovery mode and make a 'GET' request with same sessionUUID
    function enterErrorRecovery(sessionUUID) {
        $.ajax({
            url: appSettings.cloudAPIUri + "sessions/" + sessionUUID + "/transaction",
            type: 'get',
            beforeSend: function (xhr) {
                xhr.setRequestHeader('Authorization', 'Bearer ' + token);
            },
            contentType: "application/json; charset=utf-8",
            success: function (response, textStatus, xhr) {
                if (xhr.status == 200 && response.response.success) {
                    $("#txtContent").html(response.response.responseText);
                    $("#btnClose").show();
                }
                else if (xhr.status == 202) {
                    setTimeout(function () { enterErrorRecovery(sessionUUID) }, 10000);
                }
            },
            error: function (xhr, textStatus, errorThrown) {
                if (xhr.status == 400) {
                    $("#txtContent").html("INVALID REQUEST. THE TRANSACTION HAS NOT COMPLETED SUCCESSFULLY");
                    $("#btnClose").show();
                }
                else if (xhr.status == 404) {
                    $("#txtContent").html("REQUESTED RESOURCE IS UNAVAILABLE. THE TRANSACTION WAS UNSUCCESSFUL");
                    $("#btnClose").show();
                }
                else if (xhr.status == 408 || (xhr.status >= 500 && xhr.status <= 599)) {
                    setTimeout(function () { enterErrorRecovery(sessionUUID) }, 10000);
                }
                else {
                    $("#txtContent").html("AN ERROR OCCURED. PLEASE TRY AGAIN LATER");
                    $("#btnClose").show();
                }
            }
        });
    }

    $('#btnClose').click(function () {
        $("#responseModal").modal("hide");
    });
});

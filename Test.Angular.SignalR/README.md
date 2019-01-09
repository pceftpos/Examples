
# __Test POS Client-Server Application__

This project represents sample synchronous web application as a point of sale, using the Sandbox PC-EFTPOS Cloud REST API Client for communication with a physical or virtual Cloud-enabled pinpad.

Once the Transaction request is started, the Client 

This sample project represents a synchronous, client-server, web-based point of sale, using the Sandbox PC-EFTPOS Cloud REST API for communication with a physical or virtual Cloud-enabled pinpad. It uses the open-source [SignalR](https://www.asp.net/signalr) framework for server-to-client communication, while the server includes background services to handle all API requests, responses and notification postbacks.

Once a Transaction request is initiated from the client to the server, waits up to 3 minutes for a *200 OK* response code. Typically synchronous point of sale systems would not expect to receive notification postbacks, however the PC-EFTPOS Cloud REST API does support notification postbacks, even when using synchronous mode. All postbacks are processed with SignalR notifications, while the client continues to wait for the transaction response.  If the Transaction request is not finished within 3 minutes or the response fails (with HTTP 4xx, 5xx codes), the transaction enters recovery mode and another 3 minute timer starts to try and get the transaction status from the API, ie. did the EFT transaction succeed or fail.

#### Technologies used
- Client: Angular 5
- Server: ASP .Net Core 2.1

#### Requirements
- Visual Studio 2017 with .Net Core 2.1 framework
- NodeJs ([download](https://nodejs.org/en/download/))

#### Glossary
|         Word            |                               Meaning                                 |
| ------------------------|-----------------------------------------------------------------------|
| POS                     | Point of sale                                                         |
| Client, POS Client      | Point of sale client application, In this project it's an Angular 5 application in the `ClientApp` folder |
| Server, POS Server | Point of sale server application. In this project it's a .Net Core 2.1 application|
| REST API, API           | PC-EFTPOS Cloud REST API                                              |
| Notifications           | Messages from the REST API, which include messages from the pinpad screen, receipts and the final transaction response.               |

## __Getting started__
#### Preparation
##### 1. Set up your pinpad:
* You will need a PC-EFTPOS Cloud Username and Password
* Follow the bank/pinpad instructions to login and get the Pairing Code
##### 2. Set up project settings
Open "appsettings.json" in the project root and update the `posName`, `posVersion`, `posId` , `notificationUri`, `pinpadUsername`, `pinpadPassword`, `pinpadPairCode` values under the `AppSettings` section with your project URI and the parameters from the POS and pinpad:
```json
      {
	      "AppSettings": {
			   "posName": "<YOUR POS NAME>",
			   "posVersion": "<YOUR POS VERSION>",
			   "posId": "<YOUR POS GUID>",
			   "notificationUri": "<YOUR POS SERVER API URI>/pceftposnotify/{{sessionid}}/{{type}}",
			   "pinpadUsername": "<YOUR PINPAD USERNAME>",
			   "pinpadPassword": "<YOUR PINPAD PASSWORD>",
			   "pinpadPairCode": "<YOUR PINPAD PAIRING CODE>"
          }
      }
```
        
__Please note, if you want to get notifications from the REST API you should deploy your POS server. The notifications will not be shown at the Client for localhost__
     
To set POS client communication with server open the "config.json" file found in **Test.Angular.SignalR.Async\ClientApp\src\assets\config.json** and set the `uri` value under `apiServer`:
 ```json
   {
         "apiserver": {
              "uri": "<YOUR POS SERVER URI>/",
          }
   }
```

##### 3. Set up the project
In the project properties under the Debug section make sure you use:
* Profile: IIS Express
* Launch: IIS Express
* Enable SSL checkbox is selected
    
__You can use `https://localhost:<YOUR PORT NUMBER>/` as `<YOUR POS SERVER URI>` mentioned above, but you will not get the notifications for localhost__
If you decide to use localhost, please update the files mentioned in step 2 with `https://localhost:<YOUR PORT NUMBER>/api/v1` instead of `<YOUR POS SERVER URI>`

Make sure you have the certificate to run the service over SSL. When prompted, you can safely accept the IIS Express generated certificate for this test application:

![Certificate](Docs/certificate.png)

#### Run
Build the project, make sure you use the right NuGet packages and other Dependencies:
* NuGet:
    Microsoft.AspNetCore.App (2.1.0)
* SDK:
    Microsoft.AspNetCore.App (2.1.0)
    Microsoft.NETCore.App (2.1.0)

## __Usage__
__Please note, if you want to get notifications from the REST API you should deploy your POS server. The notifications will not be shown at the Client for localhost__

Once the application is running you will see the following page with Home, Pinpad and Settings tabs.<br/>
![POS Start](Docs/pos_home.png)

#### Check Status
First of all go to the Pinpad tab and click the "Status" button to check it. The notification shows the completed operation:<br/>
![Status](Docs/notification_status.png)

The following picture shows the successful status<br/>
![POS Pinpad Status](Docs/pos_pinpad_status.png)

#### Do Logon
If the status is "LOGON REQUIRED"<br/>
![POS Pinpad Status Logon required](Docs/pos_logon_required.png)

Click the "Logon" button to finish the pinpad set up. You will see the following sequence of notifications showing using the SignalR library.<br/>
__Notification sequence may vary depending on pinpad.__<br/>
![Logon Init](Docs/notification_logon.png),   ![Logon Approve](Docs/notification_approve.png),   ![Logon Done](Docs/notification_logon_done.png)

Close the last notification. The final POS view should appear as follows when the logon is complete:<br/>
![POS Pinpad Logon done](Docs/pos_logon_done.png)

#### Do transaction
To create a transaction, just go to the Home tab and click the Transaction button to create a $1.00 EFTPOS transaction or $20.00 Oxipay transaction (sum may vary depending on minimum Oxipay transaction setting).<br/> 

##### EFTPOS Transaction
You will see the following sequence of notifications shown using SignalR library for __Eftpos__ payment: <br/>
__Notification sequence may vary depending on pinpad__<br/>
![Txn Swipe Card](Docs/notification_swipe_card.png), ![Txn Select Account](Docs/notification_enter_acc.png), ![Txn Enter pin](Docs/notification_enter_pin.png), ![Txn Process](Docs/notification_wait.png), ![Txn Approved](Docs/notification_approve.png), ![Txn Finish](Docs/notification_finish.png)

Once the transaction is finished, close the last notification. The POS Client will show transaction details and the receipt:<br/>
![POS Txn](Docs/pos_txn_done.png)

##### Oxipay Transaction
To make the Oxipay transaction, you need a 6-digit Oxipay barcode, contact Oxipay for more information. <br/>

You will see the following sequence of notifications shown using SignalR library for __Oxipay__ payment: <br/>
__Notification sequence may vary depending on pinpad__<br/>
![Oxipay_payment](Docs/oxipay_payment.PNG), ![Txn Process](Docs/notification_wait.png), ![Txn Approved](Docs/notification_approve.png), ![Txn Finish](Docs/notification_finish.png)

Once the transaction is finished, close the last notification. The POS Client will show transaction details.<br/>
![Oxipay Txn](Docs/oxipay_txn_done.png)

#### Make a refund
To make a refund input refund Amount and select a Refund checkbox on Home page: <br/>
![Refund EFTPOS](Docs/refund_eftpos.PNG)<br/>

##### EFTPOS Refund
Click on EFTPOS button to process. You will see the following sequence of notifications showing using the SignalR library. <br/>
__Notification sequence may vary depending on pinpad.__<br/>
![Txn Swipe Card](Docs/notification_swipe_card.png), ![Txn Select Account](Docs/notification_enter_acc.png), ![Txn Enter pin](Docs/notification_enter_pin.png), ![Txn Process](Docs/notification_wait.png), ![Txn Approved](Docs/notification_approve.png), ![Txn Finish](Docs/notification_finish.png)<br/>

##### Oxipay Refund
To make an Oxipay refund you need a REF code of Oxipay transaction: <br/>
![Oxipay REF](Docs/oxipay_txn_done_REF.png) <br/>

Once the Amount is set, Refund checkbox selected and Refund Reference input:<br/>
![Oxipay Refund](Docs/refund_oxipay.png) <br/>

Process with click "Oxipay" button.You will see the following sequence of notifications showing using the SignalR library. <br/>
__Notification sequence may vary depending on pinpad.__<br/>
![Txn Process](Docs/notification_wait.png), ![Txn Approved](Docs/notification_approve.png), ![Txn Finish](Docs/notification_finish.png)<br/>

Once the transaction is finished, close the last notification. The POS Client will show transaction details.

#### Decline transaction
You can decline a transaction by clicking the "Cancel" button on one of the Notifications (Swipe Card, Enter Account, Enter Pin) before the transaction is processed. When you cancel a transaction you will see this notification: <br/>
![Txn Cancelled](Docs/notification_txn_cancelled.png)

and the POS Client will show the transaction data (Receipt may be shown or not. It depends on the pinpad settings)<br/>
![POS Txn Cancelled](Docs/pos_txn_cancelled.png)

#### Settings
On the Settings tab you can update the POS Server and default transaction amount used on the Home page. This is stored in **ClientApp\src\assets\config.json**<br/>
![POS Settings](Docs/pos_settings.png)

## __Troubleshooting__
* Make sure the pinpad is set up and running
* Check you installed everything from the "Requirements" section
* Check you updated the "appsettings.json" and "config.json" files
* Check the NuGet packages are installed and up to date
* Deploy the application to see the notifications on the Client screen
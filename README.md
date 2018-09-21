#### Projects description

These projects represent sample web applications as a point of sale(POS), using the Sandbox PC-EFTPOS Cloud REST API Client for communication with a physical or virtual cloud-enabled pinpad.

Projects were created using Angular 5 or ASP.NET Core Razor Pages or single html page with jquery for POS Client and ASP.NET Core 2.1 for POS Server.

|         Project           |                                       Description                                                              |
| ------------------------|-------------------------------------------------------------------------------------------------------------------------------------|
| PCEFTPOS.PosCloudAPITest.Basic       			   | Sample synchronous web application as a point of sale. A simple app to make a transaction request and wait for the final transaction response. |
| PCEFTPOS.WebAPI.PosCloudAPITest.Angular          | Sample synchronous web application as a point of sale. Uses timed Get requests to get notifications from Cloud REST API. |
| PCEFTPOS.WebAPI.PosCloudAPITest.Angular.Async    | Sample asynchronous web application as a point of sale. Uses timed Get requests to get notifications from Cloud REST API. |
| PCEFTPOS.WebAPI.PosCloudAPITest.RazorPages.Async | Sample asynchronous web application as a point of sale. The pinpad notifications are processed with the SignalR library. |
| Test.Angular.SignalR                             | Sample synchronous web application as a point of sale. The pinpad notifications are processed with the SignalR library.|
| Test.Angular.SignalR.Async                       | Sample asynchronous web application as a point of sale.  The pinpad notifications are processed with the SignalR library.|

#### Important:

Please note, if you want to get notifications from the REST API you should deploy your point of sale API. The notifications will not be shown at the Client for localhost.
If you decide to process with localhost, please use the synchronous sample. Asynchronous samples use notification to get operation result.
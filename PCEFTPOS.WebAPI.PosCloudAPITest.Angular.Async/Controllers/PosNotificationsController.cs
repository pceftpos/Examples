using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PCEFTPOS.WebAPI.PosCloudAPITest.Angular.Async.Model;
using PCEFTPOS.WebAPI.PosCloudAPITest.Angular.Async.Data.Interface;

namespace PCEFTPOS.WebAPI.PosCloudAPITest.Angular.Async.Controllers
{
    /// <summary>
    /// Notifications controller
    /// </summary>
    [Authorize(Policy = "AuthenticatedPosNotifyUser")]
    [Route("api/v1/pceftposnotify")]
    public class PosNotificationsController : ControllerBase
    {
        private INotificationRepository notificationRepository;
        private ISessionRepository sessionRepository;

        public PosNotificationsController(INotificationRepository notificationRepository, ISessionRepository sessionRepository)
        {
            this.notificationRepository = notificationRepository;
            this.sessionRepository = sessionRepository;
        }

        /// <summary>
        /// New logon type notification sent from rest API
        /// </summary>
        /// <param name="session">Session Id</param>
        /// <param name="apiResponse">Logon data</param>
        /// <returns></returns>
        [HttpPost("{session}/logon")]
        public async Task<ActionResult> PostLogonNotificationAsync([FromRoute] string session, [FromBody] ApiNotificationResponse<EFTLogonResponse> apiResponse)
        {
            await notificationRepository.AddMessageAsync(new Message
            {
                Response = apiResponse?.Response,
                SessionId = session,
                Text = apiResponse?.Response?.ResponseText.Split("\n") ?? new string[] { "", "" },
                Type = apiResponse?.Response?.ResponseType,                                
                CancelButton = true,             
            });

            return Ok();
        }

        /// <summary>
        /// New transaction type notification sent from rest API
        /// </summary>
        /// <param name="session">Session Id</param>
        /// <param name="apiResponse">Transaction data</param>
        /// <returns></returns>
        [HttpPost("{session}/transaction")]
        public async Task<ActionResult> PostTransactionNotificationAsync([FromRoute] string session, [FromBody] ApiNotificationResponse<EFTTransactionResponse> apiResponse)
        {
            await notificationRepository.AddMessageAsync(new Message
            {
                Response = apiResponse.Response,
                SessionId = session,                
                Text = apiResponse?.Response?.ResponseText.Split("\n") ?? new string[] { "", "" },
                Type = apiResponse?.Response?.ResponseType,
                CancelButton = true,
            });

            await sessionRepository.DeleteSessionAsync(session);

            return Ok();
        }

        /// <summary>
        /// New display type notification sent from rest API
        /// </summary>
        /// <param name="session">Session Id</param>
        /// <param name="apiResponse">Display data</param>
        /// <returns></returns>
        [HttpPost("{session}/display")]
        public async Task<ActionResult> PostDisplayNotificationAsync([FromRoute] string session, [FromBody] ApiNotificationResponse<EFTDisplayResponse> apiResponse)
        {
            if (apiResponse?.Response != null)
            {
                for (var i = 0; i < apiResponse.Response.DisplayText.Length; i++)
                {
                    apiResponse.Response.DisplayText[i] = apiResponse.Response.DisplayText[i].Trim();
                }
            
                await notificationRepository.AddMessageAsync(new Message
                {
                    SessionId = session,                    
                    Text = apiResponse.Response.DisplayText ?? new string[] { "", "" },
                    Type = apiResponse.Response.ResponseType,
                    AuthButton = apiResponse.Response.AuthoriseKeyFlag,
                    YesButton = apiResponse.Response.AcceptYesKeyFlag,
                    NoButton = apiResponse.Response.DeclineNoKeyFlag,
                    CancelButton = apiResponse.Response.CancelKeyFlag,
                    OkButton = apiResponse.Response.OKKeyFlag,
                });

                return Ok();
            }
            return BadRequest();
        }

        /// <summary>
        /// New receipt sent from rest API
        /// </summary>
        /// <param name="session">Session Id</param>
        /// <param name="apiResponse">Receipt data</param>
        /// <returns></returns>
        [HttpPost("{session}/receipt")]
        public async Task<ActionResult> PostReceiptNotificationAsync([FromRoute] string session, [FromBody] ApiNotificationResponse<EFTReceiptResponse> apiResponse)
        {
            await notificationRepository.AddMessageAsync(new Message
            {
                Response = apiResponse?.Response,
                SessionId = session,                
                Text = apiResponse?.Response?.ReceiptText ?? new string[] { "", "" },
                Type = apiResponse?.Response?.ResponseType,
                CancelButton = true,
            });

            return Ok();
        }

        /// <summary>
        /// New status type notification sent from rest API
        /// </summary>
        /// <param name="session">Session Id</param>
        /// <param name="apiResponse">Status data</param>
        /// <returns></returns>
        [HttpPost("{session}/status")]
        public async Task<ActionResult> PostStatusNotificationAsync([FromRoute] string session, [FromBody] ApiNotificationResponse<EFTStatusResponse> apiResponse)
        {
            await notificationRepository.AddMessageAsync(new Message() {
                SessionId = apiResponse?.SessionId,
                Type = "status",
                Response = apiResponse?.Response });

            return Ok();
        }


        /// <summary>
        /// Get new notification if any
        /// </summary>
        /// <returns>Message</returns>
        [HttpGet("message")]
        public async Task<Message> GetMessageAsync()
        {
            var message = await notificationRepository.GetLastMessageAsync();
            await notificationRepository.DeleteMessageAsync(message);
            
            return message;
        }
    }
}

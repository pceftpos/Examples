using System;
using System.ComponentModel.DataAnnotations;

namespace PCEFTPOS.WebAPI.PosCloudAPITest.RazorPages.Async.Model
{
    public class EFTRequest
    {
        protected bool isStartOfTransactionRequest = true;
        protected Type pairedResponseType = null;

        private EFTRequest()
        {

        }

        public EFTRequest(bool isStartOfTransactionRequest, Type pairedResponseType)
        {
            if (pairedResponseType != null && pairedResponseType.IsSubclassOf(typeof(EFTResponse)) != true)
            {
                throw new InvalidOperationException("pairedResponseType must be based on EFTResponse");
            }

            this.isStartOfTransactionRequest = isStartOfTransactionRequest;
            this.pairedResponseType = pairedResponseType;
        }

        public virtual bool GetIsStartOfTransactionRequest() { return isStartOfTransactionRequest; }
        public virtual Type GetPairedResponseType() { return pairedResponseType; }
    }
}

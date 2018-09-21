using System;

namespace PCEFTPOS.WebAPI.PosCloudAPITest.RazorPages.Async.Model
{
    public class EFTResponse
    {
        public string ResponseType { get; set; }
        protected Type pairedRequestType = null;

        private EFTResponse()
        {
        }

        public EFTResponse(Type pairedRequestType, string responseType)
        {
            if (pairedRequestType != null && pairedRequestType.IsSubclassOf(typeof(EFTRequest)) != true)
            {
                throw new InvalidOperationException("pairedRequestType must be based on EFTRequest");
            }

            this.pairedRequestType = pairedRequestType;
            this.ResponseType = responseType;
        }

        public virtual Type GetPairedRequestType() { return pairedRequestType; }
    }
}

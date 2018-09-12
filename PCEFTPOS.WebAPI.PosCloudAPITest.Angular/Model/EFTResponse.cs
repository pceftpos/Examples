using System;

namespace PCEFTPOS.WebAPI.PosCloudAPITest.Angular.Model
{
    /// <summary>Abstract base class for EFT client responses.</summary>
    public abstract class EFTResponse
    {
        /// <summary>
        /// Response type
        /// </summary>
        public string ResponseType { get; set; }

        /// <summary>
        /// Paired request type
        /// </summary>
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

        /// <summary>
        /// Indicates the paired EFTRequest for this EFTResponse if one exists. Null otherwise.
        /// e.g. EFTLogonResponse will have a paired EFTLogonRequest request
        /// </summary>
        public virtual Type GetPairedRequestType() { return pairedRequestType; }
    }
}

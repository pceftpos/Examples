﻿using System;

namespace Test.Angular.SignalR.Model
{
    /// <summary>Abstract base class for EFT client responses.</summary>
    public abstract class EFTResponse
    {
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
        }

        /// <summary>
        /// Indicates the paired EFTRequest for this EFTResponse if one exists. Null otherwise.
        /// e.g. EFTLogonResponse will have a paired EFTLogonRequest request
        /// </summary>
        public virtual Type GetPairedRequestType() { return pairedRequestType; }
    }
}

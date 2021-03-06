﻿using System;
using System.ComponentModel.DataAnnotations;

namespace Test.Angular.SignalR.Async.Model
{
    /// <summary>Abstract base class for EFT client requests.</summary>
    public abstract class EFTRequest
    {
        /// <summary>
        /// Flag if it's start of transaction request
        /// </summary>
        protected bool isStartOfTransactionRequest = true;

        /// <summary>
        /// Response type
        /// </summary>
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

        /// <summary>
        /// True if this request starts a paired transaction request/response with displays etc (i.e. transaction, logon, settlement etc)
        /// </summary>
        public virtual bool GetIsStartOfTransactionRequest() { return isStartOfTransactionRequest; }

        /// <summary>
        /// Indicates the paired EFTResponse for this EFTRequest if one exists. Null otherwise.
        /// e.g. EFTLogonRequest will have a paired EFTLogonResponse response
        /// </summary>
        public virtual Type GetPairedResponseType() { return pairedResponseType; }

        
    }
}

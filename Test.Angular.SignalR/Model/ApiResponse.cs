﻿namespace Test.Angular.SignalR.Model
{
    /// <summary>
    /// Cloud API response
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ApiResponse<T>
    {
        public ApiResponse() : this(default(T))
        {
        }

        public ApiResponse(T data)
        {
            Response = data;
        }

        /// <summary> ResponseType </summary>       
        public string ResponseType { get; set; }

        /// <summary>
        /// Response body
        /// </summary>
        public T Response { get; set; }
    }
}

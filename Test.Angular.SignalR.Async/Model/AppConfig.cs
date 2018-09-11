namespace Test.Angular.SignalR.Async.Model
{
    /// <summary> Client application configuration</summary>
    public class AppConfig
    {
        /// <summary> API server settings</summary>
        public ApiServer ApiServer { get; set; }

        /// <summary> Application settings</summary>
        public Application Application { get; set; }
    }

    /// <summary>
    /// API server settings
    /// </summary>
    public class ApiServer
    {
        /// <summary> Server URI</summary>
        public string Uri { get; set; }
    }

    /// <summary>
    /// Application settings
    /// </summary>
    public class Application
    {
        /// <summary> Defaul transaction amount</summary>
        public string DefaultAmount { get; set; }
    }
}

namespace Test.Angular.SignalR.Model
{
    /// <summary> Client application configuration</summary>
    public class AppConfig
    {
        /// <summary> API server settings</summary>
        public ApiServer apiServer { get; set; }

        /// <summary> Application settings</summary>
        public Application application { get; set; }
    }

    /// <summary>
    /// API server settings
    /// </summary>
    public class ApiServer
    {
        /// <summary> Server URI</summary>
        public string uri { get; set; }
    }

    /// <summary>
    /// Application settings
    /// </summary>
    public class Application
    {
        /// <summary> Defaul transaction amount</summary>
        public string defaultAmount { get; set; }
    }

    public enum ExtensionType
    {
        AfterPay = 65,
        Oxipay = 67
    }
}

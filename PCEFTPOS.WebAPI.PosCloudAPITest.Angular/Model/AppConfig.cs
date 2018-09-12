namespace PCEFTPOS.WebAPI.PosCloudAPITest.Angular.Model
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
        /// <summary> Timer for GetTransaction request in seconds</summary>
        public int Timer { get; set; }

        /// <summary> Defaul transaction amount</summary>
        public string DefaultAmount { get; set; }

        /// <summary> Wait for GetTransaction response period in minutes</summary>
        public int Wait { get; set; }
    }
}

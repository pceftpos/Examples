namespace PCEFTPOS.WebAPI.PosCloudAPITest.Angular.Async.Model
{
    /// <summary> Defines settings for creating a signed JWT (JWE)</summary>
    public class JwtSettings
    {
        /// <summary> The 'iss' field in the JWT</summary>
        public string Issuer { get; set; }

        /// <summary> The 'aud' field in the JWT</summary>
        public string Audience { get; set; }

        /// <summary> Symmetric security key used for signing the JWT</summary>
        public string SignKey { get; set; }

        /// <summary> Symmetric security key used for encrypting the JWT</summary>
        public string EncryptKey { get; set; }

        /// <summary> Expiry time of the certificate in days</summary>
        public double ExpiryDays { get; set; }
    }
}

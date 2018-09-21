using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PCEFTPOS.WebAPI.PosCloudAPITest.RazorPages.Async.Model;
using PCEFTPOS.WebAPI.PosCloudAPITest.RazorPages.Async.Data.Interface;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace PCEFTPOS.WebAPI.PosCloudAPITest.RazorPages.Async.Data
{
    public class TokenRepository : ITokenRepository
    {
        JwtSettings jwtSettings;

        public TokenRepository(IOptions<JwtSettings> jwtSettings)
        {
            this.jwtSettings = jwtSettings.Value;
        }

        public PosNotifyToken GetPosNotifyToken(string sessionId)
        {
            // Create claims
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, sessionId),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, sessionId),
                new Claim("PosNotify", "true"),
            };

            // Create JWE
            var signKey = System.Text.Encoding.UTF8.GetBytes(jwtSettings.SignKey);

            var tokenWriter = new JwtSecurityTokenHandler();
            var tokenData = tokenWriter.CreateJwtSecurityToken(
                    issuer: jwtSettings.Issuer,
                    audience: jwtSettings.Audience,
                    subject: new ClaimsIdentity(claims),
                    notBefore: DateTime.UtcNow.AddDays(-1),
                    expires: DateTime.UtcNow.AddDays(jwtSettings.ExpiryDays),
                    issuedAt: DateTime.UtcNow,
                    signingCredentials: new SigningCredentials(new SymmetricSecurityKey(signKey), SecurityAlgorithms.HmacSha512));

            return new PosNotifyToken()
            {
                Token = tokenWriter.WriteToken(tokenData),
                ExpirySeconds = (long)TimeSpan.FromDays(jwtSettings.ExpiryDays).TotalSeconds
            };
        }
    }
}

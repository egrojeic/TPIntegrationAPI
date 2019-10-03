using System;
using System.Configuration;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace WebApiSegura.Security
{

    internal static class TokenGenerator
    {
        public static string GenerateTokenJwt(string username, string rolname)
        {
            var secretKey = "20EE6F5DeyJzdWIiOiIxMDE4NDU0MzY5IiwibmFtZSI6IkRhbmlsbyBDYW50yMzkwMjJ9b3IiLCJpYXQiOjE1MTY0253#Mluf75AwBCCG7Ijl64bZRZo7uaQGbpA4AE7oTJUeaCc3v9xADB1&CE7658djclEF71D0";
            var audienceToken = ConfigurationManager.AppSettings["JWT_AUDIENCE_TOKEN"];
            var issuerToken = ConfigurationManager.AppSettings["JWT_ISSUER_TOKEN"];

            var securityKey = new SymmetricSecurityKey(System.Text.Encoding.Default.GetBytes(secretKey));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(new[] {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, rolname)
            });

            var tokenHandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
            var jwtSecurityToken = tokenHandler.CreateJwtSecurityToken(
                audience: audienceToken,
                issuer: issuerToken,
                subject: claimsIdentity,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddMonths(3),
                signingCredentials: signingCredentials);

            var jwtTokenString = tokenHandler.WriteToken(jwtSecurityToken);
            return jwtTokenString;
        }
    }
}

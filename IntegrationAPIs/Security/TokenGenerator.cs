using System;
using System.Configuration;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using IntegrationAPIs.Models;

namespace IntegrationAPIs.Security
{

    internal static class TokenGenerator
    {
        public static TokenResponse GenerateTokenJwt(string username, string rolname)
        {
            var secretKey = "20EE6F5DeyJzdWIiOiIxMDE4NDU0MzY5IiwibmFtZSI6IkRhbmlsbyBDYW50yMzkwMjJ9b3IiLCJpYXQiOjE1MTY0253#Mluf75AwBCCG7Ijl64bZRZo7uaQGbpA4AE7oTJUeaCc3v9xADB1&CE7658djclEF71D0";
            var audienceToken = "http://localhost:12711";
            var issuerToken = "http://localhost:12711";

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

            TokenResponse response = new TokenResponse();
            response.Token = jwtTokenString;
            response.ExpireDate = DateTime.UtcNow.AddMonths(Convert.ToInt32(3)).ToString("MM-dd-yyyy");

            return response;
        }
    }
}

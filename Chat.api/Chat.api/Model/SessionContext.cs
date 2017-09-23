using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Tokens;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Chat.api.Model
{
    public class SessionContext
    {
        public List<Session> Sessions { get; } = new List<Session>();

        public string CreateUser() => $"user{new Random().Next(999999):000000}";

        public Session CreateSession()
        {
            var creator = CreateUser();
            var session = new Session
            {
                Creator = creator,
                CreatedTime = DateTime.UtcNow,
                SecurityToken = $"Bearer {GenerateToken(creator).RawData}"
            };

            Sessions.Add(session);
            return session;
        }

        public static readonly byte[] Secret = Encoding.UTF8.GetBytes(ConfigurationManager.AppSettings["secret"] ?? "this ain't no secret");
        public static JwtSecurityToken GenerateToken(string username)
        {
            var header = new JwtHeader { ["alg"] = "HS256" };
            var payload = new JwtPayload(new[] { new Claim("username", username) });
            var hmac = new HMACSHA256(Secret);
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(string.Join(".", header.Base64UrlEncode(), payload.Base64UrlEncode())));
            return new JwtSecurityToken(header, payload, header.Base64UrlEncode(), payload.Base64UrlEncode(), System.IdentityModel.Tokens.Base64UrlEncoder.Encode(hash));
        }
    }
}
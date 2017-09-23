using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Chat.api.Model
{
    public class SessionContext
    {
        public Dictionary<int,Session> Sessions { get; } = new Dictionary<int, Session>();
        public Dictionary<int, User> Users { get; } = new Dictionary<int, User>();

        public User CreateUser()
        {
            var user = new User
            {
                Username = $"user{new Random().Next(999999):000000}",
            };

            Users.Add(Users.Count + 1, user);
            return user;
        }

        public Session CreateSession()
        {
            var creator = CreateUser();
            var session = new Session
            {
                Creator = creator,
                CreatedTime = DateTime.UtcNow,
                SecurityToken = GenerateToken(creator.Username).RawData
            };

            Sessions.Add(Sessions.Count + 1, session);
            return session;
        }

        private static readonly string _secret = ConfigurationManager.AppSettings["secret"] ?? "this ain't no secret";
        public static JwtSecurityToken GenerateToken(string username)
        {
            var header = new JwtHeader { ["alg"] = "HS256" };
            var payload = new JwtPayload(new[] { new Claim("username", username) });
            var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(_secret));
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(string.Join(".", header.Base64UrlEncode(), payload.Base64UrlEncode())));
            return new JwtSecurityToken(header, payload, header.Base64UrlEncode(), payload.Base64UrlEncode(), Base64UrlEncoder.Encode(hash));
        }
    }
}
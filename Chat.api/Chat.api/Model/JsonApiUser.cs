using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Chat.api.Model
{
    public class JsonApiUser
    {
        public readonly string type ="users";
        public readonly int id;
        public readonly UserAttributes attributes;
        public readonly SelfLink links;

        internal JsonApiUser(string userUri, string userName, int userId)
        {
            id = userId;
            links = new SelfLink(userUri, userId);
            attributes = new UserAttributes(userName);
        }

        public class UserAttributes
        {
            public readonly string username;

            internal UserAttributes(string name)
            {
                username = name;
            }
        }
    }
}
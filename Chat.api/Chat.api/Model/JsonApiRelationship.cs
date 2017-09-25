using Chat.api.Controllers;

namespace Chat.api.Model
{
    public class JsonApiRelationship
    {
        public readonly Creator creator;

        internal JsonApiRelationship(Session creatorSession, string requestUri)
        {
            creator = new Creator(creatorSession, requestUri);
        }

        public class Creator
        {
            public readonly CreatorLinks links;
            public readonly ApiSessionData data;

            public Creator(Session creatorSession, string requestUri)
            {
                var id = SessionController.Context.Sessions.IndexOf(creatorSession);
                links = new CreatorLinks(requestUri, id);
                data = new ApiSessionData(id);
            }

            public class CreatorLinks
            {
                public readonly string self;
                public readonly string related;

                internal CreatorLinks(string requestUri, int id)
                {
                    self = $"{requestUri}/{id}/relationships/creator";
                    related = $"{requestUri}/{id}/creator";
                }
            }
        }

        public class ApiSessionData
        {
            public readonly string type = "users";
            public readonly int id;

            internal ApiSessionData(int sessionId) { id = sessionId; }
        }
    }
}
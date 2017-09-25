using Chat.api.Controllers;
using System;

namespace Chat.api.Model
{
    public class JsonApiMessage
    {
        public readonly string type = "messages";
        public readonly int id;
        public readonly MessageAttributes attributes;
        public readonly JsonApiRelationship relationships;
        public readonly SelfLink links;

        internal JsonApiMessage(Message message, string requestUri)
        {
            id = SessionController.Context.Messages.IndexOf(message);
            attributes = new MessageAttributes(message);
            relationships = new JsonApiRelationship(message.Creator, requestUri);
            links = new SelfLink(requestUri, id);
        }

        public class MessageAttributes
        {
            public readonly DateTime created_at;
            public readonly string message;

            internal MessageAttributes(Message chatMessage)
            {
                created_at = chatMessage.CreatedTime;
                message = chatMessage.Text;
            }
        }
    }
}
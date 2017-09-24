using Chat.api.Model;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;

namespace Chat.api.Controllers
{
    [JwtAuthorize]
    [RoutePrefix("api/v1/messages")]
    public class MessagesController : ApiController
    {
        [Route("")]
        [HttpPost]
        public async System.Threading.Tasks.Task<HttpResponseMessage> CreateAsync()
        {
            var session = ActionContext.ActionArguments["session"] as Session;
            var msg = JsonConvert.DeserializeAnonymousType(await Request.Content.ReadAsStringAsync(),
                new
                {
                    data = new
                    {
                        type = string.Empty,
                        attributes = new
                        {
                            message = string.Empty
                        }
                    }
                });
            var text = msg.data.attributes.message;
            var message = SessionController.Context.CreateMessage(session, text);
            var sessionId = SessionController.Context.Sessions.IndexOf(session);
            var messageId = SessionController.Context.Messages.IndexOf(message);
            var result = new
            {
                data = new
                {
                    type = "messages",
                    id = messageId,
                    attributes = new { created_at = session.CreatedTime, message = message.Text },
                    relationships = new
                    {
                        creator = new
                        {
                            links = new
                            {
                                self = $"{Request.RequestUri}/{messageId}/relationships/creator",
                                related = $"{Request.RequestUri}/{messageId}/creator"
                            }
                        },
                        data = new
                        {
                            type = "users",
                            id = sessionId
                        }
                    },
                    links = new
                    {
                        self = $"{Request.RequestUri}/{messageId}"
                    }
                },
                included = new[]
                {
                    new
                    {
                        type ="users",
                        id = sessionId,
                        attributes = new
                        {
                            username = session.Creator
                        },
                        links = new
                        {
                            self = $"{Request.RequestUri.ToString().Replace("messages","users")}/{sessionId}"
                        }
                    }
                },
                meta = new { }
            };
            var resp = Request.CreateResponse(HttpStatusCode.Created, result);
            resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.api+json");
            resp.Headers.Add("authorization", session.SecurityToken);
            return resp;
        }
    }
}

using Chat.api.Model;
using Newtonsoft.Json;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;

namespace Chat.api.Controllers
{
    [RoutePrefix("api/v1/sessions")]
    public class SessionController : ApiController
    {
        private static readonly SessionContext Context = new SessionContext();

        [Route("")]
        [HttpPost]
        public HttpResponseMessage Create()
        {
            var session = Context.CreateSession();
            var result = new
            {
                data = new
                {
                    type = "sessions",
                    id = Context.Sessions.Count,
                    attributes = new { created_at = session.CreatedTime },
                    relationships = new
                    {
                        creator = new
                        {
                            links = new
                            {
                                self = $"{Request.RequestUri}/{Context.Sessions.Count}/relationships/creator",
                                related = $"{Request.RequestUri}/{Context.Sessions.Count}/creator"
                            }
                        },
                        data = new
                        {
                            type = "users",
                            id = Context.Users.Count
                        }
                    },
                    links = new
                    {
                        self = $"{Request.RequestUri}/{Context.Sessions.Count}"
                    }
                },
                included = new[]
                {
                    new
                    {
                        type ="users",
                        id = Context.Users.Count,
                        attributes = new
                        {
                            username = session.Creator.Username
                        },
                        links = new
                        {
                            self = $"{Request.RequestUri.ToString().Replace("sessions","users")}/{Context.Users.Count}"
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

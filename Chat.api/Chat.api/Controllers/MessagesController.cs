using Chat.api.Model;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Chat.api.Controllers
{
    [JwtAuthorize]
    [RoutePrefix("api/v1/messages")]
    public class MessagesController : ApiController
    {
        [Route("")]
        [HttpGet]
        public async System.Threading.Tasks.Task<HttpResponseMessage> ListAsync()
        {
            var session = ActionContext.ActionArguments["session"] as Session;
            var query = Request.GetQueryNameValuePairs().ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

            if (!query.TryGetValue("page.number", out string startString) || !int.TryParse(startString, out int start))
                return await BadRequest("Invalid query string").ExecuteAsync(CancellationToken.None);

            if (!query.TryGetValue("page.size", out string sizeString) || !int.TryParse(sizeString, out int size))
                return await BadRequest("Invalid query string").ExecuteAsync(CancellationToken.None);

            var resp = GenerateResponse(start,size);
            resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.api+json");
            resp.Headers.Add("authorization", session.SecurityToken);
            return resp;
        }

        private HttpResponseMessage GenerateResponse(int start, int size)
        {
            var userUri = Request.RequestUri.ToString().Replace("messages", "users");
            var data = SessionController.Context.Messages.Skip(start).Take(size)
                .Select(message => new JsonApiMessage(message, Request.RequestUri.ToString()))
                .ToList();
            var included = data.Select(d => d.relationships.creator.data.id)
                .Distinct()
                .Select(id => new JsonApiUser(userUri, SessionController.Context.Sessions[id].Creator, id));

            var result = new
            {
                data = data,
                included = included,
                meta = new { count = data.Count },
                links = new JsonApiPagination(Request.RequestUri, start, size, SessionController.Context.Messages.Count)
            };
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("")]
        [HttpPost]
        public async System.Threading.Tasks.Task<HttpResponseMessage> CreateAsync()
        {
            var session = ActionContext.ActionArguments["session"] as Session;
            var message = SessionController.Context.CreateMessage(session, await GetMesageTextAsync());
            var resp = GenerateResponse(message, session);
            resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.api+json");
            resp.Headers.Add("authorization", session.SecurityToken);
            return resp;
        }

        private async System.Threading.Tasks.Task<string> GetMesageTextAsync()
        {
            var msg = JsonConvert.DeserializeAnonymousType(await Request.Content.ReadAsStringAsync(),
                new { data = new { type = string.Empty, attributes = new { message = string.Empty } } });
            return msg.data.attributes.message;
        }

        private HttpResponseMessage GenerateResponse(Message message, Session session)
        {
            var data = new JsonApiMessage(message, Request.RequestUri.ToString());
            
            var result = new
            {
                data = data,
                included = new[] { new JsonApiUser(Request.RequestUri.ToString().Replace("messages","users"), session.Creator, data.relationships.creator.data.id) },
                meta = new { }
            };
            return Request.CreateResponse(HttpStatusCode.Created, result);
        }
    }
}

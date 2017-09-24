﻿using Chat.api.Model;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;

namespace Chat.api.Controllers
{
    [RoutePrefix("api/v1/sessions")]
    public class SessionController : ApiController
    {
        public static readonly ChatContext Context = new ChatContext();

        [Route("")]
        [HttpPost]
        public HttpResponseMessage Create()
        {
            var session = Context.CreateSession();
            var id = Context.Sessions.IndexOf(session);
            var result = new
            {
                data = new
                {
                    type = "sessions",
                    id = id,
                    attributes = new { created_at = session.CreatedTime },
                    relationships = new
                    {
                        creator = new
                        {
                            links = new
                            {
                                self = $"{Request.RequestUri}/{id}/relationships/creator",
                                related = $"{Request.RequestUri}/{id}/creator"
                            }
                        },
                        data = new
                        {
                            type = "users",
                            id = id
                        }
                    },
                    links = new
                    {
                        self = $"{Request.RequestUri}/{id}"
                    }
                },
                included = new[]
                {
                    new
                    {
                        type ="users",
                        id = id,
                        attributes = new
                        {
                            username = session.Creator
                        },
                        links = new
                        {
                            self = $"{Request.RequestUri.ToString().Replace("sessions","users")}/{id}"
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

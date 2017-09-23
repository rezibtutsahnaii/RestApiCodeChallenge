using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Chat.api.Controllers
{
    [JwtAuthorize]
    [RoutePrefix("api/v1/messages")]
    public class MessagesController : ApiController
    {
        [Route("")]
        public IHttpActionResult Get()
        {
            return Ok("WHAT?!");
        }
    }
}
